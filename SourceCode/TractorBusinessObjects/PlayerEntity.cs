using System.Runtime.Serialization;

namespace Duan.Xiugang.Tractor.Objects
{
    [DataContract]
    public class PlayerEntity
    {
        [DataMember]
        public string PlayerId { get; set; }

        [DataMember]
        public string PlayerName { get; set; }

        [DataMember]
        public int Rank { get; set; }

        [DataMember]
        public GameTeam Team { get; set; }
    }
}