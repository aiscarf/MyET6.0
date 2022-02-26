using System.Collections.Generic;

namespace ET
{
    public class MainDataComponent : Entity
    {
        public long SessionId { get; set; }
        public string GateToken { get; set; }
        public PlayerInfo PlayerInfo { get; set; }
        public long ServerTime { get; set; }
        public List<FriendInfo> FriendInfos { get; set; }
    }
}