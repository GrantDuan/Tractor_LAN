using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using Duan.Xiugang.Tractor.Objects;
using Duan.Xiugang.Tractor.Player;
using Duan.Xiugang.Tractor.Properties;
using Kuaff.CardResouces;

namespace Duan.Xiugang.Tractor
{
    internal partial class MainForm : Form
    {
        #region ��������

        //������ͼ��
        internal Dictionary<string, int> PlayerPosition;
        internal Dictionary<int, string> PositionPlayer;
        internal int Scores = 0;
        internal List<int> SelectedCards;
        internal TractorPlayer ThisPlayer;
        internal object[] UserAlgorithms = {null, null, null, null};
        internal Bitmap bmp = null;
        internal CalculateRegionHelper calculateRegionHelper = null;
        internal Bitmap[] cardsImages = new Bitmap[54];
        internal int cardsOrderNumber = 0;

        internal CurrentPoker[] currentAllSendPokers =
        {
            new CurrentPoker(), new CurrentPoker(), new CurrentPoker(),
            new CurrentPoker()
        };

        //ԭʼ����ͼƬ

        //��ͼ�Ĵ��������ڷ���ʱʹ�ã�
        internal int currentCount = 0;

        internal CurrentPoker[] currentPokers =
        {
            new CurrentPoker(), new CurrentPoker(), new CurrentPoker(),
            new CurrentPoker()
        };

        internal int currentRank = 0;

        //��ǰһ�ָ��ҵĳ������
        internal ArrayList[] currentSendCards = new ArrayList[4];
        internal CurrentState currentState;
        internal DrawingFormHelper drawingFormHelper = null;
        //Ӧ��˭����
        //һ�γ�����˭���ȿ�ʼ������
        internal int firstSend = 0;
        internal GameConfig gameConfig = new GameConfig();
        internal Bitmap image = null;
        internal bool isNew = true;
        private string musicFile = "";
        internal ArrayList myCardIsReady = new ArrayList();

        //*��������
        //��ǰ�����Ƶ�����
        internal ArrayList myCardsLocation = new ArrayList();
        //��ǰ�����Ƶ���ֵ
        internal ArrayList myCardsNumber = new ArrayList();
        internal ArrayList[] pokerList = null;
        //��ǰ�����Ƶ��Ƿ񱻵��
        //��ǰ�۵׵���
        internal ArrayList send8Cards = new ArrayList();
        internal int showSuits = 0;

        //*���ҵ��Ƶĸ�������
        //����˳��
        internal long sleepMaxTime = 2000;
        internal long sleepTime;
        internal CardCommands wakeupCardCommands;

        //*�滭������
        //DrawingForm����

        //����ʱĿǰ��������һ��
        internal int whoIsBigger = 0;
        internal int whoShowRank = 0;
        internal int whoseOrder = 0; //0δ��,1�ң�2�Լң�3����,4����


        //�����ļ�

        #endregion // ��������

        internal MainForm()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);


            //��ȡ��������
            InitAppSetting();

            notifyIcon.Text = Text;
            BackgroundImage = image;

            //������ʼ��
            bmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            ThisPlayer = new TractorPlayer();

            ThisPlayer.PlayerOnGetCard += PlayerGetCard;
            ThisPlayer.GameOnStarted += StartGame;
            ThisPlayer.TrumpChanged += ThisPlayer_TrumpUpdated;
            ThisPlayer.AllCardsGot += ResortMyCards;
            ThisPlayer.PlayerShowedCards += ThisPlayer_PlayerShowedCards;
            ThisPlayer.ShowingCardBegan += ThisPlayer_ShowingCardBegan;
            ThisPlayer.NewPlayerJoined += ThisPlayer_NewPlayerJoined;
            ThisPlayer.PlayersTeamMade += ThisPlayer_PlayersTeamMade;
            ThisPlayer.TrickFinished += ThisPlayer_TrickFinished;
            ThisPlayer.HandEnding += ThisPlayer_HandEnding;
            ThisPlayer.StarterFailedForTrump += ThisPlayer_StarterFailedForTrump;
            ThisPlayer.Last8Discarded += ThisPlayer_Last8Discarded;
            ThisPlayer.DiscardingLast8 += ThisPlayer_DiscardingLast8;
            ThisPlayer.DumpingFail += ThisPlayer_DumpingFail;
            SelectedCards = new List<int>();
            PlayerPosition = new Dictionary<string, int>();
            PositionPlayer = new Dictionary<int, string>();
            drawingFormHelper = new DrawingFormHelper(this);
            calculateRegionHelper = new CalculateRegionHelper(this);

