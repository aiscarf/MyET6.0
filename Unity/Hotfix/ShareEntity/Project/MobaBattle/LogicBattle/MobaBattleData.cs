using System.Collections.Generic;

namespace ET
{
    public class MobaBattleData
    {
        public EBattleMode BattleMode { get; set; }
        public bool IsNet { get; set; }
        public MapData MapData { get; set; }
        public List<MobaPlayerInfo> Players = new List<MobaPlayerInfo>();
    }
}