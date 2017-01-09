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
    /// 实现大部分的绘画操作
    /// </summary>
    class DrawingFormHelper
    {
        MainForm mainForm;
        internal DrawingFormHelper(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }


        #region 发牌动画

        internal void IGetCard(int cardNumber)
        {
            //得到缓冲区图像的Graphics
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
        //清除亮的牌
        internal void ClearSuitCards(Graphics g)
        {
            g.DrawImage(mainForm.image, new Rectangle(80, 158, 71, 116), new Rectangle(80, 158, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(480, 200, 71, 116), new Rectangle(480, 200, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(423, 124, 85, 96), new Rectangle(423, 124, 85, 96), GraphicsUnit.Pixel);
        }

        //清除亮的牌
        internal void ClearSuitCards()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(mainForm.image, new Rectangle(80, 158, 71, 116), new Rectangle(80, 158, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(480, 200, 71, 116), new Rectangle(480, 200, 71, 116), GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, new Rectangle(423, 124, 85, 96), new Rectangle(423, 85, 71, 96), GraphicsUnit.Pixel);
            mainForm.Refresh();
            g.Dispose();
        }

        #endregion // 发牌动画

        #region 画中心位置的牌
        /// <summary>
        /// 发牌时画中央的牌.
        /// 首先从底图中取相应的位置，重画这块背景。
        /// 然后用牌的背面画58-count*2张牌。
        /// 
        /// </summary>
        /// <param name="g">缓冲区图片的Graphics</param>
        /// <param name="num">牌的数量=58-发牌次数*2</param>
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
        /// 发完一次牌，需要清理程序中心
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
        /// 画流局图片
        /// </summary>
        internal void DrawPassImage()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Rectangle rect = new Rectangle(110, 150, 400, 199);
            g.DrawImage(Properties.Resources.Pass, rect);
            g.Dispose();
            mainForm.Refresh();
        }
        #endregion // 画中心位置的牌

        #region 底牌处理

        internal void DrawDiscardedLast8Cards()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Rectangle rect = new Rectangle(200, 186, 90, 96);
            Rectangle backRect = new Rectangle(77, 121, 477, 254);
            //最后8张的图像取出来
            Bitmap backup = mainForm.bmp.Clone(rect, PixelFormat.DontCare);
            //将其位置用背景贴上
            //g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, backRect, backRect, GraphicsUnit.Pixel);
            mainForm.Refresh();
            g.Dispose();
        }
        //收底牌的动画
        /// <summary>
        /// 发牌25次后，最后剩余8张牌.
        /// 这时已经确定了庄家，将8张牌交给庄家,
        /// 同时以动画的方式显示。
        /// </summary>
        internal void DrawCenter8Cards()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Rectangle rect = new Rectangle(200, 186, 90, 96);
            Rectangle backRect = new Rectangle(77, 121, 477, 254);
            //最后8张的图像取出来
            Bitmap backup = mainForm.bmp.Clone(rect, PixelFormat.DontCare);
            //将其位置用背景贴上
            //g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
            g.DrawImage(mainForm.image, backRect, backRect, GraphicsUnit.Pixel);

            //将底牌8张交给庄家（动画方式）
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
        //将最后8张交给庄家
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

            //画底牌,从169开始画
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
        #endregion // 底牌处理


        #region 绘制Sidebar和toolbar
        /// <summary>
        /// 绘制Sidebar
        /// </summary>
        /// <param name="g"></param>
        internal void DrawSidebar(Graphics g)
        {
            DrawMyImage(g, Properties.Resources.Sidebar, 20, 30, 70, 89);
            DrawMyImage(g, Properties.Resources.Sidebar, 540, 30, 70, 89);
        }
        /// <summary>
        /// 画东西南北
        /// </summary>
        /// <param name="g">缓冲区图像的Graphics</param>
        /// <param name="who">画谁</param>
        /// <param name="b">是否画亮色</param>
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
        /// 画其他白色
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
        /// 绘制Rank
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



            //然后将数字填写上
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
        /// 画主
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
        /// 画工具栏
        /// </summary>
        internal void DrawToolbar()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(Properties.Resources.Toolbar, new Rectangle(415, 325, 129, 29), new Rectangle(0, 0, 129, 29), GraphicsUnit.Pixel);
            //画五种暗花色
            g.DrawImage(Properties.Resources.Suit, new Rectangle(417, 327, 125, 25), new Rectangle(125, 0, 125, 25), GraphicsUnit.Pixel);
            g.Dispose();
        }

        /// <summary>
        /// 擦去工具栏
        /// </summary>
        internal void RemoveToolbar()
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(mainForm.image, new Rectangle(415, 325, 129, 29), new Rectangle(415, 325, 129, 29), GraphicsUnit.Pixel);
            g.Dispose();
        }


        #endregion // 绘制Sidebar和toolbar



        //判断我是否亮主
        internal void ReDrawToolbar()
        {
            //如果打无主，无需再判断
            if (mainForm.ThisPlayer.CurrentHandState.Rank == 53)
                return;
            var availableTrump = mainForm.ThisPlayer.AvailableTrumps();
            ReDrawToolbar(availableTrump);


        }

        //画我的工具栏
        internal void ReDrawToolbar(List<Suit> suits)
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            g.DrawImage(Properties.Resources.Toolbar, new Rectangle(415, 325, 129, 29), new Rectangle(0, 0, 129, 29), GraphicsUnit.Pixel);
            //画五种暗花色
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
        /// 判断最后是否有人亮主.
        /// 根据算法，如果无人亮主，则本局流局，重新发牌
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


  


        #region 在各种情况下画自己的牌

        /// <summary>
        /// 发牌期间进行绘制我的区域.
        /// 按照花色和主牌进行区分。
        /// </summary>
        /// <param name="g">缓冲区图片的Graphics</param>
        /// <param name="currentPoker">我当前得到的牌</param>
        /// <param name="index">手中牌的数量</param>
        internal void DrawMyCards(Graphics g, CurrentPoker currentPoker, int index)
        {
            int j = 0;

            //清下面的屏幕
            Rectangle rect = new Rectangle(30, 360, 560, 96);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);

            //确定绘画起始位置
            int start = (int)((2780 - index * 75) / 10);

            //红桃
            j = DrawMyHearts(g, currentPoker, j, start);
            //花色之间加空隙
            j++;


            //黑桃
            j = DrawMyPeachs(g, currentPoker, j, start);
            //花色之间加空隙
            j++;


            //方块
            j = DrawMyDiamonds(g, currentPoker, j, start);
            //花色之间加空隙
            j++;


            //梅花
            j = DrawMyClubs(g, currentPoker, j, start);
            //花色之间加空隙
            j++;

            //Rank(暂不分主、副Rank)
            j = DrawHeartsRank(g, currentPoker, j, start);
            j = DrawPeachsRank(g, currentPoker, j, start);
            j = DrawClubsRank(g, currentPoker, j, start);
            j = DrawDiamondsRank(g, currentPoker, j, start);

            //小王
            j = DrawSmallJack(g, currentPoker, j, start);
            //大王
            j = DrawBigJack(g, currentPoker, j, start);


        }

        //画自己排序好的牌,一般在摸完牌后调用,和出一次牌后调用
        /// <summary>
        /// 在程序底部绘制已经排序好的牌.
        /// 两种情况下会使用这个方法：
        /// 1.收完底准备出牌时
        /// 2.出完一次牌,需要重画底部
        /// </summary>
        /// <param name="currentPoker"></param>
        internal void DrawMySortedCards(CurrentPoker currentPoker, int index)
        {

            //将临时变量清空
            //这三个临时变量记录我手中的牌的位置、大小和是否被点出
            mainForm.myCardsLocation = new ArrayList();
            mainForm.myCardsNumber = new ArrayList();
            mainForm.myCardIsReady = new ArrayList();


            Graphics g = Graphics.FromImage(mainForm.bmp);

            //清下面的屏幕
            Rectangle rect = new Rectangle(30, 355, 600, 116);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);

            //计算初始位置
            int start = (int)((2780 - index * 75) / 10);


            //记录每张牌的X值
            int j = 0;
            //临时变量，用来辅助判断是否某花色缺失
            int k = 0;
            if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Heart)//红桃
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
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Spade) //黑桃
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
            else if (mainForm.ThisPlayer.CurrentHandState.Trump == Suit.Diamond)  //方片
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
                j = DrawDiamondsRank(g, currentPoker, j, start);//方块
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
                j = DrawClubsRank(g, currentPoker, j, start);//梅花
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


            //小王
            j = DrawSmallJack(g, currentPoker, j, start);

            //大王
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
        /// 重画我手中的牌.
        /// 在鼠标进行了单击或者右击之后进行绘制。
        /// </summary>
        /// <param name="currentPoker">当前我手中的牌</param>
        /// <param name="index">牌的数量</param>
        internal void DrawMyPlayingCards(CurrentPoker currentPoker)
        {
            int index = currentPoker.Count;


            mainForm.cardsOrderNumber = 0;

            Graphics g = Graphics.FromImage(mainForm.bmp);

            //清下面的屏幕
            Rectangle rect = new Rectangle(30, 355, 600, 116);
            g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
            DrawScoreImage();

            int start = (int)((2780 - index * 75) / 10);

            //Rank(分主、副Rank)
            //记录每张牌的X值
            int j = 0;
            //临时变量，用来辅助判断是否某花色缺失
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
                j = DrawHeartsRank2(g, currentPoker, j, start);//红桃
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
                j = DrawPeachsRank2(g, currentPoker, j, start);//黑桃
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
                j = DrawDiamondsRank2(g, currentPoker, j, start);//方块
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
                j = DrawClubsRank2(g, currentPoker, j, start);//梅花
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

            //小王
            j = DrawBlackJoker2(g, currentPoker, j, start);

            //大王
            j = DrawRedJoker2(g, currentPoker, j, start);


            //判断当前的出的牌是否有效,如果有效，画小猪
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
            //如果等我扣牌
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
        /// 在屏幕中央绘制我出的牌
        /// </summary>
        /// <param name="readys">我出的牌的列表</param>
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
        /// 在屏幕中央绘制我出的牌
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
        /// 画对家的牌
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
        /// 画上家应该出的牌
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
        /// 画下家应该出的牌
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



        #endregion // 在各种情况下画自己的牌


        #region 画自己的牌面(四种花色、四种花色Rank、大小王)
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

        //辅助方法
        private int DrawMyOneOrTwoCards(Graphics g, int count, int number, int j, int start)
        {
            //如果是我亮的主，我需要将亮的主往上提一下
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
                        g.DrawImage(getPokerImageByNumber(number), start + j * 13, 375, 71, 96); //单个的王不被提上
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


        #endregion // 画自己的牌面

        #region 画自己牌面的方法
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

        //辅助方法
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
        #endregion // 类似的画自己牌面的方法

        #region 绘制各家出的牌，并计算结果或者通知下一家
        /// <summary>
        /// 画自己出的牌
        /// </summary>
        internal void DrawMyFinishSendedCards()
        {
            //在中央画出点出的牌
            DrawMySendedCardsAction(mainForm.currentSendCards[0]);



            //重画自己手中的牌
            if (mainForm.currentPokers[0].Count > 0)
            {
                DrawMySortedCards(mainForm.currentPokers[0], mainForm.currentPokers[0].Count);
            }
            else //重新下部空间
            {
                Rectangle rect = new Rectangle(30, 355, 560, 116);
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(mainForm.image, rect, rect, GraphicsUnit.Pixel);
                g.Dispose();
            }

            mainForm.Refresh();

        }

        /// <summary>
        /// 下家出牌
        /// </summary>
        internal void DrawNextUserSendedCards()
        {
            var latestCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.CurrentTrickState.LatestPlayerShowedCard()];
            DrawNextUserSendedCardsAction(new ArrayList(latestCards));


            //考虑是否盖住的问题
            //我已经出牌，应该将我重画
            var myShowedCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.PlayerId];
            if (myShowedCards.Count > 0)
            {
                DrawMySendedCardsAction(myShowedCards);
            }


            // DrawScoreImage(mainForm.Scores);
            mainForm.Refresh();

        }

        /// <summary>
        /// 对家出牌
        /// </summary>
        internal void DrawFriendUserSendedCards()
        {

            var latestCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.CurrentTrickState.LatestPlayerShowedCard()];
            DrawFriendUserSendedCardsAction(new ArrayList(latestCards));



            //考虑是否盖住的问题
            //如果下家已经出牌，应该将下家重画,
            var nextPlayerShowedCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.PositionPlayer[2]];
            if (nextPlayerShowedCards.Count > 0)
            {
                DrawNextUserSendedCardsAction(new ArrayList(nextPlayerShowedCards));
            }
            //重画下家之后，重画我的牌
            var myShowedCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.PlayerId];
            if (myShowedCards.Count > 0)
            {
                DrawMySendedCardsAction(myShowedCards);
            }

            //DrawScoreImage(mainForm.Scores);
            mainForm.Refresh();

        }

        /// <summary>
        /// 上家出牌
        /// </summary>
        internal void DrawPreviousUserSendedCards()
        {
            var latestCards = mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.CurrentTrickState.LatestPlayerShowedCard()];
            DrawPreviousUserSendedCardsAction(new ArrayList(latestCards));


            //重画下家之后，重画我的牌
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
            //谁赢了这一圈
            string winner = mainForm.ThisPlayer.CurrentTrickState.Winner;
            int winnerPosition = mainForm.PlayerPosition[winner];

            if (winnerPosition == 1) //我
            {
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(Properties.Resources.Winner, 437, 310, 33, 53);
                g.Dispose();
            }
            else if (winnerPosition == 3) //对家
            {
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(Properties.Resources.Winner, 437, 120, 33, 53);
                g.Dispose();
            }
            else if (winnerPosition == 4) //西家
            {
                Graphics g = Graphics.FromImage(mainForm.bmp);
                g.DrawImage(Properties.Resources.Winner, 90, 218, 33, 53);
                g.Dispose();
            }
            else if (winnerPosition == 2) //东家
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
            Font font = new Font("宋体", 12, FontStyle.Bold);

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

            //画底牌,从169开始画
            for (int i = 0; i < 8; i++)
            {
                g.DrawImage(getPokerImageByNumber((int)mainForm.ThisPlayer.CurrentHandState.DiscardedCards[i]), 230 + i * 14, 130, 71, 96);
            }

            //画小丫
            g.DrawImage(global::Duan.Xiugang.Tractor.Properties.Resources.Logo, 160, 237, 110, 112);

            //画得分
            Font font = new Font("宋体", 16, FontStyle.Bold);
            g.DrawString("总得分 " + mainForm.ThisPlayer.CurrentHandState.Score, font, Brushes.Blue, 310, 286);

            g.Dispose();

        }

        //大家都出完牌，则计算得分多少，下次该谁出牌
        internal void DrawFinishedSendedCards()
        {
            DrawCenterImage();
            DrawFinishedScoreImage();
            mainForm.Refresh();
        }
        #endregion // 绘制各家出的牌，并计算结果或者通知下一家


        #region 画牌时的辅助方法

        //根据牌号得到相应的牌的图片
        private Bitmap getPokerImageByNumber(int number)
        {
            Bitmap bitmap = null;

            if (mainForm.gameConfig.CardImageName.Length == 0) //从内嵌的图案中读取
            {
                bitmap = (Bitmap)mainForm.gameConfig.CardsResourceManager.GetObject("_" + number, Kuaff_Cards.Culture);
            }
            else
            {
                bitmap = mainForm.cardsImages[number]; //从自定义的图片中读取
            }

            return bitmap;
        }

        /// <summary>
        /// 重画程序背景
        /// </summary>
        /// <param name="g">缓冲区图像的Graphics</param>
        internal void DrawBackground(Graphics g)
        {
            //Bitmap image = global::Kuaff.Tractor.Properties.Resources.Backgroud;
            g.DrawImage(mainForm.image, mainForm.ClientRectangle, mainForm.ClientRectangle, GraphicsUnit.Pixel);
        }

        //画发牌动画，将中间帧动画画好后再去除
        private void DrawAnimatedCard(Bitmap card, int x, int y, int width, int height)
        {
            Graphics g = Graphics.FromImage(mainForm.bmp);
            Bitmap backup = mainForm.bmp.Clone(new Rectangle(x, y, width, height), PixelFormat.DontCare);
            g.DrawImage(card, x, y, width, height);
            mainForm.Refresh();
            g.DrawImage(backup, x, y, width, height);
            g.Dispose();
        }

        //画图的方法
        private void DrawMyImage(Graphics g, Bitmap bmp, int x, int y, int width, int height)
        {
            g.DrawImage(bmp, x, y, width, height);
        }

        //设置当前的牌的信息
        private void SetCardsInformation(int x, int number, bool ready)
        {
            mainForm.myCardsLocation.Add(x);
            mainForm.myCardsNumber.Add(number);
            mainForm.myCardIsReady.Add(ready);
        }
        #endregion // 画牌时的辅助方法

        public void DrawMyShowedCards()
        {
            //在中央画出点出的牌
            DrawMySendedCardsAction(mainForm.ThisPlayer.CurrentTrickState.ShowedCards[mainForm.ThisPlayer.PlayerId]);
            //重画自己手中的牌
            if (mainForm.ThisPlayer.CurrentPoker.Count > 0)
                DrawMySortedCards(mainForm.ThisPlayer.CurrentPoker, mainForm.ThisPlayer.CurrentPoker.Count);
            else //所有的牌都出完了
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
