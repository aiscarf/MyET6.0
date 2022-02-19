using System.Collections.Generic;

namespace ET
{
    public class MobaBattleLoadData
    {        
        public int RandomSeed;
        public string ScenePfbPath = null;
        public string MapConfigPath = null;
        public List<PlayerInfo> PlayerInfos = null;
        public List<string> NeedLoadPanelIds = new List<string>();
    }
}