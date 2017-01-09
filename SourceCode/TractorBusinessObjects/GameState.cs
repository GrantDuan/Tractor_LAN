using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Duan.Xiugang.Tractor.Objects
{
    [DataContract]
    public class GameState
    {
        [DataMember] public readonly List<PlayerEntity> Players;

        public GameState()
        {
            Players = new List<PlayerEntity>();
        }

        public List<PlayerEntity> VerticalTeam
        {
            get
            {
                var team = new List<PlayerEntity>();
                foreach (PlayerEntity player in Players)
                {
                    if (player.Team == GameTeam.VerticalTeam)
                    {
                        team.Add(player);
                    }
                }
                return team;
            }
        }

        public List<PlayerEntity> HorizonTeam
        {
            get
            {
                var team = new List<PlayerEntity>();
                foreach (PlayerEntity player in Players)
                {
                    if (player.Team == GameTeam.HorizonTeam)
                    {
                        team.Add(player);
                    }
                }
                return team;
            }
        }

        public void MakeTeam(List<PlayerEntity> players)
        {
            foreach (PlayerEntity player in Players)
            {
                if (players.Exists(p => p.PlayerId == player.PlayerId))
                {
                    player.Team = GameTeam.VerticalTeam;
                }
                else
                {
                    player.Team = GameTeam.HorizonTeam;
                }
            }
        }

        /// <summary>
        ///     calculate the next state of this game
        /// </summary>
        /// <param name="starter">player id of the starter of this ending hand</param>
        /// <param name="score">socre got by the team without starter</param>
        /// <returns>the starter of next hand</returns>
        public PlayerEntity NextRank(string starter, int score)
        {
            PlayerEntity nextStarter = null;

            if (!Players.Exists(p => p.PlayerId == starter))
            {
                //log
                return null;
            }

            GameTeam starterTeam = Players.Single(p => p.PlayerId == starter).Team;

            if (score >= 80)
            {
                nextStarter = GetNextPlayerAfterThePlayer(false, starter);
                foreach (PlayerEntity player in Players)
                {
                    int scoreCopy = score;
                    while (scoreCopy >= 120)
                    {
                        //2,J必打
                        if (player.Team != starterTeam && player.Rank != 0 && player.Rank != 9)
                        {
                            player.Rank = player.Rank + 1;
                            scoreCopy -= 40;
                        }
                        else
                            break;
                    }
                }
            }
            else
            {
                nextStarter = GetNextPlayerAfterThePlayer(true, starter);
                if (score == 0)
                {
                    foreach (PlayerEntity player in Players)
                    {
                        if (player.Team == starterTeam)
                        {
                            //J必打
                            if (player.Rank < 9 && player.Rank + 3 > 9)
                                player.Rank = 9;
                            else
                                player.Rank = player.Rank + 3;
                        }
                    }
                }
                else if (score < 40)
                {
                    foreach (PlayerEntity player in Players)
                    {
                        if (player.Team == starterTeam)
                        {
                            //J必打
                            if (player.Rank < 9 && player.Rank + 2 > 9)
                                player.Rank = 9;
                            else
                                player.Rank = player.Rank + 2;
                        }
                    }
                }
                else
                {
                    foreach (PlayerEntity player in Players)
                    {
                        if (player.Team == starterTeam)
                            player.Rank = player.Rank + 1;
                    }
                }
            }

            return nextStarter;
        }

        /// <summary>
        /// </summary>
        /// <param name="handState"></param>
        /// <param name="lastTrickState">扣抵的牌</param>
        /// <returns></returns>
        public PlayerEntity NextRank(CurrentHandState handState, CurrentTrickState lastTrickState)
        {
            if (!Players.Exists(p => p.PlayerId == handState.Starter))
            {
                //log
                return null;
            }


            if (handState.Rank != 9)
            {
                return NextRank(handState.Starter, handState.Score);
            }
            if (handState.Score >= 80)
            {
                //主J勾到底

                var cardscp = new CurrentPoker(lastTrickState.ShowedCards[lastTrickState.LatestPlayerShowedCard()],
                    handState.Trump, handState.Rank);
                if (cardscp.MasterRank > 0)
                {
                    foreach (PlayerEntity player in Players)
                    {
                        if (ArePlayersInSameTeam(handState.Starter,
                            player.PlayerId))
                        {
                            player.Rank = 0;
                        }
                    }
                }
                    //副J勾一半
                else if (cardscp.SubRank > 0)
                {
                    foreach (PlayerEntity player in Players)
                    {
                        if (ArePlayersInSameTeam(handState.Starter,
                            player.PlayerId))
                        {
                            player.Rank = handState.Rank/2;
                        }
                    }
                }

                return GetNextPlayerAfterThePlayer(false, handState.Starter);
            }

            return null;
        }

        /// <summary>
        ///     get the next player after the player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public PlayerEntity GetNextPlayerAfterThePlayer(string playerId)
        {
            PlayerEntity result = null;
            if (!Players.Exists(p => p.PlayerId == playerId))
            {
                //log
                return result;
            }

            bool afterThePlayer = false;
            foreach (PlayerEntity player in Players)
            {
                if (player.PlayerId != playerId && !afterThePlayer)
                    continue;
                if (player.PlayerId == playerId)
                {
                    afterThePlayer = true;
                }
                else if (player.PlayerId != playerId && afterThePlayer)
                {
                    result = player;
                    break;
                }
            }
            if (result == null)
            {
                foreach (PlayerEntity player in Players)
                {
                    if (player.PlayerId != playerId)
                    {
                        result = player;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     get the next player after the player
        /// </summary>
        /// <param name="inSameTeam">in the starter's team or not</param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public PlayerEntity GetNextPlayerAfterThePlayer(bool inSameTeam, string playerId)
        {
            PlayerEntity result = null;
            if (!Players.Exists(p => p.PlayerId == playerId))
            {
                //log
            }
            GameTeam thePlayerTeam = Players.Single(p => p.PlayerId == playerId).Team;

            bool afterStarter = false;
            foreach (PlayerEntity player in Players)
            {
                if (player.PlayerId != playerId && !afterStarter)
                    continue;
                if (player.PlayerId == playerId)
                {
                    afterStarter = true;
                }
                else if (player.PlayerId != playerId && afterStarter)
                {
                    if ((inSameTeam && player.Team == thePlayerTeam) || (!inSameTeam && player.Team != thePlayerTeam))
                    {
                        result = player;
                        break;
                    }
                }
            }

            if (result == null)
            {
                foreach (PlayerEntity player in Players)
                {
                    if (player.PlayerId != playerId)
                    {
                        if ((inSameTeam && player.Team == thePlayerTeam) ||
                            (!inSameTeam && player.Team != thePlayerTeam))
                        {
                            result = player;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public bool ArePlayersInSameTeam(string playerId1, string playerId2)
        {
            if (VerticalTeam.Exists(p => p.PlayerId == playerId1))
            {
                return VerticalTeam.Exists(p => p.PlayerId == playerId2);
            }
            if (HorizonTeam.Exists(p => p.PlayerId == playerId1))
            {
                return HorizonTeam.Exists(p => p.PlayerId == playerId2);
            }

            return false;
        }
    }


    public enum GameTeam
    {
        None,
        VerticalTeam,
        HorizonTeam
    }
}