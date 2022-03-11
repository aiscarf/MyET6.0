using System.Collections.Generic;

namespace ET
{
    public sealed class BattleRoom: Entity
    {
        public int RoomId { get; set; }
        public int MapId { get; private set; }
        public string Token { get; private set; } // 门票认证, 用于重连
        public int Seed { get; private set; }
        
        public bool IsReady { get; set; }
        
        public bool IsEnd { get; set; }

        public int m_nCurFrameId { get; set; }

        public ETCancellationToken cancellationToken { get; set; }
        public bool IsCountdownDestroy { get; set; }
        public long TimerId;
        public long BattleId { get; set; }
        
        public B2C_OnFrame m_nextFrameOpt { get; set; }

        private Dictionary<long, Fighter> fighterDict = new Dictionary<long, Fighter>();

        public void Add(Fighter fighter)
        {
            this.fighterDict.Add(fighter.Id, fighter);
        }

        public Fighter Get(long uid)
        {
            Fighter result = null;
            this.fighterDict.TryGetValue(uid, out result);
            return result;
        }

        public void CheckRoomReady()
        {
            foreach (var kv in this.fighterDict)
            {
                if (kv.Value.IsReady == false)
                {
                    this.IsReady = false;
                    return;    
                }
            }

            this.IsReady = true;
        }

        public Dictionary<long, Fighter> GetAllFighters()
        {
            return this.fighterDict;
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