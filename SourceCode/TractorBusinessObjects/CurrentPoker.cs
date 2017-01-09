using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duan.Xiugang.Tractor.Objects
{
    public class CurrentPoker : ICloneable
    {
        //当前的Rank
        public int Rank = 0;
        private int[] _cards = new int[54];
        //当前的trump
        private Suit _trump;

        private int _trumpInt;


        public CurrentPoker()
        {
        }

        public CurrentPoker(IEnumerable<int> cards, Suit trump, int rank)
            : this(cards, (int) trump, rank)
        {
        }

        /// <summary>
        ///     对一个随机产生的序列进行解析
        /// </summary>
        /// <param name="cards">要排序的列表</param>
        /// <param name="suit">当前花色</param>
        /// <param name="rank">当前牌局</param>
        /// <returns>返回CurrentPoker对象</returns>
        public CurrentPoker(IEnumerable<int> cards, int suit, int rank)
        {
            //红桃0-12
            //黑桃13-25
            //方块26-38
            //梅花39-51
            //小王52
            //大王53


            Rank = rank;
            TrumpInt = suit;

            //解析用户的牌局
            foreach (int card in cards)
            {
                AddCard(card);
            }
        }

        public Suit Trump
        {
            get { return _trump; }
            set
            {
                _trump = value;
                _trumpInt = (int) _trump;
            }
        }

        public int TrumpInt
        {
            get { return _trumpInt; }
            set
            {
                _trumpInt = value;
                _trump = (Suit) _trumpInt;
            }
        }

        public int[] Cards
        {
            get { return _cards; }
        }

        public int Count
        {
            get { return _cards.Sum(); }
        }

        #region 方块

        public int[] Diamonds
        {
            get
            {
                var diamonds = new int[13];
                Array.Copy(_cards, 26, diamonds, 0, 13);
                return diamonds;
            }
        }

        public int[] DiamondsNoRank
        {
            get
            {
                int[] diamonds = Diamonds;
                diamonds[Rank] = 0;
                return diamonds;
            }
        }

        public int DiamondsNoRankTotal
        {
            get { return DiamondsNoRank.Sum(); }
        }

        public int DiamondsRankTotal
        {
            get { return Diamonds[Rank]; }
        }

        #endregion

        #region 梅花

        public int[] Clubs
        {
            get
            {
                var clubs = new int[13];
                Array.Copy(_cards, 39, clubs, 0, 13);
                return clubs;
            }
        }

        public int[] ClubsNoRank
        {
            get
            {
                int[] clubs = Clubs;
                clubs[Rank] = 0;
                return clubs;
            }
        }

        public int ClubsNoRankTotal
        {
            get { return ClubsNoRank.Sum(); }
        }

        public int ClubsRankTotal
        {
            get { return Clubs[Rank]; }
        }

        #endregion

        #region Hearts

        public int[] Hearts
        {
            get
            {
                var hearts = new int[13];
                Array.Copy(_cards, 0, hearts, 0, 13);
                return hearts;
            }
        }

        public int[] HeartsNoRank
        {
            get
            {
                int[] hearts = Hearts;
                hearts[Rank] = 0;
                return hearts;
            }
        }

        public int HeartsNoRankTotal
        {
            get { return HeartsNoRank.Sum(); }
        }

        public int HeartsRankTotal
        {
            get { return Hearts[Rank]; }
        }

        #endregion

        #region 黑桃

        public int SpadesNoRankCount
        {
            get { return PeachsNoRank.Sum(); }
        }

        public int SpadesRankCount
        {
            get { return Spades[Rank]; }
        }


        public int[] Spades
        {
            get
            {
                var spades = new int[13];
                Array.Copy(_cards, 13, spades, 0, 13);
                return spades;
            }
        }

        public int[] PeachsNoRank
        {
            get
            {
                int[] spades = Spades;
                spades[Rank] = 0;
                return spades;
            }
        }

        #endregion

        #region 大小王

        //大王

        public int RedJoker
        {
            get { return _cards[53]; }
        }

        //小王

        public int BlackJoker
        {
            get { return _cards[52]; }
        }

        #endregion // 大小王

        #region Rank记录

        public int MasterRank
        {
            get
            {
                if (Trump == Suit.Joker)
                    return 0;

                int index = (TrumpInt - 1)*13 + Rank;
                return _cards[index];
            }
        }

        public int SubRank
        {
            get { return HeartsRankTotal + SpadesRankCount + DiamondsRankTotal + ClubsRankTotal - MasterRank; }
        }

        #endregion // Rank记录

        public object Clone()
        {
            var result = new CurrentPoker();
            result.Rank = Rank;
            result.Trump = Trump;
            for (int i = 0; i < 54; i++)
            {
                for (int j = 0; j < Cards[i]; j++)
                {
                    result.AddCard(i);
                }
            }
            return result;
        }

        public int GetMasterCardsCount()
        {
            int tmp = RedJoker + BlackJoker + MasterRank + SubRank;
            if (TrumpInt == 1)
            {
                tmp += HeartsNoRankTotal;
            }
            else if (TrumpInt == 2)
            {
                tmp += SpadesNoRankCount;
            }
            else if (TrumpInt == 3)
            {
                tmp += DiamondsNoRankTotal;
            }
            else if (TrumpInt == 4)
            {
                tmp += ClubsNoRankTotal;
            }

            return tmp;
        }

        public int GetMaxCards(int asuit)
        {
            int rt = -1;

            if (asuit == 1)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (HeartsNoRank[i] > 0)
                    {
                        return i;
                    }
                }
            }
            else if (asuit == 2)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (PeachsNoRank[i] > 0)
                    {
                        return i + 13;
                    }
                }
            }
            else if (asuit == 3)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (DiamondsNoRank[i] > 0)
                    {
                        return i + 26;
                    }
                }
            }
            else if (asuit == 4)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (ClubsNoRank[i] > 0)
                    {
                        return i + 39;
                    }
                }
            }

            return rt;
        }

        public int GetMaxCard(int asuit)
        {
            if (asuit == TrumpInt)
            {
                return GetMaxMasterCards();
            }
            return GetMaxCards(asuit);
        }

        public int GetMaxMasterCards()
        {
            int rt = -1;

            if (RedJoker > 0)
            {
                rt = 53;
                return rt;
            }
            if (BlackJoker > 0)
            {
                rt = 52;
                return rt;
            }

            if (MasterRank > 0)
            {
                rt = (TrumpInt - 1)*13 + Rank;
                return rt;
            }

            if (TrumpInt != 1)
            {
                if (HeartsRankTotal > 0)
                {
                    rt = Rank;
                    return rt;
                }
            }
            if (TrumpInt != 2)
            {
                if (SpadesRankCount > 0)
                {
                    rt = Rank + 13;
                    return rt;
                }
            }
            if (TrumpInt != 3)
            {
                if (DiamondsRankTotal > 0)
                {
                    rt = Rank + 26;
                    return rt;
                }
            }
            if (TrumpInt != 4)
            {
                if (ClubsRankTotal > 0)
                {
                    rt = Rank + 39;
                    return rt;
                }
            }


            if (TrumpInt == 1)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (HeartsNoRank[i] > 0)
                    {
                        return i;
                    }
                }
            }
            else if (TrumpInt == 2)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (PeachsNoRank[i] > 0)
                    {
                        return i + 13;
                    }
                }
            }
            else if (TrumpInt == 3)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (DiamondsNoRank[i] > 0)
                    {
                        return i + 26;
                    }
                }
            }
            else if (TrumpInt == 4)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (ClubsNoRank[i] > 0)
                    {
                        return i + 39;
                    }
                }
            }

            return rt;
        }

        public int GetMinCardsOrScores(int asuit)
        {
            int rt = -1;

            if (asuit == 1)
            {
                if (HeartsNoRank[8] == 1)
                {
                    return 8;
                }
                if (HeartsNoRank[11] == 1)
                {
                    return 11;
                }
                if (HeartsNoRank[3] == 1)
                {
                    return 3;
                }

                for (int i = 0; i < 13; i++)
                {
                    if (HeartsNoRank[i] > 0)
                    {
                        return i;
                    }
                }
            }
            else if (asuit == 2)
            {
                if (PeachsNoRank[8] == 1)
                {
                    return 21;
                }
                if (PeachsNoRank[11] == 1)
                {
                    return 24;
                }
                if (PeachsNoRank[3] == 1)
                {
                    return 16;
                }

                for (int i = 0; i < 13; i++)
                {
                    if (PeachsNoRank[i] > 0)
                    {
                        return i + 13;
                    }
                }
            }
            else if (asuit == 3)
            {
                if (DiamondsNoRank[8] == 1)
                {
                    return 34;
                }
                if (DiamondsNoRank[11] == 1)
                {
                    return 37;
                }
                if (DiamondsNoRank[3] == 1)
                {
                    return 29;
                }

                for (int i = 0; i < 13; i++)
                {
                    if (DiamondsNoRank[i] > 0)
                    {
                        return i + 26;
                    }
                }
            }
            else if (asuit == 4)
            {
                if (ClubsNoRank[8] == 1)
                {
                    return 47;
                }
                if (ClubsNoRank[11] == 1)
                {
                    return 50;
                }
                if (ClubsNoRank[3] == 1)
                {
                    return 42;
                }

                for (int i = 0; i < 13; i++)
                {
                    if (ClubsNoRank[i] > 0)
                    {
                        return i + 39;
                    }
                }
            }

            return rt;
        }

        public int GetMinCardsNoScores(int asuit)
        {
            int rt = -1;

            if (asuit == 1)
            {
                for (int i = 0; i < 13; i++)
                {
                    if ((i == 8) || (i == 11) || (i == 3))
                    {
                        continue;
                    }

                    if (HeartsNoRank[i] > 0)
                    {
                        return i;
                    }
                }
            }
            else if (asuit == 2)
            {
                for (int i = 0; i < 13; i++)
                {
                    if ((i == 21) || (i == 24) || (i == 16))
                    {
                        continue;
                    }

                    if (PeachsNoRank[i] > 0)
                    {
                        return i + 13;
                    }
                }
            }
            else if (asuit == 3)
            {
                for (int i = 0; i < 13; i++)
                {
                    if ((i == 34) || (i == 37) || (i == 29))
                    {
                        continue;
                    }

                    if (DiamondsNoRank[i] > 0)
                    {
                        return i + 26;
                    }
                }
            }
            else if (asuit == 4)
            {
                for (int i = 0; i < 13; i++)
                {
                    if ((i == 47) || (i == 50) || (i == 42))
                    {
                        continue;
                    }

                    if (ClubsNoRank[i] > 0)
                    {
                        return i + 39;
                    }
                }
            }

            return rt;
        }

        public int GetMinCards(int asuit)
        {
            int rt = -1;

            if (asuit == 1)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (HeartsNoRank[i] > 0)
                    {
                        return i;
                    }
                }
            }
            else if (asuit == 2)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (PeachsNoRank[i] > 0)
                    {
                        return i + 13;
                    }
                }
            }
            else if (asuit == 3)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (DiamondsNoRank[i] > 0)
                    {
                        return i + 26;
                    }
                }
            }
            else if (asuit == 4)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (ClubsNoRank[i] > 0)
                    {
                        return i + 39;
                    }
                }
            }


            return rt;
        }

        public int GetMinMasterCards(int asuit)
        {
            int rt = -1;

            if (asuit == 1)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (HeartsNoRank[i] > 0)
                    {
                        return i;
                    }
                }
            }
            else if (asuit == 2)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (PeachsNoRank[i] > 0)
                    {
                        return i + 13;
                    }
                }
            }
            else if (asuit == 3)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (DiamondsNoRank[i] > 0)
                    {
                        return i + 26;
                    }
                }
            }
            else if (asuit == 4)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (ClubsNoRank[i] > 0)
                    {
                        return i + 39;
                    }
                }
            }

            if (TrumpInt != 1)
            {
                if (HeartsRankTotal > 0)
                {
                    rt = Rank;
                    return rt;
                }
            }
            if (TrumpInt != 2)
            {
                if (SpadesRankCount > 0)
                {
                    rt = Rank + 13;
                    return rt;
                }
            }
            if (TrumpInt != 3)
            {
                if (DiamondsRankTotal > 0)
                {
                    rt = Rank + 26;
                    return rt;
                }
            }
            if (TrumpInt != 4)
            {
                if (ClubsRankTotal > 0)
                {
                    rt = Rank + 39;
                    return rt;
                }
            }

            if (MasterRank > 0)
            {
                rt = (TrumpInt - 1)*13 + Rank;
                return rt;
            }

            if (BlackJoker > 0)
            {
                rt = 52;
                return rt;
            }
            if (RedJoker > 0)
            {
                rt = 53;
                return rt;
            }


            return rt;
        }

        public int[] GetSuitCardsWithJokerAndRank(int asuit)
        {
            var list = new ArrayList();
            if (asuit == 5)
            {
                if (Rank != 53)
                {
                    if (SpadesRankCount == 1)
                    {
                        list.Add(13 + Rank);
                    }
                    else if (SpadesRankCount == 2)
                    {
                        list.Add(13 + Rank);
                        list.Add(13 + Rank);
                    }
                    if (DiamondsRankTotal == 1)
                    {
                        list.Add(26 + Rank);
                    }
                    else if (DiamondsRankTotal == 2)
                    {
                        list.Add(26 + Rank);
                        list.Add(26 + Rank);
                    }
                    if (ClubsRankTotal == 1)
                    {
                        list.Add(39 + Rank);
                    }
                    else if (ClubsRankTotal == 2)
                    {
                        list.Add(39 + Rank);
                        list.Add(39 + Rank);
                    }
                    //
                    if (HeartsRankTotal == 1)
                    {
                        list.Add(Rank);
                    }
                    else if (HeartsRankTotal == 2)
                    {
                        list.Add(Rank);
                        list.Add(Rank);
                    }
                }


                if (BlackJoker == 1)
                {
                    list.Add(52);
                }
                else if (BlackJoker == 2)
                {
                    list.Add(52);
                    list.Add(52);
                }
                if (RedJoker == 1)
                {
                    list.Add(53);
                }
                else if (RedJoker == 2)
                {
                    list.Add(53);
                    list.Add(53);
                }
            }
            else if (asuit == TrumpInt)
            {
                if (asuit == 1)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (HeartsNoRank[i] == 1)
                        {
                            list.Add(i);
                        }
                        else if (HeartsNoRank[i] == 2)
                        {
                            list.Add(i);
                            list.Add(i);
                        }
                    }

                    //
                    if (SpadesRankCount == 1)
                    {
                        list.Add(13 + Rank);
                    }
                    else if (SpadesRankCount == 2)
                    {
                        list.Add(13 + Rank);
                        list.Add(13 + Rank);
                    }
                    if (DiamondsRankTotal == 1)
                    {
                        list.Add(26 + Rank);
                    }
                    else if (DiamondsRankTotal == 2)
                    {
                        list.Add(26 + Rank);
                        list.Add(26 + Rank);
                    }
                    if (ClubsRankTotal == 1)
                    {
                        list.Add(39 + Rank);
                    }
                    else if (ClubsRankTotal == 2)
                    {
                        list.Add(39 + Rank);
                        list.Add(39 + Rank);
                    }
                    //
                    if (HeartsRankTotal == 1)
                    {
                        list.Add(Rank);
                    }
                    else if (HeartsRankTotal == 2)
                    {
                        list.Add(Rank);
                        list.Add(Rank);
                    }
                    //
                    if (BlackJoker == 1)
                    {
                        list.Add(52);
                    }
                    else if (BlackJoker == 2)
                    {
                        list.Add(52);
                        list.Add(52);
                    }
                    if (RedJoker == 1)
                    {
                        list.Add(53);
                    }
                    else if (RedJoker == 2)
                    {
                        list.Add(53);
                        list.Add(53);
                    }
                }
                else if (asuit == 2)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (PeachsNoRank[i] == 1)
                        {
                            list.Add(i + 13);
                        }
                        else if (PeachsNoRank[i] == 2)
                        {
                            list.Add(i + 13);
                            list.Add(i + 13);
                        }
                    }

                    //
                    if (HeartsRankTotal == 1)
                    {
                        list.Add(Rank);
                    }
                    else if (HeartsRankTotal == 2)
                    {
                        list.Add(Rank);
                        list.Add(Rank);
                    }
                    if (DiamondsRankTotal == 1)
                    {
                        list.Add(26 + Rank);
                    }
                    else if (DiamondsRankTotal == 2)
                    {
                        list.Add(26 + Rank);
                        list.Add(26 + Rank);
                    }
                    if (ClubsRankTotal == 1)
                    {
                        list.Add(39 + Rank);
                    }
                    else if (ClubsRankTotal == 2)
                    {
                        list.Add(39 + Rank);
                        list.Add(39 + Rank);
                    }
                    //
                    if (SpadesRankCount == 1)
                    {
                        list.Add(13 + Rank);
                    }
                    else if (SpadesRankCount == 2)
                    {
                        list.Add(13 + Rank);
                        list.Add(13 + Rank);
                    }
                    //
                    if (BlackJoker == 1)
                    {
                        list.Add(52);
                    }
                    else if (BlackJoker == 2)
                    {
                        list.Add(52);
                        list.Add(52);
                    }
                    if (RedJoker == 1)
                    {
                        list.Add(53);
                    }
                    else if (RedJoker == 2)
                    {
                        list.Add(53);
                        list.Add(53);
                    }
                }
                else if (asuit == 3)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (DiamondsNoRank[i] == 1)
                        {
                            list.Add(i + 26);
                        }
                        else if (DiamondsNoRank[i] == 2)
                        {
                            list.Add(i + 26);
                            list.Add(i + 26);
                        }
                    }

                    //
                    if (SpadesRankCount == 1)
                    {
                        list.Add(13 + Rank);
                    }
                    else if (SpadesRankCount == 2)
                    {
                        list.Add(13 + Rank);
                        list.Add(13 + Rank);
                    }
                    if (HeartsRankTotal == 1)
                    {
                        list.Add(Rank);
                    }
                    else if (HeartsRankTotal == 2)
                    {
                        list.Add(Rank);
                        list.Add(Rank);
                    }

                    //
                    if (DiamondsRankTotal == 1)
                    {
                        list.Add(26 + Rank);
                    }
                    else if (DiamondsRankTotal == 2)
                    {
                        list.Add(26 + Rank);
                        list.Add(26 + Rank);
                    }
                    if (ClubsRankTotal == 1)
                    {
                        list.Add(39 + Rank);
                    }
                    else if (ClubsRankTotal == 2)
                    {
                        list.Add(39 + Rank);
                        list.Add(39 + Rank);
                    }


                    //
                    if (BlackJoker == 1)
                    {
                        list.Add(52);
                    }
                    else if (BlackJoker == 2)
                    {
                        list.Add(52);
                        list.Add(52);
                    }
                    if (RedJoker == 1)
                    {
                        list.Add(53);
                    }
                    else if (RedJoker == 2)
                    {
                        list.Add(53);
                        list.Add(53);
                    }
                }
                else if (asuit == 4)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (ClubsNoRank[i] == 1)
                        {
                            list.Add(i + 39);
                        }
                        else if (ClubsNoRank[i] == 2)
                        {
                            list.Add(i + 39);
                            list.Add(i + 39);
                        }
                    }

                    //
                    if (HeartsRankTotal == 1)
                    {
                        list.Add(Rank);
                    }
                    else if (HeartsRankTotal == 2)
                    {
                        list.Add(Rank);
                        list.Add(Rank);
                    }

                    if (SpadesRankCount == 1)
                    {
                        list.Add(13 + Rank);
                    }
                    else if (SpadesRankCount == 2)
                    {
                        list.Add(13 + Rank);
                        list.Add(13 + Rank);
                    }
                    if (DiamondsRankTotal == 1)
                    {
                        list.Add(26 + Rank);
                    }
                    else if (DiamondsRankTotal == 2)
                    {
                        list.Add(26 + Rank);
                        list.Add(26 + Rank);
                    }

                    //
                    if (ClubsRankTotal == 1)
                    {
                        list.Add(39 + Rank);
                    }
                    else if (ClubsRankTotal == 2)
                    {
                        list.Add(39 + Rank);
                        list.Add(39 + Rank);
                    }
                    //
                    if (BlackJoker == 1)
                    {
                        list.Add(52);
                    }
                    else if (BlackJoker == 2)
                    {
                        list.Add(52);
                        list.Add(52);
                    }
                    if (RedJoker == 1)
                    {
                        list.Add(53);
                    }
                    else if (RedJoker == 2)
                    {
                        list.Add(53);
                        list.Add(53);
                    }
                }
            }
            else
            {
                if (asuit == 1)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (HeartsNoRank[i] == 1)
                        {
                            list.Add(i);
                        }
                        else if (HeartsNoRank[i] == 2)
                        {
                            list.Add(i);
                            list.Add(i);
                        }
                    }
                }
                else if (asuit == 2)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (PeachsNoRank[i] == 1)
                        {
                            list.Add(i + 13);
                        }
                        else if (PeachsNoRank[i] == 2)
                        {
                            list.Add(i + 13);
                            list.Add(i + 13);
                        }
                    }
                }
                else if (asuit == 3)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (DiamondsNoRank[i] == 1)
                        {
                            list.Add(i + 26);
                        }
                        else if (DiamondsNoRank[i] == 2)
                        {
                            list.Add(i + 26);
                            list.Add(i + 26);
                        }
                    }
                }
                else if (asuit == 4)
                {
                    for (int i = 0; i < 13; i++)
                    {
                        if (ClubsNoRank[i] == 1)
                        {
                            list.Add(i + 39);
                        }
                        else if (ClubsNoRank[i] == 2)
                        {
                            list.Add(i + 39);
                            list.Add(i + 39);
                        }
                    }
                }
            }

            return (int[]) list.ToArray(typeof (int));
        }

        public int[] GetSuitAllCardsWithoutRankAndJoker(int asuit)
        {
            if (asuit == 1)
            {
                return HeartsNoRank;
            }
            if (asuit == 2)
            {
                return PeachsNoRank;
            }
            if (asuit == 3)
            {
                return DiamondsNoRank;
            }
            if (asuit == 4)
            {
                return ClubsNoRank;
            }
            return new int[13];
        }


        //增加一张牌
        public void AddCard(int number)
        {
            if (number < 0 || number > 53)
                return;
            _cards[number] = _cards[number] + 1;
        }

        //增加一张牌
        public void RemoveCard(int number)
        {
            if (number < 0 || number > 53)
                return;
            if (_cards[number] > 0)
            {
                _cards[number] = _cards[number] - 1;
            }
        }

        //全部清空
        public void Clear()
        {
            _cards = new int[54];
        }

        //是否是混合出牌
        public bool IsMixed()
        {
            int[] c = {0, 0, 0, 0, 0};

            for (int i = 0; i < 13; i++)
            {
                if (HeartsNoRank[i] > 0)
                {
                    c[0]++;
                    break;
                }
            }
            for (int i = 0; i < 13; i++)
            {
                if (PeachsNoRank[i] > 0)
                {
                    c[1]++;
                    break;
                }
            }
            for (int i = 0; i < 13; i++)
            {
                if (DiamondsNoRank[i] > 0)
                {
                    c[2]++;
                    break;
                }
            }
            for (int i = 0; i < 13; i++)
            {
                if (ClubsNoRank[i] > 0)
                {
                    c[3]++;
                    break;
                }
            }

            if (HeartsRankTotal > 0 || SpadesRankCount > 0 || DiamondsRankTotal > 0 || ClubsRankTotal > 0 ||
                BlackJoker > 0 || RedJoker > 0)
                c[4] = 1;

            if (Trump == Suit.Heart)
            {
                c[0] = Math.Max(c[0], c[4]);
            }

            else if (Trump == Suit.Spade)
            {
                c[1] = Math.Max(c[1], c[4]);
            }
            else if (Trump == Suit.Diamond)
            {
                c[2] = Math.Max(c[2], c[4]);
            }
            else if (Trump == Suit.Club)
            {
                c[3] = Math.Max(c[3], c[4]);
            }

            c[4] = 0;
            return c.Sum() > 1;
        }

        //是否有对
        public ArrayList GetPairs()
        {
            var list = new ArrayList();
            for (int i = 0; i < 13; i++)
            {
                if (Hearts[i] > 1)
                {
                    list.Add(i);
                }
                if (Spades[i] > 1)
                {
                    list.Add(i + 13);
                }
                if (Diamonds[i] > 1)
                {
                    list.Add(i + 26);
                }
                if (Clubs[i] > 1)
                {
                    list.Add(i + 39);
                }
            }


            if (BlackJoker > 1)
            {
                list.Add(52);
            }

            if (RedJoker > 1)
            {
                list.Add(53);
            }
            return list;
        }

        public ArrayList GetPairs(int asuit)
        {
            if (asuit == TrumpInt)
            {
                return GetMasterPairs();
            }
            return GetNoRankPairs(asuit);
        }

        public ArrayList GetMasterPairs()
        {
            var list = new ArrayList();
            for (int i = 0; i < 13; i++)
            {
                if (TrumpInt == 1)
                {
                    if (HeartsNoRank[i] > 1)
                    {
                        list.Add(i);
                    }
                }
                if (TrumpInt == 2)
                {
                    if (PeachsNoRank[i] > 1)
                    {
                        list.Add(i + 13);
                    }
                }
                if (TrumpInt == 3)
                {
                    if (DiamondsNoRank[i] > 1)
                    {
                        list.Add(i + 26);
                    }
                }
                if (TrumpInt == 4)
                {
                    if (ClubsNoRank[i] > 1)
                    {
                        list.Add(i + 39);
                    }
                }
            }

            if (TrumpInt != 1)
            {
                if (HeartsRankTotal > 1)
                {
                    list.Add(Rank);
                }
            }
            if (TrumpInt != 2)
            {
                if (SpadesRankCount > 1)
                {
                    list.Add(Rank + 13);
                }
            }
            if (TrumpInt != 3)
            {
                if (DiamondsRankTotal > 1)
                {
                    list.Add(Rank + 26);
                }
            }
            if (TrumpInt != 4)
            {
                if (ClubsRankTotal > 1)
                {
                    list.Add(Rank + 39);
                }
            }

            if (MasterRank == 2)
            {
                list.Add((TrumpInt - 1)*13 + Rank);
            }

            if (BlackJoker > 1)
            {
                list.Add(52);
            }
            if (RedJoker > 1)
            {
                list.Add(53);
            }

            return list;
        }

        public ArrayList GetSubRankPairs()
        {
            var list = new ArrayList();
            if (TrumpInt != 1)
            {
                if (HeartsRankTotal == 2)
                {
                    list.Add(Rank);
                }
            }
            if (TrumpInt != 2)
            {
                if (SpadesRankCount == 2)
                {
                    list.Add(13 + Rank);
                }
            }
            if (TrumpInt != 3)
            {
                if (DiamondsRankTotal == 2)
                {
                    list.Add(26 + Rank);
                }
            }
            if (TrumpInt != 4)
            {
                if (ClubsRankTotal == 2)
                {
                    list.Add(39 + Rank);
                }
            }

            return list;
        }

        public ArrayList GetNoRankPairs(int asuit)
        {
            var list = new ArrayList();

            if ((asuit == 1))
            {
                for (int i = 0; i < 13; i++)
                {
                    if (HeartsNoRank[i] > 1)
                    {
                        list.Add(i);
                    }
                }
            }
            else if ((asuit == 2))
            {
                for (int i = 0; i < 13; i++)
                {
                    if (PeachsNoRank[i] > 1)
                    {
                        list.Add(i + 13);
                    }
                }
            }
            else if ((asuit == 3))
            {
                for (int i = 0; i < 13; i++)
                {
                    if (DiamondsNoRank[i] > 1)
                    {
                        list.Add(i + 26);
                    }
                }
            }
            else if ((asuit == 4))
            {
                for (int i = 0; i < 13; i++)
                {
                    if (ClubsNoRank[i] > 1)
                    {
                        list.Add(i + 39);
                    }
                }
            }
            else if ((asuit == 5))
            {
                if (BlackJoker > 1)
                {
                    list.Add(52);
                }
                if (RedJoker > 1)
                {
                    list.Add(53);
                }
            }

            return list;
        }

        public ArrayList GetNoRankPairs()
        {
            var list = new ArrayList();
            for (int i = 0; i < 13; i++)
            {
                if (HeartsNoRank[i] > 1)
                {
                    list.Add(i);
                }
                if (PeachsNoRank[i] > 1)
                {
                    list.Add(i + 13);
                }
                if (DiamondsNoRank[i] > 1)
                {
                    list.Add(i + 26);
                }
                if (ClubsNoRank[i] > 1)
                {
                    list.Add(i + 39);
                }
            }

            return list;
        }

        public ArrayList GetNoRankNoSuitPairs()
        {
            var list = new ArrayList();
            for (int i = 0; i < 13; i++)
            {
                if (TrumpInt != 1)
                {
                    if (HeartsNoRank[i] > 1)
                    {
                        list.Add(i);
                    }
                }
                if (TrumpInt != 2)
                {
                    if (PeachsNoRank[i] > 1)
                    {
                        list.Add(i + 13);
                    }
                }
                if (TrumpInt != 3)
                {
                    if (DiamondsNoRank[i] > 1)
                    {
                        list.Add(i + 26);
                    }
                }
                if (TrumpInt != 4)
                {
                    if (ClubsNoRank[i] > 1)
                    {
                        list.Add(i + 39);
                    }
                }
            }

            return list;
        }

        public bool HasMasterRankPairs()
        {
            if (Rank > 12)
            {
                return false;
            }

            if (MasterRank > 1)
            {
                return true;
            }
            return false;
        }

        public bool HasSubRankPairs()
        {
            if (Rank > 12)
            {
                return false;
            }

            int count = 0;
            if (Hearts[Rank] > 1)
            {
                count++;
            }
            if (Spades[Rank] > 1)
            {
                count++;
            }
            if (Diamonds[Rank] > 1)
            {
                count++;
            }
            if (Clubs[Rank] > 1)
            {
                count++;
            }

            if (HasMasterRankPairs())
            {
                count--;
            }

            if (count > 0)
            {
                return true;
            }
            return false;
        }

        //是否有拖拉机
        public bool HasTractors()
        {
            ArrayList list = GetPairs();
            if (list.Count == 0)
            {
                return false;
            }

            if (GetTractor() == -1)
            {
                return false;
            }
            return true;
        }

        public int GetTractor()
        {
            //大小王
            if ((BlackJoker == 2) && (RedJoker == 2))
            {
                return 53;
            }
            //小王主花色
            if ((BlackJoker == 2) && (MasterRank == 2))
            {
                return 52;
            }


            //主花色副花色
            if ((MasterRank == 2) && HasSubRankPairs())
            {
                return ((TrumpInt - 1)*13 + Rank);
            }

            //副花色A时
            if (HasSubRankPairs())
            {
                ArrayList a = GetSubRankPairs();

                int m = 12;
                if (Rank == 12)
                {
                    m = 11;
                }

                if ((TrumpInt == 1) && (Hearts[m] > 1))
                {
                    return (int) a[0];
                }
                if ((TrumpInt == 2) && (Spades[m] > 1))
                {
                    return (int) a[0];
                }
                if ((TrumpInt == 3) && (Diamonds[m] > 1))
                {
                    return (int) a[0];
                }
                if ((TrumpInt == 4) && (Clubs[m] > 1))
                {
                    return (int) a[0];
                }
            }


            //顺序比较
            for (int i = 12; i > 0; i--)
            {
                if (i == Rank)
                {
                    continue;
                }
                int m = i - 1;
                if (m == Rank)
                {
                    m--;
                }
                if (m < 0)
                {
                    break;
                }


                if ((HeartsNoRank[i] > 1) && (HeartsNoRank[m] > 1))
                {
                    return i;
                }
                if ((PeachsNoRank[i] > 1) && (PeachsNoRank[m] > 1))
                {
                    return (i + 13);
                }
                if ((DiamondsNoRank[i] > 1) && (DiamondsNoRank[m] > 1))
                {
                    return (i + 26);
                }
                if ((ClubsNoRank[i] > 1) && (ClubsNoRank[m] > 1))
                {
                    return (i + 39);
                }
            }

            return -1;
        }

        public int GetTractor(int asuit)
        {
            if (asuit == TrumpInt)
            {
                return GetMasterTractor();
            }
            //顺序比较
            for (int i = 12; i > 0; i--)
            {
                if (i == Rank)
                {
                    continue;
                }
                int m = i - 1;
                if (m == Rank)
                {
                    m--;
                }
                if (m < 0)
                {
                    break;
                }

                if (asuit == 1)
                {
                    if ((HeartsNoRank[i] > 1) && (HeartsNoRank[m] > 1))
                    {
                        return i;
                    }
                }
                if (asuit == 2)
                {
                    if ((PeachsNoRank[i] > 1) && (PeachsNoRank[m] > 1))
                    {
                        return (i + 13);
                    }
                }
                if (asuit == 3)
                {
                    if ((DiamondsNoRank[i] > 1) && (DiamondsNoRank[m] > 1))
                    {
                        return (i + 26);
                    }
                }
                if (asuit == 4)
                {
                    if ((ClubsNoRank[i] > 1) && (ClubsNoRank[m] > 1))
                    {
                        return (i + 39);
                    }
                }
            }

            return -1;
        }

        public List<int> GetTractorOfAnySuit()
        {
            List<int> result = GetTractor(Suit.Club);
            if (result.Count > 1)
                return result;
            result = GetTractor(Suit.Diamond);
            if (result.Count > 1)
                return result;
            result = GetTractor(Suit.Heart);
            if (result.Count > 1)
                return result;
            result = GetTractor(Suit.Spade);
            if (result.Count > 1)
                return result;

            result = GetTractor(Suit.Joker);
            if (result.Count > 1)
                return result;

            return new List<int>();
        }

        public List<int> GetTractor(Suit suit)
        {
            if (suit == Trump)
                return GetTrumpTractor();

            var result = new List<int>();
            //顺序比较
            for (int i = 12; i > -1; i--)
            {
                if (i == Rank)
                {
                    continue;
                }

                if (i < 0)
                {
                    break;
                }

                if (suit == Suit.Heart)
                {
                    if (HeartsNoRank[i] > 1)
                        result.Add(i);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }

                else if (suit == Suit.Spade)
                {
                    if (PeachsNoRank[i] > 1)
                        result.Add(i + 13);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }

                else if (suit == Suit.Diamond)
                {
                    if (DiamondsNoRank[i] > 1)
                        result.Add(i + 26);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }
                else if (suit == Suit.Club)
                {
                    if (ClubsNoRank[i] > 1)
                        result.Add(i + 39);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }
            }

            if (result.Count < 1)
                result.Clear();

            return result;
        }

        public List<int> GetTrumpTractor()
        {
            var result = new List<int>();
            //大小王
            if (RedJoker == 2)
            {
                result.Add(53);
            }
            //小王主花色
            if (BlackJoker == 2)
            {
                result.Add(52);
            }
            else
                result.Clear();


            //主花色副花色
            if (MasterRank == 2)
                result.Add((TrumpInt - 1)*13 + Rank);
            else if (result.Count > 1)
                return result;
            else
                result.Clear();
            //副花色A时
            if (HasSubRankPairs())
            {
                ArrayList a = GetSubRankPairs();
                result.Add((int) a[0]);
            }
            else if (result.Count > 1)
                return result;
            else
                result.Clear();


            //顺序比较
            for (int i = 12; i > 0; i--)
            {
                if (i == Rank)
                {
                    continue;
                }

                if (i < 0)
                {
                    break;
                }

                if (Trump == Suit.Heart)
                {
                    if (HeartsNoRank[i] > 1)
                        result.Add(i);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }

                else if (Trump == Suit.Spade)
                {
                    if (PeachsNoRank[i] > 1)
                        result.Add(i + 13);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }

                else if (Trump == Suit.Diamond)
                {
                    if (DiamondsNoRank[i] > 1)
                        result.Add(i + 26);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }
                else if (Trump == Suit.Club)
                {
                    if (ClubsNoRank[i] > 1)
                        result.Add(i + 39);
                    else if (result.Count > 1)
                        return result;
                    else
                        result.Clear();
                }
            }

            if (result.Count < 1)
                result.Clear();
            return result;
        }

        public int GetMasterTractor()
        {
            //大小王
            if ((BlackJoker == 2) && (RedJoker == 2))
            {
                return 53;
            }
            //小王主花色
            if ((BlackJoker == 2) && (MasterRank == 2))
            {
                return 52;
            }


            //主花色副花色
            if ((MasterRank == 2) && HasSubRankPairs())
            {
                return ((TrumpInt - 1)*13 + Rank);
            }

            //副花色A时
            if (HasSubRankPairs())
            {
                ArrayList a = GetSubRankPairs();
                int m = Rank;
                if (Rank == 12)
                {
                    m = 11;
                }

                if ((TrumpInt == 1) && (Hearts[m] > 1))
                {
                    return (int) a[0];
                }
                if ((TrumpInt == 2) && (Spades[m] > 1))
                {
                    return (int) a[0];
                }
                if ((TrumpInt == 3) && (Diamonds[m] > 1))
                {
                    return (int) a[0];
                }
                if ((TrumpInt == 4) && (Clubs[m] > 1))
                {
                    return (int) a[0];
                }
            }


            //顺序比较
            for (int i = 12; i > 0; i--)
            {
                if (i == Rank)
                {
                    continue;
                }
                int m = i - 1;
                if (m == Rank)
                {
                    m--;
                }
                if (m < 0)
                {
                    break;
                }

                if (TrumpInt == 1)
                {
                    if ((HeartsNoRank[i] > 1) && (HeartsNoRank[m] > 1))
                    {
                        return i;
                    }
                }
                if (TrumpInt == 2)
                {
                    if ((PeachsNoRank[i] > 1) && (PeachsNoRank[m] > 1))
                    {
                        return (i + 13);
                    }
                }
                if (TrumpInt == 3)
                {
                    if ((DiamondsNoRank[i] > 1) && (DiamondsNoRank[m] > 1))
                    {
                        return (i + 26);
                    }
                }
                if (TrumpInt == 4)
                {
                    if ((ClubsNoRank[i] > 1) && (ClubsNoRank[m] > 1))
                    {
                        return (i + 39);
                    }
                }
            }

            return -1;
        }

        public int[] GetTractorOtherCards(int max)
        {
            //大小王
            if (max == 53)
            {
                return new[] {53, 52, 52};
            }
            //小王主花色
            if (max == 52)
            {
                return new[] {52, (TrumpInt - 1)*13 + Rank, (TrumpInt - 1)*13 + Rank};
            }


            //主花色副花色
            if (max == ((TrumpInt - 1)*13 + Rank))
            {
                ArrayList a = GetSubRankPairs();
                return new[] {(TrumpInt - 1)*13 + Rank, (int) a[0], (int) a[0]};
            }

            //副花色A时
            if (HasSubRankPairs())
            {
                ArrayList a = GetSubRankPairs();

                if ((int) a[0] == max)
                {
                    int m = 12;
                    if (Rank == 12)
                    {
                        m = 11;
                    }

                    if ((TrumpInt == 1) && (Hearts[m] > 1))
                    {
                        return new[] {(int) a[0], m, m};
                    }
                    if ((TrumpInt == 2) && (Spades[m] > 1))
                    {
                        return new[] {(int) a[0], m + 13, m + 13};
                    }
                    if ((TrumpInt == 3) && (Diamonds[m] > 1))
                    {
                        return new[] {(int) a[0], m + 26, m + 26};
                    }
                    if ((TrumpInt == 4) && (Clubs[m] > 1))
                    {
                        return new[] {(int) a[0], m + 39, m + 39};
                    }
                }
            }

            //顺序比较
            for (int i = 12; i > 0; i--)
            {
                if (TrumpInt == 1)
                {
                    int m = i - 1;
                    if (m == Rank)
                    {
                        m--;
                    }
                    if (m < 0)
                    {
                        break;
                    }

                    if (max == i)
                    {
                        if ((HeartsNoRank[i] > 1) && (HeartsNoRank[m] > 1))
                        {
                            return new[] {i, m, m};
                        }
                    }
                }
                if (TrumpInt == 2)
                {
                    if ((max - 13) == i)
                    {
                        int m = i - 1;
                        if (m == Rank)
                        {
                            m--;
                        }
                        if (m < 0)
                        {
                            break;
                        }

                        if ((PeachsNoRank[i] > 1) && (PeachsNoRank[m] > 1))
                        {
                            return new[] {i + 13, m + 13, m + 13};
                        }
                    }
                }
                if (TrumpInt == 3)
                {
                    if ((max - 26) == i)
                    {
                        int m = i - 1;
                        if (m == Rank)
                        {
                            m--;
                        }
                        if (m < 0)
                        {
                            break;
                        }

                        if ((DiamondsNoRank[i] > 1) && (DiamondsNoRank[m] > 1))
                        {
                            return new[] {i + 26, m + 26, m + 26};
                        }
                    }
                }
                if (TrumpInt == 4)
                {
                    if ((max - 39) == i)
                    {
                        int m = i - 1;
                        if (m == Rank)
                        {
                            m--;
                        }
                        if (m < 0)
                        {
                            break;
                        }

                        if ((ClubsNoRank[i] > 1) && (ClubsNoRank[m] > 1))
                        {
                            return new[] {i + 39, m + 39, m + 39};
                        }
                    }
                }
            }


            //顺序比较
            for (int i = 12; i > 0; i--)
            {
                if (TrumpInt != 1)
                {
                    if (max == i)
                    {
                        int m = i - 1;
                        if (m == Rank)
                        {
                            m--;
                        }
                        if (m < 0)
                        {
                            break;
                        }
                        if ((HeartsNoRank[i] > 1) && (HeartsNoRank[m] > 1))
                        {
                            return new[] {i, m, m};
                        }
                    }
                }
                if (TrumpInt != 2)
                {
                    if ((max - 13) == i)
                    {
                        int m = i - 1;
                        if (m == Rank)
                        {
                            m--;
                        }
                        if (m < 0)
                        {
                            break;
                        }
                        if ((PeachsNoRank[i] > 1) && (PeachsNoRank[m] > 1))
                        {
                            return new[] {i + 13, m + 13, m + 13};
                        }
                    }
                }
                if (TrumpInt != 3)
                {
                    if ((max - 26) == i)
                    {
                        int m = i - 1;
                        if (m == Rank)
                        {
                            m--;
                        }
                        if (m < 0)
                        {
                            break;
                        }

                        if ((DiamondsNoRank[i] > 1) && (DiamondsNoRank[m] > 1))
                        {
                            return new[] {i + 26, m + 26, m + 26};
                        }
                    }
                }
                if (TrumpInt != 4)
                {
                    if ((max - 39) == i)
                    {
                        int m = i - 1;
                        if (m == Rank)
                        {
                            m--;
                        }
                        if (m < 0)
                        {
                            break;
                        }
                        if ((ClubsNoRank[i] > 1) && (ClubsNoRank[m] > 1))
                        {
                            return new[] {i + 39, m + 39, m + 39};
                        }
                    }
                }
            }

            return null;
        }

        public int GetNoRankNoSuitTractor()
        {
            //顺序比较
            for (int i = 12; i > 0; i--)
            {
                if (TrumpInt != 1)
                {
                    if ((HeartsNoRank[i] > 1) && (HeartsNoRank[i - 1] > 1))
                    {
                        return i;
                    }
                }
                if (TrumpInt != 2)
                {
                    if ((PeachsNoRank[i] > 1) && (PeachsNoRank[i - 1] > 1))
                    {
                        return (i + 13);
                    }
                }
                if (TrumpInt != 3)
                {
                    if ((DiamondsNoRank[i] > 1) && (DiamondsNoRank[i - 1] > 1))
                    {
                        return (i + 26);
                    }
                }
                if (TrumpInt != 4)
                {
                    if ((ClubsNoRank[i] > 1) && (ClubsNoRank[i - 1] > 1))
                    {
                        return (i + 39);
                    }
                }
            }

            return -1;
        }


        //比较单张副牌
        public bool CompareTo(int number)
        {
            int masterCards = GetMasterCardsCount();

            if (number >= 0 && number < 13)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (HeartsNoRank[i] > 0)
                    {
                        if (number >= i)
                            return false;
                        return true;
                    }
                }

                if (masterCards > 0)
                {
                    return true;
                }
                return false;
            }
            if (number >= 13 && number < 26)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (PeachsNoRank[i] > 0)
                    {
                        if ((number - 13) >= i)
                            return false;
                        return true;
                    }
                }


                if (masterCards > 0)
                {
                    return true;
                }
                return false;
            }
            if (number >= 26 && number < 39)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (DiamondsNoRank[i] > 0)
                    {
                        if ((number - 26) >= i)
                            return false;
                        return true;
                    }
                }

                if (masterCards > 0)
                {
                    return true;
                }
                return false;
            }
            if (number >= 39 && number < 52)
            {
                for (int i = 12; i > -1; i--)
                {
                    if (ClubsNoRank[i] > 0)
                    {
                        if ((number - 39) >= i)
                            return false;
                        return true;
                    }
                }

                if (masterCards > 0)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        //比较对
        public bool CompareTo(int[] numbers)
        {
            if (numbers.Length >= 6)
            {
                return false;
            }

            var al = new ArrayList();
            if (numbers[0] >= 0 && numbers[0] < 13)
            {
                al = GetNoRankPairs(1);
            }
            else if (numbers[0] >= 13 && numbers[0] < 26)
            {
                al = GetNoRankPairs(2);
            }
            else if (numbers[0] >= 26 && numbers[0] < 39)
            {
                al = GetNoRankPairs(3);
            }
            else if (numbers[0] >= 39 && numbers[0] < 52)
            {
                al = GetNoRankPairs(4);
            }

            if (al.Count == 0)
            {
                return false;
            }
            if (al.Count >= 0)
            {
                if ((int) al[0] - numbers[0] >= 0)
                {
                    return true;
                }
                return false;
            }

            return true;
        }


        public bool HasSomeCards(int suit)
        {
            if (suit == TrumpInt)
            {
                int count = HeartsRankTotal + SpadesRankCount + DiamondsRankTotal + ClubsRankTotal;
                count = count + MasterRank + SubRank + RedJoker + BlackJoker;
                if (suit == 1)
                {
                    count += HeartsNoRankTotal;
                }
                else if (suit == 2)
                {
                    count += SpadesNoRankCount;
                }
                else if (suit == 3)
                {
                    count += DiamondsNoRankTotal;
                }
                else if (suit == 4)
                {
                    count += ClubsNoRankTotal;
                }

                if (count > 0)
                    return true;
                return false;
            }
            if (suit == 1)
            {
                if (HeartsNoRankTotal > 0)
                {
                    return true;
                }
                return false;
            }

            if (suit == 2)
            {
                if (SpadesNoRankCount > 0)
                {
                    return true;
                }
                return false;
            }

            if (suit == 3)
            {
                if (DiamondsNoRankTotal > 0)
                {
                    return true;
                }
                return false;
            }

            if (suit == 4)
            {
                if (ClubsNoRankTotal > 0)
                {
                    return true;
                }
                return false;
            }

            if (suit == 5)
            {
                if ((BlackJoker + RedJoker) > 0)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        public int GetCardCount(int number)
        {
            if (number == 53)
            {
                return RedJoker;
            }
            if (number == 52)
            {
                return BlackJoker;
            }
            if (number >= 0 && number < 13)
            {
                return Hearts[number];
            }
            if (number >= 13 && number < 26)
            {
                return Spades[number - 13];
            }
            if (number >= 26 && number < 39)
            {
                return Diamonds[number - 26];
            }
            if (number >= 39 && number < 52)
            {
                return Clubs[number - 39];
            }

            return 0;
        }


        public int[] GetCards()
        {
            var cards = new int[54];
            for (int i = 0; i < 13; i++)
            {
                cards[i] = Hearts[i];
            }
            for (int i = 0; i < 13; i++)
            {
                cards[i + 13] = Spades[i];
            }
            for (int i = 0; i < 13; i++)
            {
                cards[i + 26] = Diamonds[i];
            }
            for (int i = 0; i < 13; i++)
            {
                cards[i + 39] = Clubs[i];
            }
            cards[52] = BlackJoker;
            cards[53] = RedJoker;
            return cards;
        }

        public string getAllCards()
        {
            string pokers = "";

            for (int i = 0; i < 13; i++)
            {
                if (Hearts[i] == 1)
                {
                    pokers = pokers + i + ",";
                }
                else if (Hearts[i] == 2)
                {
                    pokers = pokers + i + "," + i + ",";
                }
            }

            for (int i = 0; i < 13; i++)
            {
                if (Spades[i] == 1)
                {
                    pokers = pokers + (i + 13) + ",";
                }
                else if (Spades[i] == 2)
                {
                    pokers = pokers + (i + 13) + "," + (i + 13) + ",";
                }
            }

            for (int i = 0; i < 13; i++)
            {
                if (Diamonds[i] == 1)
                {
                    pokers = pokers + (i + 26) + ",";
                }
                else if (Diamonds[i] == 2)
                {
                    pokers = pokers + (i + 26) + "," + (i + 26) + ",";
                }
            }

            for (int i = 0; i < 13; i++)
            {
                if (Clubs[i] == 1)
                {
                    pokers = pokers + (i + 39) + ",";
                }
                else if (Clubs[i] == 2)
                {
                    pokers = pokers + (i + 39) + "," + (i + 39) + ",";
                }
            }

            if (BlackJoker == 1)
            {
                pokers = pokers + 52 + ",";
            }
            else if (BlackJoker == 2)
            {
                pokers = pokers + 52 + "," + 52 + ",";
            }

            if (RedJoker == 1)
            {
                pokers = pokers + 53 + ",";
            }
            else if (RedJoker == 2)
            {
                pokers = pokers + 53 + "," + 53 + ",";
            }

            if (pokers.EndsWith(","))
            {
                pokers = pokers.Substring(0, pokers.Length - 1);
            }

            return pokers;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int i = 0; i < 54; i++)
            {
                for (int j = Cards[i]; j > 0; j--)
                {
                    result.Append(i + ", ");
                }
            }

            return result.ToString();
        }
    }
}