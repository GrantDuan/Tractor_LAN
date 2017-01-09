namespace Duan.Xiugang.Tractor.Objects
{
    public enum Suit
    {
        None,
        Heart,
        Spade,
        Diamond,
        Club,
        Joker
    }

    public enum TrumpExposingPoker
    {
        None,
        SingleRank,
        PairRank,
        PairBlackJoker,
        PairRedJoker
    }

    //每把牌的不同阶段
    public enum HandStep
    {
        BeforeDistributingCards,
        DistributingCards,
        DistributingCardsFinished,
        DistributingLast8Cards,
        DistributingLast8CardsFinished,
        DiscardingLast8Cards,
        DiscardingLast8CardsFinished,
        Last8CardsRobbed,
        Playing,
        Ending
    }
}