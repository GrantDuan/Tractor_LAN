using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Kuaff.CardResouces;
using Duan.Xiugang.Tractor.Player;
using Duan.Xiugang.Tractor.Objects;

namespace Duan.Xiugang.Tractor
{
    /// <summary>
    /// ʵ�ִ󲿷ֵĻ滭����
    /// </summary>
    class DrawingFormHelper
    {
        MainForm mainForm;
        internal DrawingFormHelper(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }


        #region ���ƶ���

        internal void IGetCard(int cardNumber)
        {
            //�õ�������ͼ���Graphics
            Graphics g = Graphics.FromImage(mainForm.bmp);
            DrawAnimatedCard(getPokerImageByNumber(cardNumber), 260, 280, 71, 96);
            DrawMyCards(g, mainForm.ThisPlayer.CurrentPoker, mainForm.ThisPlayer.CurrentPoker.Count);

            ReDrawToolbar();

            mainForm.Refresh();
            g.Dispose();
        }

        internal void TrumpMadeCardsShow()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            var trumpMadeCard = (int)mainForm.ThisPlayer.CurrentHandState.Trump * 13 - 13 + mainForm.ThisPlayer.CurrentHandState.Rank;
            if (mainForm.ThisPlayer.CurrentHandState.TrumpExposingPoker == TrumpExposingPoker.PairBlackJoker)
                trumpMadeCard = 52;
            else if (mainForm.ThisPlayer.CurrentHandState.TrumpExposingPoker == TrumpExposingPoker.PairRedJoker)
                trumpMadeCard = 53;

