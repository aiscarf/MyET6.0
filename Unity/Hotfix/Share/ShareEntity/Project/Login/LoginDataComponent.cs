using System.Collections.Generic;

namespace ET
{
    public class LoginDataComponent : Entity
    {
        public bool IsLoginRealm;
        public string RealmToken;
        public List<GameService> GameServices = new List<GameService>();
        public List<GameRegion> GameRegions = new List<GameRegion>();
    }
}