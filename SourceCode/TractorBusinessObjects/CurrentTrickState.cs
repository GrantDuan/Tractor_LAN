using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Duan.Xiugang.Tractor.Objects
{
    //一个回合牌的状态
    [DataContract]
    public class CurrentTrickState
    {
        //第一个出牌的玩家
        public CurrentTrickState()
        {
            ShowedCards = new Dictionary<string, List<int>>();
        }

        public CurrentTrickState(List<string> playeIds)
        {
            ShowedCards = new Dictionary<string, List<int>>();
            foreach (string playeId in playeIds)
            {
                ShowedCards.Add(playeId, new List<int>());
            }
        }

        [DataMember]
        public string Learder { get; set; }

        [DataMember]
        public string Winner { get; set; }

        //player ID and cards
        [DataMember]
        public Dictionary<string, List<int>> ShowedCards { get; set; }

        [DataMember]
        public Suit Trump { get; set; }

        [DataMember]
        public int Rank { get; set; }

        public List<int> LeadingCards
        {
            get
            {
                if (IsStarted())
                {
                    return ShowedCards[Learder];
                }
                return new List<int>();
            }
        }

        public Suit LeadingSuit
        {
            get
            {
                if (IsStarted())
                    return PokerHelper.GetSuit(LeadingCards[0]);
                return Suit.None;
            }
        }

        public int Points
        {
            get
            {
                int points = 0;
                foreach (var cardsList in ShowedCards.Values)
                {
                    foreach (int card in cardsList)
                    {
                        if (card%13 == 3)
                            points += 5;
                        else if (card%13 == 8)
                            points += 10;
                        else if (card%13 == 11)
                            points += 10;
                    }
                }
                return points;
            }
        }

        /// <summary>
        ///     get the next player who should show card
        /// </summary>
        /// <returns>return "" if all player has showed cards</returns>
        public string NextPlayer()
        {
            string playerId = "";
            if (ShowedCards[Learder].Count == 0)
                playerId = Learder;

            else
            {
                bool afterLeader = false;
                //find next player to show card after learder
                foreach (var showedCard in ShowedCards)
                {
                    if (showedCard.Key != Learder && afterLeader == false)
                        continue;
                    if (showedCard.Key == Learder) // search from learder
                    {
                        afterLeader = true;
                    }
                    if (afterLeader)
                    {
                        if (showedCard.Value.Count == 0)
                        {
                            playerId = showedCard.Key;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(playerId))
                {
                    foreach (var showedCard in ShowedCards)
                    {
                        if (showedCard.Key != Learder)
                        {
                            if (showedCard.Value.Count == 0)
                            {
                                playerId = showedCard.Key;
                                break;
                            }
                        }
                        else //search end before leader;
                            break;
                    }
                }
            }

            return playerId;
        }


        /// <summary>
        ///     get the next player after a player
        /// </summary>
        /// <returns>return "" if all player has showed cards</returns>
        public string NextPlayer(string playerId)
        {
            string nextPlayer = "";
            if (!ShowedCards.Keys.Contains(playerId))
                return "";


            bool afterLeader = false;
            //find next player to show card after learder
            foreach (var showedCard in ShowedCards)
            {
                if (showedCard.Key != playerId && afterLeader == false)
                    continue;
                else if (showedCard.Key == playerId) // search from learder
                {
                    afterLeader = true;
                }
                else if (afterLeader)
                {
                    nextPlayer = showedCard.Key;
                    break;
                }
            }


            if (string.IsNullOrEmpty(nextPlayer))
            {
                foreach (var showedCard in ShowedCards)
                {
                    if (showedCard.Key != playerId)
                    {
                        nextPlayer = showedCard.Key;
                    }
                    break;
                }
            }


            return nextPlayer;
        }

        /// <summary>
        ///     get the latest player who just showed cards
        /// </summary>
        /// <returns>return "" if no player has showed cards</returns>
        public string LatestPlayerShowedCard()
        {
            string playerId = "";
            if (ShowedCards[Learder].Count == 0)
                return playerId;

            bool afterLeader = false;
            //find next player to show card after learder
            foreach (var showedCard in ShowedCards)
            {
                if (showedCard.Key != Learder && afterLeader == false)
                    continue;
                else if (showedCard.Key == Learder) //search from leader;
                {
                    playerId = Learder;
                    afterLeader = true;
                }
                else if (afterLeader)
                {
                    if (showedCard.Value.Count == 0)
                        return playerId;
                    playerId = showedCard.Key;
                }
            }


            foreach (var showedCard in ShowedCards)
            {
                if (showedCard.Key != Learder)
                {
                    if (showedCard.Value.Count == 0)
                        return playerId;
                    playerId = showedCard.Key;
                }
                else //search end before leader
                    break;
            }

            return playerId;
        }

        public bool AllPlayedShowedCards()
        {
            foreach (var cards in ShowedCards.Values)
            {
                if (cards.Count == 0)
                    return false;
            }
            return true;
        }

        public bool IsStarted()
        {
            if (string.IsNullOrEmpty(Learder))
                return false;
            if (ShowedCards.Count == 0)
                return false;
            return ShowedCards[Learder].Count > 0;
        }

        public int CountOfPlayerShowedCards()
        {
            int result = 0;
            foreach (var showedCard in ShowedCards)
            {
                if (showedCard.Value.Count > 0)
                    result++;
            }
            return result;
        }
    }
}