using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Duan.Xiugang.Tractor.Objects;
using System.Threading;
using System.Text;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TractorServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class TractorHost : ITractorHost
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal GameState CurrentGameState;
        internal CurrentHandState CurrentHandState;
        internal CurrentTrickState CurrentTrickState;
        public CardsShoe CardsShoe { get; set; }

        public Dictionary<string, IPlayer> PlayersProxy { get; set; }

        public TractorHost()
        {
            CurrentGameState = new GameState();
            CurrentHandState = new CurrentHandState(this.CurrentGameState);
            CardsShoe = new CardsShoe();
            PlayersProxy = new Dictionary<string, IPlayer>();
        }

        #region implement interface ITractorHost

        public void PlayerIsReady(string playerID)
        {
            if (!PlayersProxy.Keys.Contains(playerID))
            {
                IPlayer player = OperationContext.Current.GetCallbackChannel<IPlayer>();
                CurrentGameState.Players.Add(new PlayerEntity { PlayerId = playerID, Rank = 0, Team = GameTeam.None });
                PlayersProxy.Add(playerID, player);
                log.Debug(string.Format("player {0} joined.", playerID));
                if (PlayersProxy.Count < 4)
                    UpdateGameState();
                if (PlayersProxy.Count == 4)
                {
                    //create team
                    CurrentGameState.Players[0].Team = GameTeam.VerticalTeam;
                    CurrentGameState.Players[2].Team = GameTeam.VerticalTeam;
                    CurrentGameState.Players[1].Team = GameTeam.HorizonTeam;
                    CurrentGameState.Players[3].Team = GameTeam.HorizonTeam;
                    UpdateGameState();

                    RestartGame();
                }
            }
            else
            {
                if (PlayersProxy.Count == 4)
                {
                    if (this.CurrentHandState.IsFirstHand)
                        RestartGame();
                    else
                        RestartCurrentHand();
                }
            }
        }

        //玩家推出
        public void PlayerQuit(string playerId)
        {
            log.Debug(playerId + " quit.");
            var quitPlayer = CurrentGameState.Players.SingleOrDefault(p => p.PlayerId == playerId);
            if (quitPlayer != null)
                CurrentGameState.Players.Remove(quitPlayer);

            PlayersProxy.Remove(playerId);
            foreach (var player in PlayersProxy.Values)
            {
                player.StartGame();
            }
        }

        //player discard last 8 cards
        public void StoreDiscardedCards(int[] cards)
        {
            this.CurrentHandState.DiscardedCards = cards;
            this.CurrentHandState.CurrentHandStep = HandStep.DiscardingLast8CardsFinished;
            foreach (var card in cards)
            {
                this.CurrentHandState.PlayerHoldingCards[this.CurrentHandState.Last8Holder].RemoveCard(card);
            }
            var logMsg =  "player " + this.CurrentHandState.Last8Holder+ " discard 8 cards: ";
            foreach (var card in cards)
            {
                logMsg += card.ToString() + ", ";
            }
            log.Debug(logMsg);

            log.Debug(this.CurrentHandState.Last8Holder + "'s cards after discard 8 cards: " + this.CurrentHandState.PlayerHoldingCards[this.CurrentHandState.Last8Holder].ToString());

            UpdatePlayersCurrentHandState();

            //等待5秒，让玩家反底
            var trump = CurrentHandState.Trump;
            Thread.Sleep(5000);
            if (trump == CurrentHandState.Trump) //没有玩家反
            {
                BeginNewTrick(this.CurrentHandState.Starter);
                this.CurrentHandState.CurrentHandStep = HandStep.Playing;
                UpdatePlayersCurrentHandState();
            }
        }


        //亮主
        public void PlayerMakeTrump(Duan.Xiugang.Tractor.Objects.TrumpExposingPoker trumpExposingPoker, Duan.Xiugang.Tractor.Objects.Suit trump, string trumpMaker)
        {
            lock (CurrentHandState)
            {
                //invalid user;
                if (PlayersProxy[trumpMaker] == null)
                    return;
                if (trumpExposingPoker > this.CurrentHandState.TrumpExposingPoker)
                {
                    this.CurrentHandState.TrumpExposingPoker = trumpExposingPoker;
                    this.CurrentHandState.TrumpMaker = trumpMaker;
                    this.CurrentHandState.Trump = trump;
                    if (this.CurrentHandState.IsFirstHand && this.CurrentHandState.CurrentHandStep < HandStep.DistributingLast8Cards)
                        this.CurrentHandState.Starter = trumpMaker;
                    //反底
                    if (this.CurrentHandState.CurrentHandStep == HandStep.DiscardingLast8CardsFinished)
                    {
                        this.CurrentHandState.Last8Holder = trumpMaker;
                        this.CurrentHandState.CurrentHandStep = HandStep.Last8CardsRobbed;
                        DistributeLast8Cards();
                    }
                    UpdatePlayersCurrentHandState();
                }
            }
        }



        public void PlayerShowCards(CurrentTrickState currentTrickState)
        {
            string lastestPlayer = currentTrickState.LatestPlayerShowedCard();
            if (PlayersProxy[lastestPlayer] != null)
            {
                this.CurrentTrickState.ShowedCards[lastestPlayer] = currentTrickState.ShowedCards[lastestPlayer];
                string cardsString = "";
                foreach(var card in this.CurrentTrickState.ShowedCards[lastestPlayer])
                {
                    cardsString += card.ToString() + " ";
                }
                log.Debug("Player " + lastestPlayer + " showed cards: " + cardsString);
                //更新每个用户手中的牌在SERVER
                foreach (int card in this.CurrentTrickState.ShowedCards[lastestPlayer])
                {
                    this.CurrentHandState.PlayerHoldingCards[lastestPlayer].RemoveCard(card);
                }
                //回合结束
                if (this.CurrentTrickState.AllPlayedShowedCards())
                {
                    this.CurrentTrickState.Winner = TractorRules.GetWinner(this.CurrentTrickState);
                    if (!string.IsNullOrEmpty(this.CurrentTrickState.Winner))
                    {
                        if (
                            !this.CurrentGameState.ArePlayersInSameTeam(CurrentHandState.Starter,
                                                                        this.CurrentTrickState.Winner))
                        {
                            CurrentHandState.Score += currentTrickState.Points;
                            UpdatePlayersCurrentHandState();
                        }


                       log.Debug("Winner: " + this.CurrentTrickState.Winner);

                    }

                    UpdatePlayerCurrentTrickState();

                    CurrentHandState.LeftCardsCount -= currentTrickState.ShowedCards[lastestPlayer].Count;

                    //开始新的回合
                    if (this.CurrentHandState.LeftCardsCount > 0)
                    {
                        BeginNewTrick(this.CurrentTrickState.Winner);
                    }
                    else //所有牌都出完了
                    {
                        //扣底
                        CalculatePointsFromDiscarded8Cards();
                        Thread.Sleep(2000);
                        this.CurrentHandState.CurrentHandStep = HandStep.Ending;
                        UpdatePlayersCurrentHandState();

                        Thread.Sleep(5000);
                        var starter = this.CurrentGameState.NextRank(this.CurrentHandState, this.CurrentTrickState);

                        Thread.Sleep(5000);
                        StartNextHand(starter);

                    }
                }
                else
                    UpdatePlayerCurrentTrickState();

            }
        }

        public ShowingCardsValidationResult ValidateDumpingCards(List<int> selectedCards, string playerId)
        {
            var result = TractorRules.IsLeadingCardsValid(this.CurrentHandState.PlayerHoldingCards, selectedCards,
                                                          playerId);
            result.PlayerId = playerId;

            if (result.ResultType == ShowingCardsValidationResultType.DumpingFail)
            {
                foreach (var player in PlayersProxy)
                {
                    if (player.Key != playerId)
                    {
                        player.Value.NotifyDumpingValidationResult(result);
                    }
                }
            }
            var cardString = "";
            foreach(var card in selectedCards)
            {
                cardString += card.ToString() + " ";
            }
            log.Debug(playerId + " tried to dump cards: " + cardString + " Result: " + result.ResultType.ToString());
            return result;
        }
        #endregion

        #region Host Action
        public void RestartGame()
        {
            log.Debug("restart game");
            foreach (var player in CurrentGameState.Players)
            {
                player.Rank = 0;
            }

            this.CurrentHandState = new CurrentHandState(this.CurrentGameState);
            this.CurrentHandState.LeftCardsCount = TractorRules.GetCardNumberofEachPlayer(this.CurrentGameState.Players.Count);
            CurrentHandState.IsFirstHand = true;
            UpdatePlayersCurrentHandState();
            var currentHandId = this.CurrentHandState.Id;
            DistributeCards();
            if (this.CurrentHandState.Id != currentHandId)
                return;
            Thread.Sleep(5000);
            if (this.CurrentHandState.Trump != Suit.None)
                DistributeLast8Cards();
            else if (PlayersProxy.Count == 4)
                RestartGame();
        }

        public void RestartCurrentHand()
        {
            log.Debug("restart current hand, starter: " + this.CurrentHandState.Starter + " Rank: " + this.CurrentHandState.Rank.ToString());
            StartNextHand(this.CurrentGameState.Players.Single(p => p.PlayerId == this.CurrentHandState.Starter));
        }

        public void StartNextHand(PlayerEntity nextStarter)
        {
            UpdateGameState();
            this.CurrentHandState = new CurrentHandState(this.CurrentGameState);
            this.CurrentHandState.Starter = nextStarter.PlayerId;
            this.CurrentHandState.Rank = nextStarter.Rank;
            this.CurrentHandState.LeftCardsCount = TractorRules.GetCardNumberofEachPlayer(this.CurrentGameState.Players.Count);

            log.Debug("start next hand, starter: " + this.CurrentHandState.Starter + " Rank: " + this.CurrentHandState.Rank.ToString());

            UpdatePlayersCurrentHandState();

            var currentHandId = this.CurrentHandState.Id;

            DistributeCards();
            if (this.CurrentHandState.Id != currentHandId)
                return;

            Thread.Sleep(5000);
            if (this.CurrentHandState.Trump != Suit.None)
                DistributeLast8Cards();
            
            else if (PlayersProxy.Count == 4)
            {
                //如果庄家TEAM亮不起，则庄家的下家成为新的庄家
                var nextStarter2 = CurrentGameState.GetNextPlayerAfterThePlayer(false, CurrentHandState.Starter);
                this.CurrentHandState.Starter = nextStarter2.PlayerId;
                this.CurrentHandState.Rank = nextStarter2.Rank;
                log.Debug("starter team fail to make trump, next starter: " + this.CurrentHandState.Starter + " Rank: " + this.CurrentHandState.Rank.ToString());

                UpdatePlayersCurrentHandState();

                //10 seconds to make trump
                Thread.Sleep(10000);
                if (this.CurrentHandState.Trump != Suit.None)
                    DistributeLast8Cards();
                else if (PlayersProxy.Count == 4)
                {
                    //如果下家也亮不起，重新发牌
                    StartNextHand(nextStarter);
                }
            }
        }

        //发牌
        public void DistributeCards()
        {
            CurrentHandState.CurrentHandStep = HandStep.DistributingCards;
            UpdatePlayersCurrentHandState();
            string currentHandId = this.CurrentHandState.Id;
            this.CardsShoe.Shuffle();
            int cardNumberofEachPlayer = TractorRules.GetCardNumberofEachPlayer(this.CurrentGameState.Players.Count);
            int j = 0;

            var LogList = new Dictionary<string, StringBuilder>();
            foreach (var player in PlayersProxy)
            {
                LogList[player.Key] = new StringBuilder();
            }

            for (int i = 0; i < cardNumberofEachPlayer; i++)
            {
                foreach (var player in PlayersProxy)
                {
                    var index = j++;
                    if (this.CurrentHandState.Id == currentHandId)
                    {
                        player.Value.GetDistributedCard(CardsShoe.Cards[index]);
                        LogList[player.Key].Append(CardsShoe.Cards[index].ToString() + ", ");
                    }
                    else
                        return;

                    this.CurrentHandState.PlayerHoldingCards[player.Key].AddCard(CardsShoe.Cards[index]);
                }
                Thread.Sleep(500);
            }

            log.Debug("distribute cards to each player: ");
            foreach (var logItem in LogList)
            {                
                log.Debug(logItem.Key + ": " + logItem.Value.ToString());
            }

            foreach (var keyvalue in this.CurrentHandState.PlayerHoldingCards)
            {
                log.Debug(keyvalue.Key + "'s cards:  " + keyvalue.Value.ToString());
            }

            CurrentHandState.CurrentHandStep = HandStep.DistributingCardsFinished;
            UpdatePlayersCurrentHandState();
        }

        //发底牌
        public void DistributeLast8Cards()
        {
            var last8Cards = new int[8];
            if (CurrentHandState.CurrentHandStep == HandStep.DistributingCardsFinished)
            {
                for (int i = 0; i < 8; i++)
                {
                    last8Cards[i] = CardsShoe.Cards[CardsShoe.Cards.Length - i - 1];
                }
            }
            else if (CurrentHandState.CurrentHandStep == HandStep.Last8CardsRobbed)
            {
                last8Cards = CurrentHandState.DiscardedCards;
            }

            CurrentHandState.CurrentHandStep = HandStep.DistributingLast8Cards;
            UpdatePlayersCurrentHandState();


            IPlayer last8Holder = PlayersProxy.SingleOrDefault(p => p.Key == CurrentHandState.Last8Holder).Value;
            if (last8Holder != null)
            {
                for (int i = 0; i < 8; i++)
                {
                    var card = last8Cards[i];
                    last8Holder.GetDistributedCard(card);
                    this.CurrentHandState.PlayerHoldingCards[CurrentHandState.Last8Holder].AddCard(card);
                }
            }

            var logMsg = "distribute last 8 cards to " + CurrentHandState.Last8Holder + ", cards: ";
            foreach (var card in last8Cards)
            {
                logMsg += card.ToString() + ", ";
            }
            log.Debug(logMsg);

            CurrentHandState.CurrentHandStep = HandStep.DistributingLast8CardsFinished;
            UpdatePlayersCurrentHandState();

            Thread.Sleep(100);
            CurrentHandState.CurrentHandStep = HandStep.DiscardingLast8Cards;
            UpdatePlayersCurrentHandState();
        }

        //begin new trick
        private void BeginNewTrick(string leader)
        {
            this.CurrentTrickState = new CurrentTrickState(this.PlayersProxy.Keys.ToList());
            this.CurrentTrickState.Learder = leader;
            this.CurrentTrickState.Trump = CurrentHandState.Trump;
            this.CurrentTrickState.Rank = CurrentHandState.Rank;
            UpdatePlayerCurrentTrickState();
        }

        //计算被扣底牌的分数，然后加到CurrentHandState.Score
        private void CalculatePointsFromDiscarded8Cards()
        {
            if (!this.CurrentTrickState.AllPlayedShowedCards())
                return;
            if (this.CurrentHandState.LeftCardsCount > 0)
                return;

            var cards = this.CurrentTrickState.ShowedCards[this.CurrentTrickState.Winner];
            var cardscp = new CurrentPoker(cards, (int)this.CurrentHandState.Trump,
                                           this.CurrentHandState.Rank);

            //最后一把牌的赢家不跟庄家一伙
            if (!this.CurrentGameState.ArePlayersInSameTeam(this.CurrentHandState.Starter,
                                                    this.CurrentTrickState.Winner))
            {

                var points = 0;
                foreach (var card in this.CurrentHandState.DiscardedCards)
                {
                    if (card % 13 == 3)
                        points += 5;
                    else if (card % 13 == 8)
                        points += 10;
                    else if (card % 13 == 11)
                        points += 10;
                }

                if (cardscp.HasTractors())
                {
                    points *= 8;
                }
                else if (cardscp.GetPairs().Count > 0)
                {
                    points *= 4;
                }
                else
                {
                    points *= 2;
                }

                this.CurrentHandState.Score += points;


            }
        }
        #endregion

        #region Update Client State
        public void UpdateGameState()
        {
            foreach (IPlayer player in PlayersProxy.Values)
            {
                player.NotifyGameState(this.CurrentGameState);
            }
        }

        public void UpdatePlayersCurrentHandState()
        {
            foreach (IPlayer player in PlayersProxy.Values)
            {
                player.NotifyCurrentHandState(this.CurrentHandState);
            }
        }

        public void UpdatePlayerCurrentTrickState()
        {
            foreach (IPlayer player in PlayersProxy.Values)
            {
                player.NotifyCurrentTrickState(this.CurrentTrickState);
            }
        }

        #endregion
                        
    }
}