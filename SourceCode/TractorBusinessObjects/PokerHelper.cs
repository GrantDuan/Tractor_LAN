namespace Duan.Xiugang.Tractor.Objects
{
    public class PokerHelper
    {
        /// <summary>
        ///     得到一个牌的花色
        /// </summary>
        /// <param name="cardNumber">牌值</param>
        /// <returns>花色</returns>
        internal static Suit GetSuit(int cardNumber)
        {
            if (cardNumber >= 0 && cardNumber < 13)
            {
                return Suit.Heart;
            }
            if (cardNumber >= 13 && cardNumber < 26)
            {
                return Suit.Spade;
            }
            if (cardNumber >= 26 && cardNumber < 39)
            {
                return Suit.Diamond;
            }

            if (cardNumber >= 39 && cardNumber < 52)
            {
                return Suit.Club;
            }

            return Suit.Joker;
        }

        internal static bool IsTrump(int cardNumber, Suit trump, int rank)
        {
            bool result;

            if (cardNumber == 53 || cardNumber == 52)
            {
                result = true;
            }
            else if ((cardNumber%13) == rank)
            {
                result = true;
            }
            else
            {
                Suit suit = GetSuit(cardNumber);
                if (suit == trump)
                    result = true;
                else
                    result = false;
            }

            return result;
        }
    }
}