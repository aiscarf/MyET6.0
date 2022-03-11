using System.Collections.Generic;

namespace ET
{
    public class LoginViewDataComponent : Entity
    {
        public readonly DataProxy<bool> IsLoginRealmProxy = new DataProxy<bool>(false);
        public long Uid;
        public string RealmToken;
        public List<GameServiceVO> GameServiceVos = new List<GameServiceVO>();
        public List<GameRegionVO> GameRegionVos = new List<GameRegionVO>();
        
        public readonly DataProxy<GameRegionVO> CurSelectRegionProxy = new DataProxy<GameRegionVO>(null);
    }

    public class GameServiceVO
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public long ServiceStartTime { get; set; }
    }

    public class GameRegionVO
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int State { get; set; }
        public string Address { get; set; }
    }
}