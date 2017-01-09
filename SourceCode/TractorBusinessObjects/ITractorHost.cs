using System.Collections.Generic;
using System.ServiceModel;

namespace Duan.Xiugang.Tractor.Objects
{
    [ServiceContract(CallbackContract = typeof (IPlayer))]
    public interface ITractorHost
    {
        CardsShoe CardsShoe { get; set; }
        Dictionary<string, IPlayer> PlayersProxy { get; set; }

        [OperationContract(IsOneWay = true)]
        void PlayerIsReady(string playerId);

        [OperationContract(IsOneWay = true)]
        void PlayerQuit(string playerId);

        [OperationContract(IsOneWay = true)]
        void PlayerMakeTrump(TrumpExposingPoker trumpExposingPoker, Suit trump, string playerId);

        [OperationContract(IsOneWay = true)]
        void StoreDiscardedCards(int[] cards);

        //玩家出牌
        [OperationContract(IsOneWay = true)]
        void PlayerShowCards(CurrentTrickState currentTrickState);

        //甩牌检查
        [OperationContract]
        ShowingCardsValidationResult ValidateDumpingCards(List<int> selectedCards, string playerId);
    }
}