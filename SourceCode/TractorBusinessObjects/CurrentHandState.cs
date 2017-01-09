using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Duan.Xiugang.Tractor.Objects
{
    //当前这把牌的状态
    [DataContract]
    public class CurrentHandState
    {
        //庄
        public string Id;
        public Dictionary<string, CurrentPoker> PlayerHoldingCards;
        [DataMember] private string _last8Holder;
        [DataMember] private int _rank;
        [DataMember] private string _starter;
        [DataMember] private Suit _trump;

        public CurrentHandState(GameState gameState)
        {
            PlayerHoldingCards = new Dictionary<string, CurrentPoker>();
            foreach (PlayerEntity player in gameState.Players)
            {
                PlayerHoldingCards[player.PlayerId] = new CurrentPoker();
            }
            LeftCardsCount = TractorRules.GetCardNumberofEachPlayer(gameState.Players.Count);
            Id = Guid.NewGuid().ToString();
        }

        [DataMember]
        public string Starter
        {
            get { return _starter; }
            set
            {
                _starter = value;
                _last8Holder = _starter;
            }
        }

        //埋底牌的玩家

        [DataMember]
        public string Last8Holder
        {
            get { return _last8Holder; }
            set { _last8Holder = value; }
        }

        //打几

        [DataMember]
        public int Rank
        {
            get { return _rank; }
            set
            {
                _rank = value;
                if (PlayerHoldingCards != null)
                {
                    foreach (var keyvalue in PlayerHoldingCards)
                    {
                        keyvalue.Value.Rank = _rank;
                    }
                }
            }
        }

        //主

        [DataMember]
        public Suit Trump
        {
            get { return _trump; }
            set
            {
                _trump = value;
                if (PlayerHoldingCards != null)
                {
                    foreach (var keyvalue in PlayerHoldingCards)
                    {
                        keyvalue.Value.Trump = _trump;
                    }
                }
            }
        }

        //亮主的牌
        [DataMember]
        public TrumpExposingPoker TrumpExposingPoker { get; set; }

        //亮主的人
        [DataMember]
        public string TrumpMaker { get; set; }

        [DataMember]
        public HandStep CurrentHandStep { get; set; }

        [DataMember]
        public bool IsFirstHand { get; set; }

        //埋得的牌
        [DataMember]
        public int[] DiscardedCards { get; set; }

        //得分
        [DataMember]
        public int Score { get; set; }

        public int LeftCardsCount { get; set; }
    }
}