using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Duan.Xiugang.Tractor.Objects
{
    /// <summary>
    ///     通用处理类.
    ///     用来处理程序中常用的方法，比如解析等.
    /// </summary>
    public class CommonMethods
    {

        /// <summary>
        ///     得到一个牌的花色
        /// </summary>
        /// <param name="a">牌值</param>
        /// <returns>花色</returns>
        internal static int GetSuit(int a)
        {
            if (a >= 0 && a < 13)
            {
                return 1;
            }
            if (a >= 13 && a < 26)
            {
                return 2;
            }
            if (a >= 26 && a < 39)
            {
                return 3;
            }

            if (a >= 39 && a < 52)
            {
                return 4;
            }

            return 5;
        }

        /// <summary>
        ///     得到一张牌的花色，如果是主，则返回主的花色
        /// </summary>
        /// <param name="a">牌值</param>
        /// <param name="suit">主花色</param>
        /// <param name="rank">主Rank</param>
        /// <returns>花色</returns>
        internal static int GetSuit(int a, int suit, int rank)
        {
            int firstSuit = 0;

            if (a == 53 || a == 52)
            {
                firstSuit = suit;
            }
            else if ((a%13) == rank)
            {
                firstSuit = suit;
            }
            else
            {
                firstSuit = GetSuit(a);
            }

            return firstSuit;
        }

        /// <summary>
        ///     从一堆牌中找出最大的牌，考虑主
        /// </summary>
        /// <param name="cards">一堆牌</param>
        /// <param name="trump">花色</param>
        /// <param name="rank"></param>
        /// <returns>最大的牌</returns>
        public static int GetMaxCard(List<int> cards, Suit trump, int rank)
        {
            var cp = new CurrentPoker();
            cp.Trump = trump;
            cp.Rank = rank;
            foreach (int card in cards)
            {
                cp.AddCard(card);
            }
            //cp.Sort();

            if (cp.IsMixed())
            {
                return -1;
            }

            if (cp.RedJoker > 0)
                return 53;
            if (cp.BlackJoker > 0)
                return 52;
            if (cp.MasterRank > 0)
                return rank + ((int) trump - 1)*13;

            if (cp.HeartsRankTotal > 0)
                return rank;
            if (cp.SpadesRankCount > 0)
                return rank + 13;
            if (cp.DiamondsRankTotal > 0)
                return rank + 26;
            if (cp.ClubsRankTotal > 0)
                return rank + 39;

            for (int i = 51; i > -1; i--)
            {
                if (cards.Contains(i))
                    return i;
            }

            return -1;
        }

        /// <summary>
        ///     从一堆牌中找出最大的牌，考虑主
        /// </summary>
        /// <param name="cards">一堆牌</param>
        /// <param name="trump">花色</param>
        /// <param name="rank"></param>
        /// <returns>最大的牌</returns>
        public static int GetMaxCard(ArrayList cards, Suit trump, int rank)
        {
            List<int> cardsList = cards.Cast<int>().ToList();
            return GetMaxCard(cardsList, trump, rank);
        }




        /// <summary>
        ///     比较两张牌孰大孰小
        /// </summary>
        /// <param name="a">第一张牌</param>
        /// <param name="b">第二张牌</param>
        /// <param name="suit">主花色</param>
        /// <param name="rank">主Rank</param>
        /// <param name="firstSuit">第一张牌的花色</param>
        /// <returns>如果第一张大于等于第二张牌，返回true,否则返回false</returns>
        internal static bool CompareTo(int a, int b, int suit, int rank, int firstSuit)
        {
            if ((a == -1) && (b == -1))
            {
                return true;
            }
            if ((a == -1) && (b != -1))
            {
                return false;
            }
            if ((a != -1) && (b == -1))
            {
                return true;
            }


            int suit1 = GetSuit(a, suit, rank);
            int suit2 = GetSuit(b, suit, rank);

            if ((suit1 == firstSuit) && (suit2 != firstSuit))
            {
                if (suit1 == suit)
                {
                    return true;
                }
                if (suit2 == suit)
                {
                    return false;
                }
                return true;
            }
            if ((suit1 != firstSuit) && (suit2 == firstSuit))
            {
                if (suit1 == suit)
                {
                    return true;
                }
                if (suit2 == suit)
                {
                    return false;
                }

                return false;
            }

            if (a == 53)
            {
                return true;
            }


            if (a == 52)
            {
                if (b == 53)
                {
                    return false;
                }
                return true;
            }
            if (b == 52)
            {
                if (a == 53)
                {
                    return true;
                }
                return false;
            }


            if (a == (suit - 1)*13 + rank)
            {
                if (b == 53 || b == 52)
                {
                    return false;
                }
                return true;
            }
            if (a%13 == rank)
            {
                if (b == 53 || b == 52 || (b == (suit - 1)*13 + rank))
                {
                    return false;
                }
                return true;
            }
            if (b == (suit - 1)*13 + rank)
            {
                if (a == 53 || a == 52)
                {
                    return true;
                }
                return false;
            }
            if (b%13 == rank)
            {
                if (a == 53 || a == 52 || (a == (suit - 1)*13 + rank))
                {
                    return true;
                }
                return false;
            }
            if ((suit1 == suit) && (suit2 != suit))
            {
                return true;
            }
            if ((suit1 != suit) && (suit2 == suit))
            {
                return false;
            }
            if (suit1 == suit2)
            {
                return (a - b >= 0);
            }
            return true;
        }


    }
}