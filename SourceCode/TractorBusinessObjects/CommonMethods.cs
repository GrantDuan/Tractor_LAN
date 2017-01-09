using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Duan.Xiugang.Tractor.Objects
{
    /// <summary>
    ///     ͨ�ô�����.
    ///     ������������г��õķ��������������.
    /// </summary>
    public class CommonMethods
    {

        /// <summary>
        ///     �õ�һ���ƵĻ�ɫ
        /// </summary>
        /// <param name="a">��ֵ</param>
        /// <returns>��ɫ</returns>
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
        ///     �õ�һ���ƵĻ�ɫ������������򷵻����Ļ�ɫ
        /// </summary>
        /// <param name="a">��ֵ</param>
        /// <param name="suit">����ɫ</param>
        /// <param name="rank">��Rank</param>
        /// <returns>��ɫ</returns>
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
        ///     ��һ�������ҳ������ƣ�������
        /// </summary>
        /// <param name="cards">һ����</param>
        /// <param name="trump">��ɫ</param>
        /// <param name="rank"></param>
        /// <returns>������</returns>
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
        ///     ��һ�������ҳ������ƣ�������
        /// </summary>
        /// <param name="cards">һ����</param>
        /// <param name="trump">��ɫ</param>
        /// <param name="rank"></param>
        /// <returns>������</returns>
        public static int GetMaxCard(ArrayList cards, Suit trump, int rank)
        {
            List<int> cardsList = cards.Cast<int>().ToList();
            return GetMaxCard(cardsList, trump, rank);
        }




        /// <summary>
        ///     �Ƚ������������С
        /// </summary>
        /// <param name="a">��һ����</param>
        /// <param name="b">�ڶ�����</param>
        /// <param name="suit">����ɫ</param>
        /// <param name="rank">��Rank</param>
        /// <param name="firstSuit">��һ���ƵĻ�ɫ</param>
        /// <returns>�����һ�Ŵ��ڵ��ڵڶ����ƣ�����true,���򷵻�false</returns>
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