using System;
using System.Collections.Generic;
using System.ServiceModel;
using Duan.Xiugang.Tractor.Objects;


namespace Duan.Xiugang.Tractor.Player
{
    public delegate void NewPlayerJoinedEventHandler();
    public delegate void PlayersTeamMadeEventHandler();
    public delegate void GameStartedEventHandler();


    public delegate void GetCardEventHandler(int cardNumber);    
    public delegate void TrumpChangedEventHandler(CurrentHandState currentHandState);
    public delegate void DistributingCardsFinishedEventHandler();
    public delegate void StarterFailedForTrumpEventHandler();

    public delegate void DiscardingLast8EventHandler();
    public delegate void Last8DiscardedEventHandler();    
    
    public delegate void ShowingCardBeganEventHandler();
    public delegate void CurrentTrickStateUpdateEventHandler();
    public delegate void DumpingFailEventHandler(ShowingCardsValidationResult result);
    public delegate void TrickFinishedEventHandler();

    public delegate void HandEndingEventHandler();
    
    
    
    

    public class TractorPlayer : IPlayer
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int Rank { get; set; }

        public GameState CurrentGameState;
        public CurrentPoker CurrentPoker;
        public CurrentHandState CurrentHandState { get; set; }
        public CurrentTrickState CurrentTrickState { get; set; }

        public event NewPlayerJoinedEventHandler NewPlayerJoined;
        public event PlayersTeamMadeEventHandler PlayersTeamMade;
        public event GameStartedEventHandler GameOnStarted;


        public event GetCardEventHandler PlayerOnGetCard;        
        public event TrumpChangedEventHandler TrumpChanged;
        public event DistributingCardsFinishedEventHandler AllCardsGot;
        public event StarterFailedForTrumpEventHandler StarterFailedForTrump; //亮不起


        public event DiscardingLast8EventHandler DiscardingLast8;
        public event Last8DiscardedEventHandler Last8Discarded;

        public event ShowingCardBeganEventHandler ShowingCardBegan;
        public event CurrentTrickStateUpdateEventHandler PlayerShowedCards;
        public event DumpingFailEventHandler DumpingFail; //甩牌失败
        public event TrickFinishedEventHandler TrickFinished;

        
        public event HandEndingEventHandler HandEnding;

        private readonly ITractorHost _tractorHost;
        

        public TractorPlayer()
        {
            CurrentPoker = new CurrentPoker();
            PlayerId = Guid.NewGuid().ToString();
            CurrentGameState = new GameState();
            CurrentHandState = new CurrentHandState(CurrentGameState);
            CurrentTrickState = new CurrentTrickState();

            var instanceContext = new InstanceContext(this);
            var channelFactory = new DuplexChannelFactory<ITractorHost>(instanceContext, "NetTcpBinding_ITractorHost");

                _tractorHost = channelFactory.CreateChannel();



        }

        //player get card 
        public void GetDistributedCard(int number)
        {
            this.CurrentPoker.AddCard(number);
            if (PlayerOnGetCard != null)
                PlayerOnGetCard(number);

            if (this.CurrentPoker.Count == TractorRules.GetCardNumberofEachPlayer(this.CurrentGameState.Players.Count) && this.PlayerId != this.CurrentHandState.Last8Holder)
            {
                if (AllCardsGot != null)
                    AllCardsGot();
            }
            else if (this.CurrentPoker.Count == TractorRules.GetCardNumberofEachPlayer(this.CurrentGameState.Players.Count) + 8)
            {
                if (AllCardsGot != null)
                    AllCardsGot();
            }

        }


        public void ClearAllCards()
        {
            this.CurrentPoker.Clear();
        }

        

        public void Ready()
        {
            _tractorHost.PlayerIsReady(this.PlayerId);
        }

        public void Quit()
        {
            _tractorHost.PlayerQuit(this.PlayerId);
            (_tractorHost as IDisposable).Dispose();
        }

        public void ShowCards(List<int> cards)
        {
            if (this.CurrentTrickState.NextPlayer() == PlayerId)
            {
                this.CurrentTrickState.ShowedCards[PlayerId] = cards;

                _tractorHost.PlayerShowCards(this.CurrentTrickState);
                
            }
        }

        public void StartGame()
        {
            ClearAllCards();
            this.CurrentPoker.Rank = CurrentHandState.Rank;

            if (GameOnStarted != null)
                GameOnStarted();
        }

        


