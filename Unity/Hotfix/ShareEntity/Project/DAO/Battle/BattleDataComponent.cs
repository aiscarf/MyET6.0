using System.Collections.Generic;

namespace ET
{
    public sealed class BattleDataComponent : Entity
    {
        public long BattleId { get; set; }
        public long Uid { get; set; }
        public int MapId { get; set; }
        public int RoomId { get; set; }
        public string Token { get; set; }
        public int RandomSeed { get; set; }
        public string BattleAddress { get; set; }
        public List<MobaPlayerInfo> Players = new List<MobaPlayerInfo>();
    }
}