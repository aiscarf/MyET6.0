using System.Collections.Generic;

namespace ET
{
    public class BattleRoomAwakeSystem: AwakeSystem<BattleRoom, int, int, string, int>
    {
        public override void Awake(BattleRoom self, int a, int b, string c, int d)
        {
            self.Awake(a, b, c, d);
        }
    }

    public sealed class BattleRoom: Entity
    {
        public int RoomId { get; set; }
        public int MapId { get; private set; }
        public string Token { get; private set; } // 门票认证, 用于重连
        public int Seed { get; private set; }

        private Dictionary<long, Fighter> fighterDict = new Dictionary<long, Fighter>();

        public void Add(Fighter fighter)
        {
            this.fighterDict.Add(fighter.Id, fighter);
        }

        public void Awake(int id, int mapId, string token, int seed)
        {
            this.RoomId = id;
            this.MapId = mapId;
            this.Token = token;
            this.Seed = seed;
        }
    }
}