        public void ExposeTrump(TrumpExposingPoker trumpExposingPoker, Suit trump)
        {
            _tractorHost.PlayerMakeTrump(trumpExposingPoker, trump, this.PlayerId);
            
        }

        public void NotifyCurrentHandState(CurrentHandState currentHandState)
        {
            bool trumpChanged = false;
            bool newHandStep = false;
            bool starterChanged = false;
            trumpChanged = this.CurrentHandState.Trump != currentHandState.Trump || (this.CurrentHandState.Trump == currentHandState.Trump && this.CurrentHandState.TrumpExposingPoker < currentHandState.TrumpExposingPoker);
            newHandStep = this.CurrentHandState.CurrentHandStep != currentHandState.CurrentHandStep;
            starterChanged = this.CurrentHandState.Starter  != currentHandState.Starter;

            this.CurrentHandState = currentHandState;
            this.CurrentPoker.Trump = this.CurrentHandState.Trump;

            if (trumpChanged)
            {
                if (TrumpChanged != null)
                    TrumpChanged(currentHandState);

                //resort cards
                if (currentHandState.CurrentHandStep > HandStep.DistributingCards)
                {
                    if (AllCardsGot != null)
                        AllCardsGot();
                }
            }

            if (currentHandState.CurrentHandStep == HandStep.BeforeDistributingCards)
            {
                StartGame();
            }
            else if (newHandStep)
            {
                if (currentHandState.CurrentHandStep == HandStep.DistributingCardsFinished)
                {
                    if (AllCardsGot != null)
                        AllCardsGot();
                }

                else if (currentHandState.CurrentHandStep == HandStep.DiscardingLast8Cards)
                {
                    if (DiscardingLast8 != null)
                        DiscardingLast8();
                }

                else if (currentHandState.CurrentHandStep == HandStep.DistributingCardsFinished)
                {
                    if (AllCardsGot != null)
                        AllCardsGot();
                }

                else if (currentHandState.CurrentHandStep == HandStep.DiscardingLast8CardsFinished)
                {
                    if (Last8Discarded != null)
                        Last8Discarded();
                }
                //player begin to showing card
                //开始出牌
                else if (currentHandState.CurrentHandStep == HandStep.Playing)
                {
                    if (AllCardsGot != null)
                        AllCardsGot();

                    if (ShowingCardBegan != null)
                        ShowingCardBegan();
                }

                else if (currentHandState.CurrentHandStep == HandStep.Ending)
                {
                    if (HandEnding != null)
                        HandEnding();
                }
            }

            //出完牌，庄家亮不起主，所以换庄家
            if (currentHandState.CurrentHandStep == HandStep.DistributingCardsFinished && starterChanged)
            {
                this.CurrentPoker.Rank = this.CurrentHandState.Rank;
                if (StarterFailedForTrump != null)
                {
                    StarterFailedForTrump();
                }
            }

        }

        public void NotifyCurrentTrickState(CurrentTrickState currentTrickState)
        {
            this.CurrentTrickState = currentTrickState;

            if (this.CurrentTrickState.LatestPlayerShowedCard() != "")
            {
                if (PlayerShowedCards != null)
                    PlayerShowedCards();
            }

            if (!string.IsNullOrEmpty(this.CurrentTrickState.Winner))
            {
                if (TrickFinished != null)
                    TrickFinished();
            }

        }

        public void NotifyGameState(GameState gameState)
        {
            bool newPlayerJoined = false;
            bool teamMade = false;            

            if (gameState.Players.Count > this.CurrentGameState.Players.Count)
                newPlayerJoined = true;
            if (this.CurrentGameState.Players.Count > 0)
            {
                if (this.CurrentGameState.Players[0].Team == GameTeam.None &&
                gameState.Players[0] != null && gameState.Players[0].Team != GameTeam.None)
                teamMade = true;
            }
            else
            {
                if (gameState.Players[0] != null && gameState.Players[0].Team != GameTeam.None)
                    teamMade = true;
            }

            this.CurrentGameState = gameState;

            if (newPlayerJoined)
            {
                if (NewPlayerJoined != null)
                {
                    NewPlayerJoined();
                }
            }

            if (teamMade)
            {
                if (PlayersTeamMade != null)
                {
                    PlayersTeamMade();
                }
            }

        }

