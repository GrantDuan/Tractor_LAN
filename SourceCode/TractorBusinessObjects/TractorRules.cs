using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Duan.Xiugang.Tractor.Objects
{
    public class TractorRules
    {
        /// <summary>
        ///     判断玩家出的牌是否合法
        ///     不能检查甩牌是否成功
        /// </summary>
        /// <param name="currentTrickState">这回和出牌的状态</param>
        /// <param name="selectedCards">玩家选择的牌</param>
        /// <param name="currentCards">玩家的牌（包括选择的）</param>
        /// <returns></returns>
        public static ShowingCardsValidationResult IsValid(CurrentTrickState currentTrickState, List<int> selectedCards,
            CurrentPoker currentCards)
        {

            //玩家选择牌之后剩下的牌
            var leftCardsCp = (CurrentPoker) currentCards.Clone();
            foreach (int card in selectedCards)
            {
                leftCardsCp.RemoveCard(card);
            }

            var showingCardsCp = new CurrentPoker();
            showingCardsCp.TrumpInt = (int) currentCards.Trump;
            showingCardsCp.Rank = currentCards.Rank;
            foreach (int showingCard in selectedCards)
            {
                showingCardsCp.AddCard(showingCard);
            }
            //showingCardsCp.Sort();

            var leadingCardsCp = new CurrentPoker();
            leadingCardsCp.TrumpInt = (int) currentCards.Trump;
            leadingCardsCp.Rank = currentCards.Rank;
            foreach (int card in currentTrickState.LeadingCards)
            {
                leadingCardsCp.AddCard(card);
            }
            // leadingCardsCp.Sort();

            //the first player to show cards
            if (!currentTrickState.IsStarted())
            {
                if (showingCardsCp.Count > 0 && !showingCardsCp.IsMixed())
                {
                    if (selectedCards.Count == 1) //如果是单张牌
                    {
                        return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
                    }
                    if (selectedCards.Count == 2 && (showingCardsCp.GetPairs().Count == 1)) //如果是一对
                    {
                        return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
                    }
                    if ((showingCardsCp.GetTractorOfAnySuit().Count > 1) &&
                        selectedCards.Count == showingCardsCp.GetTractorOfAnySuit().Count*2) //如果是拖拉机
                    {
                        return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
                    }
                    return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.TryToDump};
                }
            }

            //牌的数量
            if (currentTrickState.LeadingCards.Count != selectedCards.Count)
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Invalid};

            //得到第一个家伙出的花色
            Suit leadingSuit = currentTrickState.LeadingSuit;
            bool isTrump = PokerHelper.IsTrump(currentTrickState.LeadingCards[0], currentCards.Trump, currentCards.Rank);
            if (isTrump)
                leadingSuit = currentCards.Trump;

            //如果出的牌混合的，则判断手中是否还剩出的花色，如果剩,false;如果不剩;true
            if (showingCardsCp.IsMixed())
            {
                if (leftCardsCp.HasSomeCards((int) leadingSuit))
                {
                    return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Invalid};
                }
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
            }

            //出的牌的花色
            Suit mysuit = PokerHelper.GetSuit(selectedCards[0]);
            isTrump = PokerHelper.IsTrump(selectedCards[0], currentCards.Trump, currentCards.Rank);
            if (isTrump)
                mysuit = currentCards.Trump;


            //花色是否一致
            if (mysuit != leadingSuit)
            {
                //而且确实没有此花色
                if (leftCardsCp.HasSomeCards((int) leadingSuit))
                {
                    return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Invalid};
                }
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
            }

            //别人如果出对，我也应该出对
            int leadingCardsPairs = leadingCardsCp.GetPairs().Count;
            int selectedCardsPairs = showingCardsCp.GetPairs().Count;
            int holdingCardsPairs = currentCards.GetPairs((int) leadingSuit).Count;


            //2.如果别人出拖拉机，我如果有，也应该出拖拉机
            if (leadingCardsCp.HasTractors())
            {
                if ((!showingCardsCp.HasTractors()) && (currentCards.GetTractor((int) leadingSuit) > -1))
                {
                    return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Invalid};
                }
                if ((selectedCardsPairs < leadingCardsPairs) && (holdingCardsPairs > selectedCardsPairs))
                    //出的对比第一个玩家少，而且没有出所有的对
                {
                    return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Invalid};
                }
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
            }


            if (leadingCardsPairs > 0)
            {
                //如果对出的不够多，而且没有出所有的对
                if ((holdingCardsPairs > selectedCardsPairs) && (selectedCardsPairs < leadingCardsPairs))
                {
                    return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Invalid};
                }
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
            }


            return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
        }

        /// <summary>
        ///     check if the player's selected cards valid
        ///     检查出牌或者甩牌合理
        /// </summary>
        /// <param name="playerHoldingCards">所有玩家当前手里的牌</param>
        /// <param name="selectedCards"></param>
        /// <param name="player">出牌的玩家</param>
        /// <returns></returns>
        public static ShowingCardsValidationResult IsLeadingCardsValid(
            Dictionary<string, CurrentPoker> playerHoldingCards, List<int> selectedCards, string player)
        {
            var mustShowCardsForDumpingFail = new List<int>();
            CurrentPoker holdingCardsCp = playerHoldingCards.Values.FirstOrDefault();
            if (holdingCardsCp == null)
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Unknown};
            var selectedCardsCp = new CurrentPoker(selectedCards, holdingCardsCp.TrumpInt, holdingCardsCp.Rank);
            int selectedCardSuit = CommonMethods.GetSuit(selectedCards[0], selectedCardsCp.TrumpInt,
                selectedCardsCp.Rank);
            int trump = selectedCardsCp.TrumpInt;
            int rank = selectedCardsCp.Rank;

            List<int> tractor = selectedCardsCp.GetTractorOfAnySuit();

            if (selectedCards.Count == 1) //如果是单张牌
            {
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
            }
            if (selectedCards.Count == 2 && (selectedCardsCp.GetPairs().Count == 1)) //如果是一对
            {
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
            }
            if (tractor.Count > 1 && selectedCards.Count == tractor.Count*2) //如果是拖拉机
            {
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Valid};
            }
            if (selectedCardsCp.IsMixed())
            {
                return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.Invalid};
            }

            if (tractor.Count > 1)
            {
                int myMax = tractor[0];
                foreach (int card in tractor)
                {
                    selectedCardsCp.RemoveCard(card);
                    selectedCardsCp.RemoveCard(card);
                }

                foreach (var keyValue in playerHoldingCards)
                {
                    if (keyValue.Key != player)
                    {
                        List<int> tractor1 = keyValue.Value.GetTractor((Suit) selectedCardSuit);
                        if (tractor1.Count < tractor.Count)
                            continue;
                        int max = tractor1[0];
                        if (!CommonMethods.CompareTo(myMax, max, trump, rank, selectedCardSuit))
                        {
                            foreach (int card in tractor)
                            {
                                mustShowCardsForDumpingFail.Add(card);
                                mustShowCardsForDumpingFail.Add(card);
                            }
                            return new ShowingCardsValidationResult
                            {
                                ResultType = ShowingCardsValidationResultType.DumpingFail,
                                MustShowCardsForDumpingFail = mustShowCardsForDumpingFail,
                                CardsToShow = selectedCards
                            };
                        }
                    }
                }
            }

            if (selectedCardsCp.GetPairs().Count > 0)
            {
                ArrayList list0 = selectedCardsCp.GetPairs();

                for (int i = 0; i < list0.Count; i++)
                {
                    var myMax = (int) list0[i];
                    selectedCardsCp.RemoveCard(myMax);
                    selectedCardsCp.RemoveCard(myMax);

                    foreach (var keyValue in playerHoldingCards)
                    {
                        if (keyValue.Key != player)
                        {
                            ArrayList list = keyValue.Value.GetPairs(selectedCardSuit);
                            if (list.Count == 0)
                                continue;
                            var max = (int) list[list.Count - 1];

                            if (!CommonMethods.CompareTo(myMax, max, trump, rank, selectedCardSuit) && max > -1)
                            {
                                mustShowCardsForDumpingFail.Add(myMax);
                                mustShowCardsForDumpingFail.Add(myMax);
                                return new ShowingCardsValidationResult
                                {
                                    ResultType = ShowingCardsValidationResultType.DumpingFail,
                                    MustShowCardsForDumpingFail = mustShowCardsForDumpingFail,
                                    CardsToShow = selectedCards
                                };
                            }
                        }
                    }
                }
            }

            int[] cards = selectedCardsCp.GetCards();
            foreach (var keyValue in playerHoldingCards)
            {
                if (keyValue.Key != player)
                {
                    int max = keyValue.Value.GetMaxCard(selectedCardSuit);
                    for (int i = 0; i < 54; i++)
                    {
                        if (cards[i] == 1)
                        {
                            if (!CommonMethods.CompareTo(i, max, trump, rank, selectedCardSuit))
                            {
                                mustShowCardsForDumpingFail.Add(i);
                                return new ShowingCardsValidationResult
                                {
                                    ResultType = ShowingCardsValidationResultType.DumpingFail,
                                    MustShowCardsForDumpingFail = mustShowCardsForDumpingFail,
                                    CardsToShow = selectedCards
                                };
                            }
                        }
                    }
                }
            }
            return new ShowingCardsValidationResult {ResultType = ShowingCardsValidationResultType.DumpingSuccess};
        }

        //确定下一次该谁出牌
        public static string GetWinner(CurrentTrickState trickState)
        {
            var cp = new CurrentPoker[4];
            Suit trump = trickState.Trump;
            var trumpInt = (int) trickState.Trump;
            int rank = trickState.Rank;
            cp[0] = new CurrentPoker(trickState.ShowedCards[trickState.Learder], trumpInt, rank);
            string nextPlayer1 = trickState.NextPlayer(trickState.Learder);
            cp[1] = new CurrentPoker(trickState.ShowedCards[nextPlayer1], trumpInt, rank);
            string nextPlayer2 = trickState.NextPlayer(nextPlayer1);
            cp[2] = new CurrentPoker(trickState.ShowedCards[nextPlayer2], trumpInt, rank);
            string nextPlayer3 = trickState.NextPlayer(nextPlayer2);
            cp[3] = new CurrentPoker(trickState.ShowedCards[nextPlayer3], trumpInt, rank);
            //cp[0].Sort();
            //cp[1].Sort();
            //cp[2].Sort();
            //cp[3].Sort();

            int leadingCardsCount = trickState.ShowedCards.Values.ToArray()[0].Count;
            int winderNumber = 0;
            var leadingSuit = (int) trickState.LeadingSuit;


            List<int> leadingTractor = cp[0].GetTractorOfAnySuit();
            //甩牌 拖拉机
            if ((leadingTractor.Count > 1) && (cp[0].Count > leadingTractor.Count*2)) //甩拖拉机
            {
                int maxCard = leadingTractor[0];
                List<int> tractor1 = cp[1].GetTractor(trump);
                if (tractor1.Count >= leadingTractor.Count &&
                    (!cp[1].IsMixed() && cp[1].GetPairs().Count >= cp[0].GetPairs().Count))
                {
                    int tmpMax = tractor1[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 1;
                        maxCard = tmpMax;
                    }
                }
                List<int> tractor2 = cp[2].GetTractor(trump);
                if (tractor2.Count >= leadingTractor.Count &&
                    (!cp[2].IsMixed() && cp[2].GetPairs().Count >= cp[0].GetPairs().Count))
                {
                    int tmpMax = tractor2[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 2;
                        maxCard = tmpMax;
                    }
                }
                List<int> tractor3 = cp[2].GetTractor(trump);
                if (tractor3.Count >= leadingTractor.Count &&
                    (!cp[3].IsMixed() && cp[3].GetPairs().Count >= cp[0].GetPairs().Count))
                {
                    int tmpMax = tractor3[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 3;
                    }
                }
            }
                //甩牌 对
            else if ((2 < leadingCardsCount) && (cp[0].GetPairs().Count > 0) && leadingTractor.Count < 2)
            {
                int maxCard = CommonMethods.GetMaxCard(cp[0].GetPairs(), trump, rank);
                if (cp[1].GetPairs().Count >= cp[0].GetPairs().Count && (!cp[1].IsMixed()))
                {
                    int tmpMax = CommonMethods.GetMaxCard(cp[1].GetPairs(), trump, rank);
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 1;
                        maxCard = tmpMax;
                    }
                }
                if (cp[2].GetPairs().Count >= cp[0].GetPairs().Count && (!cp[2].IsMixed()))
                {
                    int tmpMax = CommonMethods.GetMaxCard(cp[2].GetPairs(), trump, rank);
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 2;
                        maxCard = tmpMax;
                    }
                }
                if (cp[3].GetPairs().Count >= cp[0].GetPairs().Count && (!cp[3].IsMixed()))
                {
                    int tmpMax = CommonMethods.GetMaxCard(cp[3].GetPairs(), trump, rank);
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 3;
                    }
                }
            }
                //甩多个单张牌
            else if ((leadingCardsCount > 1) && (cp[0].GetPairs().Count == 0))
            {
                int maxCard = CommonMethods.GetMaxCard(trickState.ShowedCards[trickState.Learder], trump, rank);
                int tmpMax = CommonMethods.GetMaxCard(trickState.ShowedCards[nextPlayer1], trump, rank);

                if (cp[1].GetSuitCardsWithJokerAndRank(trumpInt).Count() ==
                    trickState.ShowedCards[trickState.Learder].Count)
                {
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 1;
                        maxCard = tmpMax;
                    }
                }

                if (cp[2].GetSuitCardsWithJokerAndRank(trumpInt).Count() ==
                    trickState.ShowedCards[trickState.Learder].Count)
                {
                    tmpMax = CommonMethods.GetMaxCard(trickState.ShowedCards[nextPlayer2], trump, rank);
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 2;
                        maxCard = tmpMax;
                    }
                }
                if (cp[3].GetSuitCardsWithJokerAndRank(trumpInt).Count() ==
                    trickState.ShowedCards[trickState.Learder].Count)
                {
                    tmpMax = CommonMethods.GetMaxCard(trickState.ShowedCards[nextPlayer3], trump, rank);
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 3;
                    }
                }
            }
                //拖拉机
            else if (leadingTractor.Count > 1)
            {
                //如果有拖拉机
                List<int> tractor0 = cp[0].GetTractorOfAnySuit();
                List<int> tractor1 = cp[1].GetTractorOfAnySuit();
                List<int> tractor2 = cp[2].GetTractorOfAnySuit();
                List<int> tractor3 = cp[3].GetTractorOfAnySuit();
                int maxCard = tractor0[0];
                if (tractor1.Count >= tractor0.Count)
                {
                    int tmpMax = tractor1[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 1;
                        maxCard = tmpMax;
                    }
                }
                if (tractor2.Count >= tractor0.Count)
                {
                    int tmpMax = tractor2[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 2;
                        maxCard = tmpMax;
                    }
                }
                if (tractor3.Count >= tractor0.Count)
                {
                    int tmpMax = tractor3[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 3;
                    }
                }
            }

            else if (cp[0].GetPairs().Count == 1 && (leadingCardsCount == 2)) //如果有一个对
            {
                var maxCard = (int) cp[0].GetPairs()[0];
                if (cp[1].GetPairs().Count > 0)
                {
                    var tmpMax = (int) cp[1].GetPairs()[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 1;
                        maxCard = tmpMax;
                    }
                }
                if (cp[2].GetPairs().Count > 0)
                {
                    var tmpMax = (int) cp[2].GetPairs()[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 2;
                        maxCard = tmpMax;
                    }
                }
                if (cp[3].GetPairs().Count > 0)
                {
                    var tmpMax = (int) cp[3].GetPairs()[0];
                    if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                    {
                        winderNumber = 3;
                    }
                }
            }
            else if (leadingCardsCount == 1) //如果是单张牌
            {
                int maxCard = trickState.ShowedCards[trickState.Learder][0];
                int tmpMax = trickState.ShowedCards[nextPlayer1][0];
                if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                {
                    winderNumber = 1;
                    maxCard = tmpMax;
                }

                tmpMax = trickState.ShowedCards[nextPlayer2][0];
                if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                {
                    winderNumber = 2;
                    maxCard = tmpMax;
                }
                tmpMax = trickState.ShowedCards[nextPlayer3][0];
                if (!CommonMethods.CompareTo(maxCard, tmpMax, trumpInt, rank, leadingSuit))
                {
                    winderNumber = 3;
                }
            }

            string winner = "";
            switch (winderNumber)
            {
                case 0:
                    winner = trickState.Learder;
                    break;

                case 1:
                    winner = nextPlayer1;
                    break;
                case 2:
                    winner = nextPlayer2;
                    break;
                case 3:
                    winner = nextPlayer3;
                    break;
            }

            return winner;
        }

        public static int GetCardNumberofEachPlayer(int playerCount)
        {
            if (playerCount == 4)
                return 25;
            if (playerCount == 5)
                return 20;

            return 25;
        }
    }

    [DataContract]
    public class ShowingCardsValidationResult
    {
        [DataMember] public List<int> CardsToShow;
        [DataMember] public List<int> MustShowCardsForDumpingFail;
        [DataMember] public string PlayerId;
        [DataMember] public ShowingCardsValidationResultType ResultType;

        public ShowingCardsValidationResult()
        {
            MustShowCardsForDumpingFail = new List<int>();
            CardsToShow = new List<int>();
        }
    }

    public enum ShowingCardsValidationResultType
    {
        Unknown,
        Invalid,
        Valid,
        TryToDump,
        DumpingFail,
        DumpingSuccess,
    }
}