            for (int i = 0; i < 54; i++)
            {
                cardsImages[i] = null; //��ʼ��
            }
        }


        private void InitAppSetting()
        {
            //û�������ļ������config�ļ��ж�ȡ
            if (!File.Exists("gameConfig"))
            {
                var reader = new AppSettingsReader();
                try
                {
                    Text = (String) reader.GetValue("title", typeof (String));
                }
                catch (Exception ex)
                {
                    Text = "��������ս";
                }

                try
                {
                    gameConfig.MustRank = (String) reader.GetValue("mustRank", typeof (String));
                }
                catch (Exception ex)
                {
                    gameConfig.MustRank = ",3,8,11,12,13,";
                }

                try
                {
                    gameConfig.IsDebug = (bool) reader.GetValue("debug", typeof (bool));
                }
                catch (Exception ex)
                {
                    gameConfig.IsDebug = false;
                }

                try
                {
                    gameConfig.BottomAlgorithm = (int) reader.GetValue("bottomAlgorithm", typeof (int));
                }
                catch (Exception ex)
                {
                    gameConfig.BottomAlgorithm = 1;
                }
            }
            else
            {
                //ʵ�ʴ�gameConfig�ļ��ж�ȡ
                Stream stream = null;
                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    stream = new FileStream("gameConfig", FileMode.Open, FileAccess.Read, FileShare.Read);
                    gameConfig = (GameConfig) formatter.Deserialize(stream);
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }

            //δ���л���ֵ
            var myreader = new AppSettingsReader();
            gameConfig.CardsResourceManager = Kuaff_Cards.ResourceManager;
            try
            {
                var bkImage = (String) myreader.GetValue("backImage", typeof (String));
                image = new Bitmap(bkImage);
            }
            catch (Exception ex)
            {
                image = Resources.Backgroud;
            }

            try
            {
                Text = (String) myreader.GetValue("title", typeof (String));
            }
            catch (Exception ex)
            {
            }

            gameConfig.CardImageName = "";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ThisPlayer.Quit();
            }
            catch (Exception)
            {
            }
        }

        //������ͣ�����ʱ�䣬�Լ���ͣ�������ִ������
        internal void SetPauseSet(int max, CardCommands wakeup)
        {
            sleepMaxTime = max;
            sleepTime = DateTime.Now.Ticks;
            wakeupCardCommands = wakeup;
            currentState.CurrentCardCommands = CardCommands.Pause;
        }

        #region �����¼��������

        private void init()
        {
            //ÿ�γ�ʼ�����ػ汳��
            Graphics g = Graphics.FromImage(bmp);
            drawingFormHelper.DrawBackground(g);

            //Ŀǰ�����Է���
            showSuits = 0;
            whoShowRank = 0;

            //�÷�����
            Scores = 0;


            //����Sidebar
            drawingFormHelper.DrawSidebar(g);
            //���ƶ�������

            drawingFormHelper.Starter();

            //����Rank
            drawingFormHelper.Rank();


            //���ƻ�ɫ
            drawingFormHelper.Trump();

            send8Cards = new ArrayList();
            //������ɫ
            if (currentRank == 53)
            {
                currentState.Suit = 5;
            }

            Refresh();
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            //���
            //ֻ�з���ʱ�͸��ҳ���ʱ������Ӧ����¼�
            if (ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.Playing ||
                ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.DiscardingLast8Cards)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if ((e.X >= (int) myCardsLocation[0] &&
                         e.X <= ((int) myCardsLocation[myCardsLocation.Count - 1] + 71)) && (e.Y >= 355 && e.Y < 472))
                    {
                        if (calculateRegionHelper.CalculateClickedRegion(e, 1))
                        {
                            drawingFormHelper.DrawMyPlayingCards(ThisPlayer.CurrentPoker);
                            Refresh();
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right) //�Ҽ�
                {
                    ToDiscard8Cards();
                    ToShowCards();
                }


                //�ж��Ƿ�����С��*********�����ϵĵ����ͬ
                var pigRect = new Rectangle(296, 300, 53, 46);
                var region = new Region(pigRect);
                if (region.IsVisible(e.X, e.Y))
                {
                    if (SelectedCards.Count > 0)
                    {
                        ToDiscard8Cards();
                        ToShowCards();
                    }
                }
            }
            else if (ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.DistributingCards ||
                     ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.DistributingCardsFinished ||
                     ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.DiscardingLast8CardsFinished)
            {
                ExposeTrump(e);
            }
        }

        private void ExposeTrump(MouseEventArgs e)
        {
            List<Suit> availableTrumps = ThisPlayer.AvailableTrumps();
            var trumpExposingToolRegion = new Dictionary<Suit, Region>();
            var spadeRegion = new Region(new Rectangle(443, 327, 25, 25));
            trumpExposingToolRegion.Add(Suit.Spade, spadeRegion);
            var heartRegion = new Region(new Rectangle(417, 327, 25, 25));
            trumpExposingToolRegion.Add(Suit.Heart, heartRegion);
            var clubRegion = new Region(new Rectangle(493, 327, 25, 25));
            trumpExposingToolRegion.Add(Suit.Club, clubRegion);
            var diamondRegion = new Region(new Rectangle(468, 327, 25, 25));
            trumpExposingToolRegion.Add(Suit.Diamond, diamondRegion);
            var jokerRegion = new Region(new Rectangle(518, 327, 25, 25));
            trumpExposingToolRegion.Add(Suit.Joker, jokerRegion);
            foreach (var keyValuePair in trumpExposingToolRegion)
            {
                if (keyValuePair.Value.IsVisible(e.X, e.Y))
                {
                    foreach (Suit trump in availableTrumps)
                    {
                        if (trump == keyValuePair.Key)
                        {
                            var next =
                                (TrumpExposingPoker)
                                    (Convert.ToInt32(ThisPlayer.CurrentHandState.TrumpExposingPoker) + 1);
                            if (trump == Suit.Joker)
                            {
                                if (ThisPlayer.CurrentPoker.BlackJoker == 2)
                                    next = TrumpExposingPoker.PairBlackJoker;
                                else if (ThisPlayer.CurrentPoker.RedJoker == 2)
                                    next = TrumpExposingPoker.PairRedJoker;
                            }
                            ThisPlayer.ExposeTrump(next, trump);
                        }
                    }
                }
            }
        }

        private void MainForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //�����ǰû���ƿɳ� 
            if (ThisPlayer.CurrentPoker.Count == 0)
            {
                return;
            }


            if (SelectedCards.Count == 0)
                return;
            ToDiscard8Cards();
            ToShowCards();
        }

        private void ToDiscard8Cards()
        {
            var pigRect = new Rectangle(296, 300, 53, 46);
            //�ж��Ƿ��ڿ��ƽ׶�
            if (ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.DiscardingLast8Cards &&
                ThisPlayer.CurrentHandState.Last8Holder == ThisPlayer.PlayerId) //������ҿ���
            {
                if (SelectedCards.Count == 8)
                {
                    //����,���Բ�ȥС��
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(image, pigRect, pigRect, GraphicsUnit.Pixel);
                    g.Dispose();

                    foreach (int card in SelectedCards)
                    {
                        ThisPlayer.CurrentPoker.RemoveCard(card);
                    }

                    ThisPlayer.DiscardCards(SelectedCards.ToArray());

                    ResortMyCards();
                }
            }
        }

        private void ToShowCards()
        {
            var pigRect = new Rectangle(296, 300, 53, 46);
            Graphics g = Graphics.FromImage(bmp);
            if (ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.Playing &&
                ThisPlayer.CurrentTrickState.NextPlayer() == ThisPlayer.PlayerId)
            {
                ShowingCardsValidationResult showingCardsValidationResult =
                    TractorRules.IsValid(ThisPlayer.CurrentTrickState, SelectedCards, ThisPlayer.CurrentPoker);
                //�����׼�������ƺϷ�
                if (showingCardsValidationResult.ResultType == ShowingCardsValidationResultType.Valid)
                {
                    //��ȥС��
                    g.DrawImage(image, pigRect, pigRect, GraphicsUnit.Pixel);

                    foreach (int card in SelectedCards)
                    {
                        ThisPlayer.CurrentPoker.RemoveCard(card);
                    }
                    ThisPlayer.ShowCards(SelectedCards);
                    drawingFormHelper.DrawMyShowedCards();
                    SelectedCards.Clear();
                }
                else if (showingCardsValidationResult.ResultType == ShowingCardsValidationResultType.TryToDump)
                {
                    //��ȥС��
                    g.DrawImage(image, pigRect, pigRect, GraphicsUnit.Pixel);

                    ShowingCardsValidationResult result = ThisPlayer.ValidateDumpingCards(SelectedCards);
                    if (result.ResultType == ShowingCardsValidationResultType.DumpingSuccess) //˦�Ƴɹ�.
                    {
                        foreach (int card in SelectedCards)
                        {
                            ThisPlayer.CurrentPoker.RemoveCard(card);
                        }
                        ThisPlayer.ShowCards(SelectedCards);

                        drawingFormHelper.DrawMyShowedCards();
                        SelectedCards.Clear();
                    }
                        //˦��ʧ��
                    else
                    {
                        foreach (int card in result.MustShowCardsForDumpingFail)
                        {
                            ThisPlayer.CurrentPoker.RemoveCard(card);
                        }
                        Thread.Sleep(2000);
                        ThisPlayer.ShowCards(result.MustShowCardsForDumpingFail);

                        SelectedCards = result.MustShowCardsForDumpingFail;
                        SelectedCards.Clear();
                    }
                }
            }
            g.Dispose();
        }

        //���ڻ滭����,��������ͼ�񻭵�������
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //��bmp����������
            g.DrawImage(bmp, 0, 0);
        }

        #endregion

        #region �˵��¼�����

        internal void MenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem) sender;
            if (menuItem.Text.Equals("�˳�"))
            {
                Close();
            }

            if (menuItem.Text.Equals("��ʼ����Ϸ"))
            {
                ThisPlayer.Ready();


                //��ʼ��
                init();
            }
        }

        //�����¼�����
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            Show();
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            Activate();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Visible = false;
                notifyIcon.Visible = true;
            }
            else
            {
                notifyIcon.Visible = false;
            }
        }
    

        #endregion // �˵��¼�����

        #region handle player event

        private void StartGame()
        {
            init();
        }

        private void PlayerGetCard(int cardNumber)
        {
            drawingFormHelper.IGetCard(cardNumber);
        }


        private void ThisPlayer_TrumpUpdated(CurrentHandState currentHandState)
        {
            ThisPlayer.CurrentHandState = currentHandState;
            drawingFormHelper.Trump();
            drawingFormHelper.TrumpMadeCardsShow();
            drawingFormHelper.ReDrawToolbar();
            if (ThisPlayer.CurrentHandState.IsFirstHand)
            {
                drawingFormHelper.Rank();
                drawingFormHelper.Starter();
            }
        }

        private void ResortMyCards()
        {
            drawingFormHelper.DrawMySortedCards(ThisPlayer.CurrentPoker, ThisPlayer.CurrentPoker.Count);
        }


        private void ThisPlayer_PlayerShowedCards()
        {
            //������һ��
            if (ThisPlayer.CurrentTrickState.CountOfPlayerShowedCards() == 1)
            {
                drawingFormHelper.DrawCenterImage();
                drawingFormHelper.DrawScoreImage();
            }

            string latestPlayer = ThisPlayer.CurrentTrickState.LatestPlayerShowedCard();
            int position = PlayerPosition[latestPlayer];
            if (latestPlayer == ThisPlayer.PlayerId)
            {
                drawingFormHelper.DrawMyShowedCards();
            }
            if (position == 2)
            {
                drawingFormHelper.DrawNextUserSendedCards();
            }
            if (position == 3)
            {
                drawingFormHelper.DrawFriendUserSendedCards();
            }
            if (position == 4)
            {
                drawingFormHelper.DrawPreviousUserSendedCards();
            }

            if (ThisPlayer.CurrentTrickState.NextPlayer() == ThisPlayer.PlayerId)
                drawingFormHelper.DrawMyPlayingCards(ThisPlayer.CurrentPoker);
        }

        private void ThisPlayer_ShowingCardBegan()
        {
            ThisPlayer_DiscardingLast8();
            drawingFormHelper.RemoveToolbar();
            drawingFormHelper.ClearSuitCards();
        }

        private void ThisPlayer_PlayersTeamMade()
        {
            //set player position
            PlayerPosition.Clear();
            PositionPlayer.Clear();
            string nextPlayer = ThisPlayer.PlayerId;
            int postion = 1;
            PlayerPosition.Add(nextPlayer, postion);
            PositionPlayer.Add(postion, nextPlayer);
            nextPlayer = ThisPlayer.CurrentGameState.GetNextPlayerAfterThePlayer(nextPlayer).PlayerId;
            while (nextPlayer != ThisPlayer.PlayerId)
            {
                postion++;
                PlayerPosition.Add(nextPlayer, postion);
                PositionPlayer.Add(postion, nextPlayer);
                nextPlayer = ThisPlayer.CurrentGameState.GetNextPlayerAfterThePlayer(nextPlayer).PlayerId;
            }
        }

        private void ThisPlayer_NewPlayerJoined()
        {
        }

        private void ThisPlayer_TrickFinished()
        {
            drawingFormHelper.DrawScoreImage();

            drawingFormHelper.DrawWhoWinThisTime();
            Refresh();
        }

        private void ThisPlayer_HandEnding()
        {
            drawingFormHelper.DrawFinishedSendedCards();
        }

        private void ThisPlayer_StarterFailedForTrump()
        {
            Graphics g = Graphics.FromImage(bmp);

            //����Sidebar
            drawingFormHelper.DrawSidebar(g);
            //���ƶ�������

            drawingFormHelper.Starter();

            //����Rank
            drawingFormHelper.Rank();

            //���ƻ�ɫ
            drawingFormHelper.Trump();

            ResortMyCards();

            drawingFormHelper.ReDrawToolbar();

            Refresh();
            g.Dispose();
        }

        private void ThisPlayer_Last8Discarded()
        {
            Graphics g = Graphics.FromImage(bmp);
            for (int i = 0; i < 8; i++)
            {
                g.DrawImage(gameConfig.BackImage, 200 + i*2, 186, 71, 96);
            }
            Refresh();
            g.Dispose();
        }

        private void ThisPlayer_DiscardingLast8()
        {
            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(image, new Rectangle(200, 186, 85, 96), new Rectangle(200, 186, 85, 96), GraphicsUnit.Pixel);
            Refresh();
            g.Dispose();
        }

        private void ThisPlayer_DumpingFail(ShowingCardsValidationResult result)
        {
            //������һ��
            if (ThisPlayer.CurrentTrickState.AllPlayedShowedCards() || ThisPlayer.CurrentTrickState.IsStarted() == false)
            {
                drawingFormHelper.DrawCenterImage();
                drawingFormHelper.DrawScoreImage();
            }
            ThisPlayer.CurrentTrickState.ShowedCards[result.PlayerId] = result.CardsToShow;

            string latestPlayer = result.PlayerId;
            int position = PlayerPosition[latestPlayer];
            if (latestPlayer == ThisPlayer.PlayerId)
            {
                drawingFormHelper.DrawMyShowedCards();
            }
            if (position == 2)
            {
                drawingFormHelper.DrawNextUserSendedCards();
            }
            if (position == 3)
            {
                drawingFormHelper.DrawFriendUserSendedCards();
            }
            if (position == 4)
            {
                drawingFormHelper.DrawPreviousUserSendedCards();
            }
        }

        #endregion
    }
}