            if (mainForm.ThisPlayer.CurrentHandState.TrumpExposingPoker ==  TrumpExposingPoker.SingleRank)
            {
                if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 3)
                {
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 437, 124, 71, 96);
                }
                else if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 4)
                {

                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 80, 158, 71, 96);
                }
                else if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 2)
                {
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 480, 200, 71, 96);
                }
            }
            else if (mainForm.ThisPlayer.CurrentHandState.TrumpExposingPoker > TrumpExposingPoker.SingleRank)
            {
                if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 3)
                {
                    ClearSuitCards(g);
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 423, 124, 71, 96);
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 437, 124, 71, 96);
                }
                else if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 4)
                {
                    ClearSuitCards(g);
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 80, 158, 71, 96);
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 80, 178, 71, 96);

                }
                else if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 2)
                {
                    ClearSuitCards(g);
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 480, 200, 71, 96);
                    g.DrawImage(getPokerImageByNumber(trumpMadeCard), 480, 220, 71, 96);
                }
                else if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 1)
                {
                    ClearSuitCards(g);                    
                }
            }
            mainForm.Refresh();
            g.Dispose();
        }
        //���������
        internal void ClearSuitCards(Graphics g)
        {
            g.DrawImage(mainForm.image, new Rectangle(80, 158, 71, 116), new Rectangle(80, 158, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(480, 200, 71, 116), new Rectangle(480, 200, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(423, 124, 85, 96), new Rectangle(423, 124, 85, 96), GraphicsUnit.Pixel);
        }

        //���������
        internal void ClearSuitCards()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(mainForm.image, new Rectangle(80, 158, 71, 116), new Rectangle(80, 158, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(480, 200, 71, 116), new Rectangle(480, 200, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(423, 124, 85, 96), new Rectangle(423, 85, 71, 96), GraphicsUnit.Pixel);
            mainForm.Refresh();
            g.Dispose();
        }

        #endregion // ���ƶ���

        #region ������λ�õ���
        /// <summary>
        /// ����ʱ���������.
        /// ���ȴӵ�ͼ��ȡ��Ӧ��λ�ã��ػ���鱳����
        /// Ȼ�����Ƶı��滭58-count*2���ơ�
        /// 
        /// </summary>
        /// <param name="g">������ͼƬ��Graphics</param>
        /// <param name="num">�Ƶ�����=58-���ƴ���*2</param>
        internal void DrawCenterAllCards(Graphics g, int num)
        {
            Rectangle rect = new Rectangle(200, 186, (num + 1) * 2 + 71, 96);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);

            for (int i = 0; i < num; i++)
            {
                g.DrawImage(mainForm.gameConfig.BackImage, 200 + i * 2, 186, 71, 96);
            }
        }

        /// <summary>
        /// ����һ���ƣ���Ҫ�����������
        /// </summary>
        internal void DrawCenterImage()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Rectangle rect = new Rectangle(77, 124, 476, 244);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
            g.Dispose();
            mainForm.Refresh();
        }

        /// <summary>
        /// ������ͼƬ
        /// </summary>
        internal void DrawPassImage()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Rectangle rect = new Rectangle(110, 150, 400, 199);
            g.DrawImage(Properties.Resources.Pass, rect);
            g.Dispose();
            mainForm.Refresh();
        }
        #endregion // ������λ�õ���

        #region ���ƴ���

        internal void DrawDiscardedLast8Cards()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Rectangle rect = new Rectangle(200, 186, 90, 96);
            Rectangle backRect = new Rectangle(77, 121, 477, 254);
            //���8�ŵ�ͼ��ȡ����
            Bitmap backup = mainForm.bmp.Clone(rect, PixelFormat.DontCare);
            //����λ���ñ�������
            //g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, backRect, backRect, GraphicsUnit.Pixel);
            mainForm.Refresh();
            g.Dispose();
        }
        //�յ��ƵĶ���
        /// <summary>
        /// ����25�κ����ʣ��8����.
        /// ��ʱ�Ѿ�ȷ����ׯ�ң���8���ƽ���ׯ��,
        /// ͬʱ�Զ����ķ�ʽ��ʾ��
        /// </summary>
        internal void DrawCenter8Cards()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Rectangle rect = new Rectangle(200, 186, 90, 96);
            Rectangle backRect = new Rectangle(77, 121, 477, 254);
            //���8�ŵ�ͼ��ȡ����
            Bitmap backup = mainForm.bmp.Clone(rect, PixelFormat.DontCare);
            //����λ���ñ�������
            //g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, backRect, backRect, GraphicsUnit.Pixel);

            //������8�Ž���ׯ�ң�������ʽ��
            if (mainForm.currentState.Master == 1)
            {
                DrawAnimatedCard(backup, 300, 330, 90, 96);
                Get8Cards(mainForm.pokerList[0], mainForm.pokerList[1], mainForm.pokerList[2], mainForm.pokerList[3]);
            }
            else if (mainForm.currentState.Master == 2)
            {
                DrawAnimatedCard(backup, 200, 80, 90, 96);
                Get8Cards(mainForm.pokerList[1], mainForm.pokerList[0], mainForm.pokerList[2], mainForm.pokerList[3]);
            }
            else if (mainForm.currentState.Master == 3)
            {
                DrawAnimatedCard(backup, 70, 186, 90, 96);
                Get8Cards(mainForm.pokerList[2], mainForm.pokerList[1], mainForm.pokerList[0], mainForm.pokerList[3]);
            }
            else if (mainForm.currentState.Master == 4)
            {
                DrawAnimatedCard(backup, 400, 186, 90, 96);
                Get8Cards(mainForm.pokerList[3], mainForm.pokerList[1], mainForm.pokerList[2], mainForm.pokerList[0]);
            }
            mainForm.Refresh();

            g.Dispose();
        }
        //�����8�Ž���ׯ��
        private void Get8Cards(ArrayList list0, ArrayList list1, ArrayList list2, ArrayList list3)
        {
            list0.Add(list1[25]);
            list0.Add(list1[26]);
            list0.Add(list2[25]);
            list0.Add(list2[26]);
            list0.Add(list3[25]);
            list0.Add(list3[26]);
            list1.RemoveAt(26);
            list1.RemoveAt(25);
            list2.RemoveAt(26);
            list2.RemoveAt(25);
            list3.RemoveAt(26);
            list3.RemoveAt(25);
        }

        internal void DrawBottomCards(ArrayList bottom)
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);

            //������,��169��ʼ��
            for (int i = 0; i < 8; i++)
            {
                if (i == 2)
                {
                    g.DrawImage(getPokerImageByNumber((int)bottom[i]), 230 + i * 14, 146, 71, 96);
                }
                else
                {
                    g.DrawImage(getPokerImageByNumber((int)bottom[i]), 230 + i * 14, 186, 71, 96);
                }
            }

            mainForm.Refresh();

            g.Dispose();
        }
        #endregion // ���ƴ���


        #region ����Sidebar��toolbar
        /// <summary>
        /// ����Sidebar
        /// </summary>
        /// <param name="g"></param>
        internal void DrawSidebar(Graphics g)
        {
            DrawMyImage(g, Properties.Resources.Sidebar, 20, 30, 70, 89);
            DrawMyImage(g, Properties.Resources.Sidebar, 540, 30, 70, 89);
        }
        /// <summary>
        /// �������ϱ�
        /// </summary>
        /// <param name="g">������ͼ���Graphics</param>
        /// <param name="who">��˭</param>
        /// <param name="b">�Ƿ���ɫ</param>
        internal void DrawMaster(Graphics g, int who, int start)
        {
            if (who < 1 || who > 4)
            {
                return;
            }

            start = start * 80;

            int X = 0;

            if (who == 1)
            {
                start += 40;
                X = 548;
            }
            else if (who == 2)
            {
                start += 60;
                X = 580;
            }
            else if (who == 3)
            {
                start += 0;
                X = 30;
            }
            else if (who == 4)
            {
                start += 20;
                X = 60;
            }

            Rectangle destRect = new Rectangle(X, 45, 20, 20);
            Rectangle srcRect = new Rectangle(start, 0, 20, 20);

            g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);

        }

        /// <summary>
        /// ��������ɫ
        /// </summary>
        /// <param name="g"></param>
        /// <param name="who"></param>
        /// <param name="start"></param>
        internal void DrawOtherMaster(Graphics g, int who, int start)
        {


            if (who != 1)
            {
                Rectangle destRect = new Rectangle(548, 45, 20, 20);
                Rectangle srcRect = new Rectangle(40, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            if (who != 2)
            {
                Rectangle destRect = new Rectangle(580, 45, 20, 20);
                Rectangle srcRect = new Rectangle(60, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            if (who != 3)
            {
                Rectangle destRect = new Rectangle(31, 45, 20, 20);
                Rectangle srcRect = new Rectangle(0, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            if (who != 4)
            {
                Rectangle destRect = new Rectangle(61, 45, 20, 20);
                Rectangle srcRect = new Rectangle(20, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }

        }


        /// <summary>
        /// ����Rank
        /// </summary>
        internal void Rank()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);

            int rankofNorthSouth = 0;
            int rankofEastWest = 0;

            if (mainForm.ThisPlayer.CurrentGameState.Players.Find(p => p.PlayerId == mainForm.ThisPlayer.PlayerId) != null)
                rankofNorthSouth = mainForm.ThisPlayer.CurrentGameState.Players.Find(p => p.PlayerId == mainForm.ThisPlayer.PlayerId).Rank;
            if (mainForm.ThisPlayer.CurrentGameState.GetNextPlayerAfterThePlayer(mainForm.ThisPlayer.PlayerId) != null)
                rankofEastWest = mainForm.ThisPlayer.CurrentGameState.GetNextPlayerAfterThePlayer(mainForm.ThisPlayer.PlayerId).Rank;

            bool starterIsKnown = false;
            bool starterInMyTeam = false;
            if (!string.IsNullOrEmpty(mainForm.ThisPlayer.CurrentHandState.Starter))
            {
                starterInMyTeam = mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.Starter] == 1 ||
                          mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.Starter] == 3;
                starterIsKnown = true;
            }


            Rectangle northSouthRect = new Rectangle(566, 68, 20, 20);
            Rectangle eastWestRect = new Rectangle(46, 68, 20, 20);



            //Ȼ��������д��
            if (!starterIsKnown)
            {
                g.DrawImage(Properties.Resources.Sidebar, northSouthRect, new Rectangle(26, 38, 20, 20), GraphicsUnit.Pixel);
                g.DrawImage(Properties.Resources.Sidebar, eastWestRect, new Rectangle(26, 38, 20, 20), GraphicsUnit.Pixel);

                g.DrawImage(Properties.Resources.CardNumber, northSouthRect, getCardNumberImage(rankofNorthSouth, false), GraphicsUnit.Pixel);
                g.DrawImage(Properties.Resources.CardNumber, eastWestRect, getCardNumberImage(rankofEastWest, false), GraphicsUnit.Pixel);
            }
            else
            {
                g.DrawImage(Properties.Resources.Sidebar, northSouthRect, new Rectangle(26, 38, 20, 20), GraphicsUnit.Pixel);
                g.DrawImage(Properties.Resources.Sidebar, eastWestRect, new Rectangle(26, 38, 20, 20), GraphicsUnit.Pixel);

                if (starterInMyTeam)
                {
                    g.DrawImage(Properties.Resources.CardNumber, northSouthRect, getCardNumberImage(rankofNorthSouth, true), GraphicsUnit.Pixel);
                    g.DrawImage(Properties.Resources.CardNumber, eastWestRect, getCardNumberImage(rankofEastWest, false), GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(Properties.Resources.CardNumber, northSouthRect, getCardNumberImage(rankofNorthSouth, false), GraphicsUnit.Pixel);
                    g.DrawImage(Properties.Resources.CardNumber, eastWestRect, getCardNumberImage(rankofEastWest, true), GraphicsUnit.Pixel);
                }
            }

            mainForm.Refresh();
            g.Dispose();
        }

        private Rectangle getCardNumberImage(int number, bool b)
        {
            Rectangle result = new Rectangle(0, 0, 0, 0);

            if (number >= 0 && number <= 12)
            {
                if (b)
                {
                    number += 14;
                }
                result = new Rectangle(number * 20, 0, 20, 20);
            }


            if ((number == 53) && (b))
            {
                result = new Rectangle(540, 0, 20, 20);
            }
            if ((number == 53) && (!b))
            {
                result = new Rectangle(260, 0, 20, 20);
            }

            return result;
        }
        /// <summary>
        /// ����
        /// </summary>
        internal void Trump()
        {
            CurrentHandState currentHandState = mainForm.ThisPlayer.CurrentHandState;

            Graphics g = Graphics.FromImage(mainForm.bmp);

            Rectangle northSouthRect = new Rectangle(563, 88, 25, 25);
            Rectangle eastWestRect = new Rectangle(43, 88, 25, 25);

            Rectangle trumpRect = new Rectangle(23, 58, 25, 25);//backGroud
            Rectangle backGroupRect = new Rectangle(23, 58, 25, 25);//backGroud
            Rectangle noTrumpRect = new Rectangle(250, 0, 25, 25);

            g.DrawImage(Properties.Resources.Sidebar, northSouthRect, backGroupRect, GraphicsUnit.Pixel);
            g.DrawImage(Properties.Resources.Sidebar, eastWestRect, backGroupRect, GraphicsUnit.Pixel);
            g.DrawImage(Properties.Resources.Suit, northSouthRect, noTrumpRect, GraphicsUnit.Pixel);
            g.DrawImage(Properties.Resources.Suit, eastWestRect, noTrumpRect, GraphicsUnit.Pixel);

            if (currentHandState == null)
                return;

            bool trumpMade = false;
            bool trumpMadeByMyTeam = false;
            if (mainForm.ThisPlayer.CurrentHandState.Trump != Suit.None)
            {
                trumpMade = true;
                trumpMadeByMyTeam = mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 1 ||
                          mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.TrumpMaker] == 3;
            }


            if (!trumpMade)
                return;

            if (currentHandState.Trump == Suit.Heart)
            {
                trumpRect = new Rectangle(0, 0, 25, 25);
            }
            else if (currentHandState.Trump == Suit.Spade)
            {
                trumpRect = new Rectangle(25, 0, 25, 25);
            }
            else if (currentHandState.Trump == Suit.Diamond)
            {
                trumpRect = new Rectangle(50, 0, 25, 25);
            }
            else if (currentHandState.Trump == Suit.Club)
            {
                trumpRect = new Rectangle(75, 0, 25, 25);
            }
            else if (currentHandState.Trump == Suit.Joker)
            {
                trumpRect = new Rectangle(100, 0, 25, 25);
            }

            if (trumpMadeByMyTeam)
            {
                g.DrawImage(Properties.Resources.Sidebar, northSouthRect, backGroupRect, GraphicsUnit.Pixel);
                g.DrawImage(Properties.Resources.Suit, northSouthRect, trumpRect, GraphicsUnit.Pixel);
            }
            else
            {
                g.DrawImage(Properties.Resources.Sidebar, eastWestRect, backGroupRect, GraphicsUnit.Pixel);
                g.DrawImage(Properties.Resources.Suit, eastWestRect, trumpRect, GraphicsUnit.Pixel);
            }

            mainForm.Refresh();
            g.Dispose();
        }


        /// <summary>
        /// ��������
        /// </summary>
        internal void DrawToolbar()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(Properties.Resources.Toolbar, new Rectangle(415, 325, 129, 29), new Rectangle(0, 0, 129, 29), GraphicsUnit.Pixel);
            //�����ְ���ɫ
            g.DrawImage(Properties.Resources.Suit, new Rectangle(417, 327, 125, 25), new Rectangle(125, 0, 125, 25), GraphicsUnit.Pixel);
            g.Dispose();
        }

        /// <summary>
        /// ��ȥ������
        /// </summary>
        internal void RemoveToolbar()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(mainForm.image, new Rectangle(415, 325, 129, 29), new Rectangle(415, 325, 129, 29), GraphicsUnit.Pixel);
            g.Dispose();
        }


        #endregion // ����Sidebar��toolbar



        //�ж����Ƿ�����
        internal void ReDrawToolbar()
        {
            //������������������ж�
            if (mainForm.ThisPlayer.CurrentHandState.Rank == 53)
                return;
            var availableTrump = mainForm.ThisPlayer.AvailableTrumps();
            ReDrawToolbar(availableTrump);


        }

        //���ҵĹ�����
        internal void ReDrawToolbar(List<Suit> suits)
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(Properties.Resources.Toolbar, new Rectangle(415, 325, 129, 29), new Rectangle(0, 0, 129, 29), GraphicsUnit.Pixel);
            //�����ְ���ɫ
            for (int i = 0; i < 5; i++)
            {
                if (suits.Exists(s=> (int)s ==i+1))
                {
                    g.DrawImage(Properties.Resources.Suit, new Rectangle(417 + i * 25, 327, 25, 25), new Rectangle(i * 25, 0, 25, 25), GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(Properties.Resources.Suit, new Rectangle(417 + i * 25, 327, 25, 25), new Rectangle(125 + i * 25, 0, 25, 25), GraphicsUnit.Pixel);
                }
            }
            g.Dispose();
        }


        /// <summary>
        /// �ж�����Ƿ���������.
        /// �����㷨����������������򱾾����֣����·���
        /// </summary>
        /// <returns></returns>
        internal bool DoRankNot()
        {

            if (mainForm.currentState.Suit == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


  


        #region �ڸ�������»��Լ�����

        /// <summary>
        /// �����ڼ���л����ҵ�����.
        /// ���ջ�ɫ�����ƽ������֡�
        /// </summary>
        /// <param name="g">������ͼƬ��Graphics</param>
        /// <param name="currentPoker">�ҵ�ǰ�õ�����</param>
        /// <param name="index">�����Ƶ�����</param>
        internal void DrawMyCards(Graphics g, CurrentPoker currentPoker, int index)
        {
            int j = 0;

            //���������Ļ
            Rectangle rect = new Rectangle(30, 360, 560, 96);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);

            //ȷ���滭��ʼλ��
            int start = (int)((2780 - index * 75) / 10);

            //����
            j = DrawMyHearts(g, currentPoker, j, start);
            //��ɫ֮��ӿ�϶
            j++;


            //����
            j = DrawMyPeachs(g, currentPoker, j, start);
            //��ɫ֮��ӿ�϶
            j++;


            //����
            j = DrawMyDiamonds(g, currentPoker, j, start);
            //��ɫ֮��ӿ�϶
            j++;


            //÷��
            j = DrawMyClubs(g, currentPoker, j, start);
            //��ɫ֮��ӿ�϶
            j++;

            //Rank(�ݲ���������Rank)
            j = DrawHeartsRank(g, currentPoker, j, start);
            j = DrawPeachsRank(g, currentPoker, j, start);
            j = DrawClubsRank(g, currentPoker, j, start);
            j = DrawDiamondsRank(g, currentPoker, j, start);

            //С��
            j = DrawSmallJack(g, currentPoker, j, start);
            //����
            j = DrawBigJack(g, currentPoker, j, start);


        }

        //���Լ�����õ���,һ���������ƺ����,�ͳ�һ���ƺ����
        /// <summary>
        /// �ڳ���ײ������Ѿ�����õ���.
        /// ��������»�ʹ�����������
        /// 1.�����׼������ʱ
        /// 2.����һ����,��Ҫ�ػ��ײ�
        /// </summary>
        /// <param name="currentPoker"></param>
        internal void DrawMySortedCards(CurrentPoker currentPoker, int index)
        {

            //����ʱ�������
            //��������ʱ������¼�����е��Ƶ�λ�á���С���Ƿ񱻵��
            mainForm.myCardsLocation = new ArrayList();
            mainForm.myCardsNumber = new ArrayList();
            mainForm.myCardIsReady = new ArrayList();


            Graphics g = Graphics.FromImage(mainForm.bmp);

            //���������Ļ
            Rectangle rect = new Rectangle(30, 355, 600, 116);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);

            //�����ʼλ��
            int start = (int)((2780 - index * 75) / 10);


            //��¼ÿ���Ƶ�Xֵ
            int j = 0;
            //��ʱ���������������ж��Ƿ�ĳ��ɫȱʧ
            int k = 0;
            if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Heart)//����
            {
                j = DrawMyPeachs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyHearts(g, currentPoker, j, start);

                j = DrawPeachsRank(g, currentPoker, j, start);
                j = DrawDiamondsRank(g, currentPoker, j, start);
                j = DrawClubsRank(g, currentPoker, j, start);
                j = DrawHeartsRank(g, currentPoker, j, start);
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Spade) //����
            {

                j = DrawMyDiamonds(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyHearts(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs(g, currentPoker, j, start);


                j = DrawDiamondsRank(g, currentPoker, j, start);
                j = DrawClubsRank(g, currentPoker, j, start);
                j = DrawHeartsRank(g, currentPoker, j, start);
                j = DrawPeachsRank(g, currentPoker, j, start);
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Diamond)  //��Ƭ
            {

                j = DrawMyClubs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyHearts(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds(g, currentPoker, j, start);


                j = DrawClubsRank(g, currentPoker, j, start);
                j = DrawHeartsRank(g, currentPoker, j, start);
                j = DrawPeachsRank(g, currentPoker, j, start);
                j = DrawDiamondsRank(g, currentPoker, j, start);//����
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Club)
            {

                j = DrawMyHearts(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs(g, currentPoker, j, start);


                j = DrawHeartsRank(g, currentPoker, j, start);
                j = DrawPeachsRank(g, currentPoker, j, start);
                j = DrawDiamondsRank(g, currentPoker, j, start);
                j = DrawClubsRank(g, currentPoker, j, start);//÷��
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Joker || mainForm.ThisPlayer.CurrentHandState.Trump == Suit.None)
            {
                j = DrawMyHearts(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);

                j = DrawHeartsRank(g, currentPoker, j, start);
                j = DrawPeachsRank(g, currentPoker, j, start);
                j = DrawDiamondsRank(g, currentPoker, j, start);
                j = DrawClubsRank(g, currentPoker, j, start);
            }


            //С��
            j = DrawSmallJack(g, currentPoker, j, start);

            //����
            j = DrawBigJack(g, currentPoker, j, start);

            mainForm.Refresh();
            g.Dispose();
        }

        private static void IsSuitLost(ref int j, ref int k)
        {
            if ((j - k) <= 1)
            {
                j--;
            }
            k = j;
        }

        /// <summary>
        /// �ػ������е���.
        /// ���������˵��������һ�֮����л��ơ�
        /// </summary>
        /// <param name="currentPoker">��ǰ�����е���</param>
        /// <param name="index">�Ƶ�����</param>
        internal void DrawMyPlayingCards(CurrentPoker currentPoker)
        {
            int index = currentPoker.Count;


            mainForm.cardsOrderNumber = 0;

            Graphics g = Graphics.FromImage(mainForm.bmp);

            //���������Ļ
            Rectangle rect = new Rectangle(30, 355, 600, 116);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
            DrawScoreImage();

            int start = (int)((2780 - index * 75) / 10);

            //Rank(��������Rank)
            //��¼ÿ���Ƶ�Xֵ
            int j = 0;
            //��ʱ���������������ж��Ƿ�ĳ��ɫȱʧ
            int k = 0;

            if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Heart)
            {
                j = DrawMyPeachs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyHearts2(g, currentPoker, j, start);

                j = DrawPeachsRank2(g, currentPoker, j, start);
                j = DrawDiamondsRank2(g, currentPoker, j, start);
                j = DrawClubsRank2(g, currentPoker, j, start);
                j = DrawHeartsRank2(g, currentPoker, j, start);//����
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Spade)
            {

                j = DrawMyDiamonds2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyHearts2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs2(g, currentPoker, j, start);

                j = DrawDiamondsRank2(g, currentPoker, j, start);
                j = DrawClubsRank2(g, currentPoker, j, start);
                j = DrawHeartsRank2(g, currentPoker, j, start);
                j = DrawPeachsRank2(g, currentPoker, j, start);//����
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Diamond)
            {

                j = DrawMyClubs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyHearts2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds2(g, currentPoker, j, start);

                j = DrawClubsRank2(g, currentPoker, j, start);
                j = DrawHeartsRank2(g, currentPoker, j, start);
                j = DrawPeachsRank2(g, currentPoker, j, start);
                j = DrawDiamondsRank2(g, currentPoker, j, start);//����
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Club)
            {

                j = DrawMyHearts2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs2(g, currentPoker, j, start);

                j = DrawHeartsRank2(g, currentPoker, j, start);
                j = DrawPeachsRank2(g, currentPoker, j, start);
                j = DrawDiamondsRank2(g, currentPoker, j, start);
                j = DrawClubsRank2(g, currentPoker, j, start);//÷��
            }
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Joker)
            {
                j = DrawMyHearts2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyPeachs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyDiamonds2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);
                j = DrawMyClubs2(g, currentPoker, j, start) + 1;
                IsSuitLost(ref j, ref k);

                j = DrawHeartsRank2(g, currentPoker, j, start);
                j = DrawPeachsRank2(g, currentPoker, j, start);
                j = DrawDiamondsRank2(g, currentPoker, j, start);
                j = DrawClubsRank2(g, currentPoker, j, start);
            }

            //С��
            j = DrawBlackJoker2(g, currentPoker, j, start);

            //����
            j = DrawRedJoker2(g, currentPoker, j, start);


            //�жϵ�ǰ�ĳ������Ƿ���Ч,�����Ч����С��
            Rectangle pigRect = new Rectangle(296, 300, 53, 46);
            if (mainForm.SelectedCards.Count > 0)
            {
                var selectedCardsValidationResult = TractorRules.IsValid(mainForm.ThisPlayer.CurrentTrickState,
                                                                         mainForm.SelectedCards,
                                                                         mainForm.ThisPlayer.CurrentPoker);

                if ((mainForm.ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.Playing
                     && mainForm.ThisPlayer.CurrentTrickState.NextPlayer() == mainForm.ThisPlayer.PlayerId)
                    &&
                    (selectedCardsValidationResult.ResultType == ShowingCardsValidationResultType.Valid ||
                     selectedCardsValidationResult.ResultType == ShowingCardsValidationResultType.TryToDump))
                {
                    g.DrawImage(Properties.Resources.Ready, pigRect);
                }
                else if ((mainForm.ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.Playing
                 && mainForm.ThisPlayer.CurrentTrickState.NextPlayer() == mainForm.ThisPlayer.PlayerId))
                {
                    g.DrawImage(mainForm.image, pigRect, pigRect, GraphicsUnit.Pixel);
                }    

            }
            else if ((mainForm.ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.Playing
             && mainForm.ThisPlayer.CurrentTrickState.NextPlayer() == mainForm.ThisPlayer.PlayerId))
            {
                g.DrawImage(mainForm.image, pigRect, pigRect, GraphicsUnit.Pixel);
            }    


            My8CardsIsReady(g);

            mainForm.Refresh();
            g.Dispose();
        }

        private void My8CardsIsReady(Graphics g)
        {
            //������ҿ���
            if (mainForm.ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.DiscardingLast8Cards && mainForm.ThisPlayer.CurrentHandState.Last8Holder == mainForm.ThisPlayer.PlayerId)
            {
                int total = 0;
                for (int i = 0; i < mainForm.myCardIsReady.Count; i++)
                {
                    if ((bool)mainForm.myCardIsReady[i])
                    {
                        total++;
                    }
                }
                Rectangle pigRect = new Rectangle(296, 300, 53, 46);
                if (total == 8)
                {
                    g.DrawImage(Properties.Resources.Ready, pigRect);
                }
                else
                {
                    g.DrawImage(mainForm.image, pigRect, pigRect, GraphicsUnit.Pixel);

                }
            }
        }


        /// <summary>
        /// ����Ļ��������ҳ�����
        /// </summary>
        /// <param name="readys">�ҳ����Ƶ��б�</param>
        internal void DrawMySendedCardsAction(ArrayList readys)
        {
            int start = 285 - readys.Count * 7;
            Graphics g = Graphics.FromImage(mainForm.bmp);
            for (int i = 0; i < readys.Count; i++)
            {
                DrawMyImage(g, getPokerImageByNumber((int)readys[i]), start, 244, 71, 96);
                start += 14;
            }
            g.Dispose();


        }

        /// <summary>
        /// ����Ļ��������ҳ�����
        /// </summary>
        /// <param name="cards">the card numbers of showed cards</param>
        internal void DrawMySendedCardsAction(List<int> cards)
        {
            int start = 285 - cards.Count * 7;
            Graphics g = Graphics.FromImage(mainForm.bmp);
            foreach (var card in cards)
            {
                DrawMyImage(g, getPokerImageByNumber(card), start, 244, 71, 96);
                start += 14;
            }
            mainForm.Refresh();
            g.Dispose();
        }

        /// <summary>
        /// ���Լҵ���
        /// </summary>
        /// <param name="readys"></param>
        private void DrawFriendUserSendedCardsAction(ArrayList readys)
        {
            int start = 285 - readys.Count * 7;
            Graphics g = Graphics.FromImage(mainForm.bmp);
            for (int i = 0; i < readys.Count; i++)
            {
                DrawMyImage(g, getPokerImageByNumber((int)readys[i]), start, 130, 71, 96);
                start += 14;
            }
            // RedrawFrieldUserCardsAction(g, mainForm.currentPokers[1]);


            g.Dispose();
        }



        /// <summary>
        /// ���ϼ�Ӧ�ó�����
        /// </summary>
        /// <param name="readys"></param>
        private void DrawPreviousUserSendedCardsAction(ArrayList readys)
        {
            int start = 245 - readys.Count * 13;
            Graphics g = Graphics.FromImage(mainForm.bmp);
            for (int i = 0; i < readys.Count; i++)
            {
                DrawMyImage(g, getPokerImageByNumber((int)readys[i]), start + i * 13, 192, 71, 96);
            }

            // RedrawPreviousUserCardsAction(g, mainForm.currentPokers[2]);

            g.Dispose();
        }



        /// <summary>
        /// ���¼�Ӧ�ó�����
        /// </summary>
        /// <param name="readys"></param>
        private void DrawNextUserSendedCardsAction(ArrayList readys)
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            for (int i = 0; i < readys.Count; i++)
            {
                DrawMyImage(g, getPokerImageByNumber((int)readys[i]), 326 + i * 13, 192, 71, 96);
            }

            //RedrawNextUserCardsAction(g, mainForm.currentPokers[3]);


            g.Dispose();
        }



        #endregion // �ڸ�������»��Լ�����


        #region ���Լ�������(���ֻ�ɫ�����ֻ�ɫRank����С��)
        private int DrawBigJack(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            j = DrawMyOneOrTwoCards(g, currentPoker.RedJoker, 53, j, start);
            return j;
        }


        private int DrawSmallJack(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            j = DrawMyOneOrTwoCards(g, currentPoker.BlackJoker, 52, j, start);
            return j;
        }

        private int DrawDiamondsRank(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            j = DrawMyOneOrTwoCards(g, currentPoker.DiamondsRankTotal, mainForm.ThisPlayer.CurrentHandState.Rank + 26, j, start);
            return j;
        }

        private int DrawClubsRank(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            j = DrawMyOneOrTwoCards(g, currentPoker.ClubsRankTotal, mainForm.ThisPlayer.CurrentHandState.Rank + 39, j, start);
            return j;
        }

        private int DrawPeachsRank(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            j = DrawMyOneOrTwoCards(g, currentPoker.SpadesRankCount, mainForm.ThisPlayer.CurrentHandState.Rank + 13, j, start);
            return j;
        }

        private int DrawHeartsRank(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            j = DrawMyOneOrTwoCards(g, currentPoker.HeartsRankTotal, mainForm.ThisPlayer.CurrentHandState.Rank, j, start);
            return j;
        }

        private int DrawMyClubs(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                j = DrawMyOneOrTwoCards(g, currentPoker.ClubsNoRank[i], i + 39, j, start);
            }
            return j;
        }

        private int DrawMyDiamonds(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                j = DrawMyOneOrTwoCards(g, currentPoker.DiamondsNoRank[i], i + 26, j, start);
            }
            return j;
        }

        private int DrawMyPeachs(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                j = DrawMyOneOrTwoCards(g, currentPoker.PeachsNoRank[i], i + 13, j, start);

            }
            return j;
        }

        private int DrawMyHearts(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                j = DrawMyOneOrTwoCards(g, currentPoker.HeartsNoRank[i], i, j, start);
            }
            return j;
        }

        //��������
        private int DrawMyOneOrTwoCards(Graphics g, int count, int number, int j, int start)
        {
            //�������������������Ҫ��������������һ��
            bool b = (number == 52) || (number == 53);
            b = b & (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Joker);
            if (mainForm.ThisPlayer.CurrentHandState.Trump != Suit.Joker)
            {
                if (number ==((int) mainForm.ThisPlayer.CurrentHandState.Trump - 1)*13 +mainForm.ThisPlayer.CurrentHandState.Rank)
                {
                    b = true;
                }

            }
            b = b && (mainForm.ThisPlayer.CurrentHandState.CurrentHandStep == HandStep.DistributingCards);

            if (count == 1)
            {
                SetCardsInformation(start + j * 13, number, false);
                if (mainForm.ThisPlayer.PlayerId == mainForm.ThisPlayer.CurrentHandState.TrumpMaker && b)
                {
                    if (number == 52 || number == 53)
                    {
                        g.DrawImage(getPokerImageByNumber(number), start + j * 13, 375, 71, 96); //����������������
                    }
                    else
                    {
                        g.DrawImage(getPokerImageByNumber(number), start + j * 13, 360, 71, 96);
                    }
                }
                else
                {
                    g.DrawImage(getPokerImageByNumber(number), start + j * 13, 375, 71, 96);
                }

                j++;
            }
            else if (count == 2)
            {
                SetCardsInformation(start + j*13, number, false);

                if (mainForm.ThisPlayer.PlayerId == mainForm.ThisPlayer.CurrentHandState.TrumpMaker && b &&
                    mainForm.ThisPlayer.CurrentHandState.TrumpExposingPoker >= TrumpExposingPoker.SingleRank)
                {
                    g.DrawImage(getPokerImageByNumber(number), start + j*13, 360, 71, 96);
                }
                else
                {
                    g.DrawImage(getPokerImageByNumber(number), start + j*13, 375, 71, 96);
                }

                j++;
                SetCardsInformation(start + j*13, number, false);
                if (mainForm.ThisPlayer.PlayerId == mainForm.ThisPlayer.CurrentHandState.TrumpMaker && b &&
                    mainForm.ThisPlayer.CurrentHandState.TrumpExposingPoker >= TrumpExposingPoker.PairRank)
                {
                    g.DrawImage(getPokerImageByNumber(number), start + j*13, 360, 71, 96);
                }
                else
                {
                    g.DrawImage(getPokerImageByNumber(number), start + j*13, 375, 71, 96);
                }

                j++;
            }
            return j;
        }


        #endregion // ���Լ�������

        #region ���Լ�����ķ���
        private int DrawRedJoker2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            if (currentPoker.RedJoker == 1)
            {
                j = DrawMyOneOrTwoCards2(g, j, 53, start + j * 13, 355, 71, 96) + 1;
            }
            else if (currentPoker.RedJoker == 2)
            {
                j = DrawMyOneOrTwoCards2(g, j, 53, start + j * 13, 355, 71, 96) + 1;
                j = DrawMyOneOrTwoCards2(g, j, 53, start + j * 13, 355, 71, 96) + 1;
            }
            return j;
        }

        private int DrawBlackJoker2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            if (currentPoker.BlackJoker == 1)
            {
                j = DrawMyOneOrTwoCards2(g, j, 52, start + j * 13, 355, 71, 96) + 1;
            }
            else if (currentPoker.BlackJoker == 2)
            {
                j = DrawMyOneOrTwoCards2(g, j, 52, start + j * 13, 355, 71, 96) + 1;
                j = DrawMyOneOrTwoCards2(g, j, 52, start + j * 13, 355, 71, 96) + 1;
            }
            return j;
        }

        private int DrawDiamondsRank2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            if (currentPoker.DiamondsRankTotal == 1)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 26, start + j * 13, 355, 71, 96) + 1;
            }
            else if (currentPoker.DiamondsRankTotal == 2)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 26, start + j * 13, 355, 71, 96) + 1;
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 26, start + j * 13, 355, 71, 96) + 1;
            }
            return j;
        }

        private int DrawClubsRank2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            if (currentPoker.ClubsRankTotal == 1)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 39, start + j * 13, 355, 71, 96) + 1;
            }
            else if (currentPoker.ClubsRankTotal == 2)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 39, start + j * 13, 355, 71, 96) + 1;
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 39, start + j * 13, 355, 71, 96) + 1;
            }
            return j;
        }

        private int DrawPeachsRank2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            if (currentPoker.SpadesRankCount == 1)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 13, start + j * 13, 355, 71, 96) + 1;
            }
            else if (currentPoker.SpadesRankCount == 2)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 13, start + j * 13, 355, 71, 96) + 1;
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank + 13, start + j * 13, 355, 71, 96) + 1;
            }
            return j;
        }

        private int DrawHeartsRank2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            if (currentPoker.HeartsRankTotal == 1)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank, start + j * 13, 355, 71, 96) + 1;
            }
            else if (currentPoker.HeartsRankTotal == 2)
            {
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank, start + j * 13, 355, 71, 96) + 1;
                j = DrawMyOneOrTwoCards2(g, j, mainForm.ThisPlayer.CurrentHandState.Rank, start + j * 13, 355, 71, 96) + 1;
            }
            return j;
        }

        private int DrawMyClubs2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                if (currentPoker.ClubsNoRank[i] == 1)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i + 39, start + j * 13, 355, 71, 96) + 1;
                }
                else if (currentPoker.ClubsNoRank[i] == 2)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i + 39, start + j * 13, 355, 71, 96) + 1;
                    j = DrawMyOneOrTwoCards2(g, j, i + 39, start + j * 13, 355, 71, 96) + 1;
                }
            }
            return j;
        }

        private int DrawMyDiamonds2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                if (currentPoker.DiamondsNoRank[i] == 1)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i + 26, start + j * 13, 355, 71, 96) + 1;
                }
                else if (currentPoker.DiamondsNoRank[i] == 2)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i + 26, start + j * 13, 355, 71, 96) + 1;
                    j = DrawMyOneOrTwoCards2(g, j, i + 26, start + j * 13, 355, 71, 96) + 1;
                }
            }
            return j;
        }

        private int DrawMyPeachs2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                if (currentPoker.PeachsNoRank[i] == 1)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i + 13, start + j * 13, 355, 71, 96) + 1;
                }
                else if (currentPoker.PeachsNoRank[i] == 2)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i + 13, start + j * 13, 355, 71, 96) + 1;
                    j = DrawMyOneOrTwoCards2(g, j, i + 13, start + j * 13, 355, 71, 96) + 1;
                }
            }
            return j;
        }

        private int DrawMyHearts2(Graphics g, CurrentPoker currentPoker, int j, int start)
        {
            for (int i = 0; i < 13; i++)
            {
                if (currentPoker.HeartsNoRank[i] == 1)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i, start + j * 13, 355, 71, 96) + 1;
                }
                else if (currentPoker.HeartsNoRank[i] == 2)
                {
                    j = DrawMyOneOrTwoCards2(g, j, i, start + j * 13, 355, 71, 96) + 1;
                    j = DrawMyOneOrTwoCards2(g, j, i, start + j * 13, 355, 71, 96) + 1;
                }
            }
            return j;
        }

        //��������
        private int DrawMyOneOrTwoCards2(Graphics g, int j, int number, int x, int y, int width, int height)
        {
            if ((bool)mainForm.myCardIsReady[mainForm.cardsOrderNumber])
            {
                g.DrawImage(getPokerImageByNumber(number), x, y, width, height);
            }
            else
            {
                g.DrawImage(getPokerImageByNumber(number), x, y + 20, width, height);
            }

            mainForm.cardsOrderNumber++;
            return j;
        }
        #endregion // ���ƵĻ��Լ�����ķ���

        #region ���Ƹ��ҳ����ƣ�������������֪ͨ��һ��
        /// <summary>
        /// ���Լ�������
        /// </summary>
        internal void DrawMyFinishSendedCards()
        {
            //�����뻭���������
            DrawMySendedCardsAction(mainForm.currentSendCards[0]);



            //�ػ��Լ����е���
            if (mainForm.currentPokers[0].Count > 0)
            {
                DrawMySortedCards(mainForm.currentPokers[0], mainForm.currentPokers[0].Count);
            }
            else //�����²��ռ�
            {
                Rectangle rect = new Rectangle(30, 355, 560, 116);
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
                g.Dispose();
            }

            mainForm.Refresh();

        }

        /// <summary>
        /// �¼ҳ���
        /// </summary>
        internal void DrawNextUserSendedCards()
        {
            var latestCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.CurrentTrickState.LatestPlayerShowedCard()];
            DrawNextUserSendedCardsAction(new ArrayList(latestCards));


            //�����Ƿ��ס������
            //���Ѿ����ƣ�Ӧ�ý����ػ�
            var myShowedCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.PlayerId];
            if (myShowedCards.Count > 0)
            {
                DrawMySendedCardsAction(myShowedCards);
            }


            // DrawScoreImage(mainForm.Scores);
            mainForm.Refresh();

        }

        /// <summary>
        /// �Լҳ���
        /// </summary>
        internal void DrawFriendUserSendedCards()
        {

            var latestCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.CurrentTrickState.LatestPlayerShowedCard()];
            DrawFriendUserSendedCardsAction(new ArrayList(latestCards));



            //�����Ƿ��ס������
            //����¼��Ѿ����ƣ�Ӧ�ý��¼��ػ�,
            var nextPlayerShowedCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.PositionPlayer[2]];
            if (nextPlayerShowedCards.Count > 0)
            {
                DrawNextUserSendedCardsAction(new ArrayList(nextPlayerShowedCards));
            }
            //�ػ��¼�֮���ػ��ҵ���
            var myShowedCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.PlayerId];
            if (myShowedCards.Count > 0)
            {
                DrawMySendedCardsAction(myShowedCards);
            }

            //DrawScoreImage(mainForm.Scores);
            mainForm.Refresh();

        }

        /// <summary>
        /// �ϼҳ���
        /// </summary>
        internal void DrawPreviousUserSendedCards()
        {
            var latestCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.CurrentTrickState.LatestPlayerShowedCard()];
            DrawPreviousUserSendedCardsAction(new ArrayList(latestCards));


            //�ػ��¼�֮���ػ��ҵ���
            var myShowedCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.PlayerId];
            if (myShowedCards.Count > 0)
            {
                DrawMySendedCardsAction(myShowedCards);
            }

            //DrawScoreImage(mainForm.Scores);
            mainForm.Refresh();

        }

        public void DrawWhoWinThisTime()
        {
            //˭Ӯ����һȦ
            string winner = mainForm.ThisPlayer.CurrentTrickState.Winner;
            int winnerPosition = mainForm.PlayerPosition[winner];

            if (winnerPosition == 1) //��
            {
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(Properties.Resources.Winner, 437, 310, 33, 53);
                g.Dispose();
            }
            else if (winnerPosition == 3) //�Լ�
            {
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(Properties.Resources.Winner, 437, 120, 33, 53);
                g.Dispose();
            }
            else if (winnerPosition == 4) //����
            {
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(Properties.Resources.Winner, 90, 218, 33, 53);
                g.Dispose();
            }
            else if (winnerPosition == 2) //����
            {
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(Properties.Resources.Winner, 516, 218, 33, 53);
                g.Dispose();
            }

            mainForm.Refresh();
        }

        internal void DrawScoreImage()
        {
            int scores = mainForm.ThisPlayer.CurrentHandState.Score;
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Bitmap bmp = global::Duan.Xiugang.Tractor.Properties.Resources.scores;
            Font font = new Font("����", 12, FontStyle.Bold);

            if (mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.Starter] == 2 || mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.Starter] == 3)
            {
                Rectangle rect = new Rectangle(490, 128, 56, 56);
                g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
                g.DrawImage(bmp, rect);
                int x = 506;
                if (scores.ToString().Length == 2)
                {
                    x -= 4;
                }
                else if (scores.ToString().Length == 3)
                {
                    x -= 8;
                }
                g.DrawString(scores + "", font, Brushes.White, x, 138);
            }
            else
            {
                Rectangle rect = new Rectangle(85, 300, 56, 56);
                g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
                g.DrawImage(bmp, rect);
                int x = 100;
                if (scores.ToString().Length == 2)
                {
                    x -= 4;
                }
                else if (scores.ToString().Length == 3)
                {
                    x -= 8;
                }
                g.DrawString(scores + "", font, Brushes.White, x, 310);
            }

            g.Dispose();
        }

        internal void DrawFinishedScoreImage()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);

            Pen pen = new Pen(Color.White, 2);
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.White)), 77, 124, 476, 244);
            g.DrawRectangle(pen, 77, 124, 476, 244);

            //������,��169��ʼ��
            for (int i = 0; i < 8; i++)
            {
                g.DrawImage(getPokerImageByNumber((int)mainForm.ThisPlayer.CurrentHandState.DiscardedCards[i]), 230 + i * 14, 130, 71, 96);
            }

            //��СѾ
            g.DrawImage(global::Duan.Xiugang.Tractor.Properties.Resources.Logo, 160, 237, 110, 112);

            //���÷�
            Font font = new Font("����", 16, FontStyle.Bold);
            g.DrawString("�ܵ÷� " + mainForm.ThisPlayer.CurrentHandState.Score, font, Brushes.Blue, 310, 286);

            g.Dispose();

        }

        //��Ҷ������ƣ������÷ֶ��٣��´θ�˭����
        internal void DrawFinishedSendedCards()
        {
            DrawCenterImage();
            DrawFinishedScoreImage();
            mainForm.Refresh();
        }
        #endregion // ���Ƹ��ҳ����ƣ�������������֪ͨ��һ��


        #region ����ʱ�ĸ�������

        //�����ƺŵõ���Ӧ���Ƶ�ͼƬ
        private Bitmap getPokerImageByNumber(int number)
        {
            Bitmap bitmap = null;

            if (mainForm.gameConfig.CardImageName.Length == 0) //����Ƕ��ͼ���ж�ȡ
            {
                bitmap = (Bitmap)mainForm.gameConfig.CardsResourceManager.GetObject("_" + number, Kuaff_Cards.Culture);
            }
            else
            {
                bitmap = mainForm.cardsImages[number]; //���Զ����ͼƬ�ж�ȡ
            }

            return bitmap;
        }

        /// <summary>
        /// �ػ����򱳾�
        /// </summary>
        /// <param name="g">������ͼ���Graphics</param>
        internal void DrawBackground(Graphics g)
        {
            //Bitmap image = global::Kuaff.Tractor.Properties.Resources.Backgroud;
            g.DrawImage(mainForm.image, mainForm.ClientRectangle, mainForm.ClientRectangle, GraphicsUnit.Pixel);
        }

        //�����ƶ��������м�֡�������ú���ȥ��
        private void DrawAnimatedCard(Bitmap card, int x, int y, int width, int height)
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Bitmap backup = mainForm.bmp.Clone(new Rectangle(x, y, width, height), PixelFormat.DontCare);
            g.DrawImage(card, x, y, width, height);
            mainForm.Refresh();
            g.DrawImage(backup, x, y, width, height);
            g.Dispose();
        }

        //��ͼ�ķ���
        private void DrawMyImage(Graphics g, Bitmap bmp, int x, int y, int width, int height)
        {
            g.DrawImage(bmp, x, y, width, height);
        }

        //���õ�ǰ���Ƶ���Ϣ
        private void SetCardsInformation(int x, int number, bool ready)
        {
            mainForm.myCardsLocation.Add(x);
            mainForm.myCardsNumber.Add(number);
            mainForm.myCardIsReady.Add(ready);
        }
        #endregion // ����ʱ�ĸ�������

        public void DrawMyShowedCards()
        {
            //�����뻭���������
            DrawMySendedCardsAction(mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.PlayerId]);
            //�ػ��Լ����е���
            if (mainForm.ThisPlayer.CurrentPoker.Count > 0)
                DrawMySortedCards(mainForm.ThisPlayer.CurrentPoker, mainForm.ThisPlayer.CurrentPoker.Count);
            else //���е��ƶ�������
            {
                Rectangle rect = new Rectangle(30, 355, 560, 116);
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
                g.Dispose();
            }

            mainForm.Refresh();

        }


        internal void Starter()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);

            int starterPostion = 0;
            if (!string.IsNullOrEmpty(mainForm.ThisPlayer.CurrentHandState.Starter))
            {
                starterPostion = mainForm.PlayerPosition[mainForm.ThisPlayer.CurrentHandState.Starter];
            }
                                   
            Rectangle destRect;
            Rectangle srcRect;

            //south
            if (starterPostion == 1)
            {

                destRect = new Rectangle(548, 45, 20, 20);
                srcRect = new Rectangle(120, 0, 20, 20);

                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                destRect = new Rectangle(548, 45, 20, 20);
                srcRect = new Rectangle(40, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }

            //north
            if (starterPostion == 3)
            {
                destRect = new Rectangle(580, 45, 20, 20);
                srcRect = new Rectangle(140, 0, 20, 20);

                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                destRect = new Rectangle(580, 45, 20, 20);
                srcRect = new Rectangle(60, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            //west
            if (starterPostion == 4)
            {
                destRect = new Rectangle(30, 45, 20, 20);
                srcRect = new Rectangle(80, 0, 20, 20);

                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                destRect = new Rectangle(31, 45, 20, 20);
                srcRect = new Rectangle(0, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            //east
            if (starterPostion == 2)
            {
                destRect = new Rectangle(60, 45, 20, 20);
                srcRect = new Rectangle(100, 0, 20, 20);

                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                destRect = new Rectangle(61, 45, 20, 20);
                srcRect = new Rectangle(20, 0, 20, 20);
                g.DrawImage(Properties.Resources.Master, destRect, srcRect, GraphicsUnit.Pixel);
            }

            mainForm.Refresh();
            g.Dispose();
        }
    }
}