        public ShowingCardsValidationResult ValidateDumpingCards(List<int> selectedCards)
        {

            var result = _tractorHost.ValidateDumpingCards(selectedCards, this.PlayerId);
            
            return result;
        }


        public void NotifyDumpingValidationResult(ShowingCardsValidationResult result)
        {
            if (result.ResultType == ShowingCardsValidationResultType.DumpingFail)
            {
                if (DumpingFail != null)
                    DumpingFail(result);
            }
        }


        public void DiscardCards(int[] cards)
        {
            if (this.CurrentHandState.Last8Holder != PlayerId)
                return;

            _tractorHost.StoreDiscardedCards(cards);
            
        }

        //我是否可以亮主
        public List<Suit> AvailableTrumps()
        {
            List<Suit> availableTrumps = new List<Suit>();
            int rank = this.CurrentHandState.Rank;

            if (this.PlayerId == this.CurrentHandState.Starter && this.CurrentHandState.CurrentHandStep >= HandStep.DistributingLast8Cards)
            {
                availableTrumps.Clear();
                return availableTrumps;
            }

            //当前是自己亮的单张主，只能加固
            if (this.CurrentHandState.TrumpMaker == this.PlayerId)
            {
                if (this.CurrentHandState.TrumpExposingPoker == TrumpExposingPoker.SingleRank)
                {
                    if (rank != 53)
                    {
                        if (this.CurrentPoker.Clubs[rank] > 1)
                        {
                            if (this.CurrentHandState.Trump == Suit.Club)
                                availableTrumps.Add(Suit.Club);
                        }
                        if (this.CurrentPoker.Diamonds[rank] > 1)
                        {
                            if (this.CurrentHandState.Trump == Suit.Diamond)
                                availableTrumps.Add(Suit.Diamond);
                        }
                        if (this.CurrentPoker.Spades[rank] > 1)
                        {
                            if (this.CurrentHandState.Trump == Suit.Spade)
                                availableTrumps.Add(Suit.Spade);
                        }
                        if (this.CurrentPoker.Hearts[rank] > 1)
                        {
                            if (this.CurrentHandState.Trump == Suit.Heart)
                                availableTrumps.Add(Suit.Heart);
                        }
                    }
                }
                return availableTrumps;
            }

            //如果目前无人亮主
            if (this.CurrentHandState.TrumpExposingPoker == TrumpExposingPoker.None)
            {
                if (rank != 53)
                {
                    if (this.CurrentPoker.Clubs[rank] > 0)
                    {
                        availableTrumps.Add(Suit.Club);
                    }
                    if (this.CurrentPoker.Diamonds[rank] > 0)
                    {
                        availableTrumps.Add(Suit.Diamond);
                    }
                    if (this.CurrentPoker.Spades[rank] > 0)
                    {
                        availableTrumps.Add(Suit.Spade);
                    }
                    if (this.CurrentPoker.Hearts[rank] > 0)
                    {
                        availableTrumps.Add(Suit.Heart);
                    }
                }
            }
            //亮了单张
            else if (this.CurrentHandState.TrumpExposingPoker == TrumpExposingPoker.SingleRank)
            {

                if (rank != 53)
                {
                    if (this.CurrentPoker.Clubs[rank] > 1)
                    {
                        availableTrumps.Add(Suit.Club);
                    }
                    if (this.CurrentPoker.Diamonds[rank] > 1)
                    {
                        availableTrumps.Add(Suit.Diamond);
                    }
                    if (this.CurrentPoker.Spades[rank] > 1)
                    {
                        availableTrumps.Add(Suit.Spade);
                    }
                    if (this.CurrentPoker.Hearts[rank] > 1)
                    {
                        availableTrumps.Add(Suit.Heart);
                    }
                }
            }

            if (this.CurrentHandState.TrumpExposingPoker != TrumpExposingPoker.PairRedJoker)
            {
                if (rank != 53)
                {
                    if (this.CurrentPoker.BlackJoker == 2)
                    {
                        availableTrumps.Add(Suit.Joker);
                    }
                }
            }


            if (rank != 53)
            {
                if (this.CurrentPoker.RedJoker == 2)
                {
                    availableTrumps.Add(Suit.Joker);
                }
            }
            return availableTrumps;
        }
    }
}
