using System.Collections.Generic;

namespace ET
{
    public class Local3v3BattleLoadData : IGetBattleLoadData
    {
        public MobaBattleLoadData GetLoadData()
        {
            var result = new MobaBattleLoadData();
            result.RandomSeed = 10000;
            result.ScenePfbPath = "map_json_1";
            result.MapConfigPath = "config_map_json_1";
            result.PlayerInfos = new List<PlayerInfo>()
            {
                new PlayerInfo()
                {
                    uid = 1001,
                    nickname = "玩家角色",
                    heroId = 1001,
                    heroSkinId = 101,
                    towerSkinId = 1001,
                    petId = 1001,
                    petSkinId = 1001,
                    score = 1000,
                    chairId = 1,
                    camp = 1,
                    headId = 1,
                    frameId = 1,
                    showId = 1,
                    heroLv = 1,
                    petLv = 1,
                    unlockedSkill = new List<int>() { 1001, 1002 },
                }
            };
            result.NeedLoadPanelIds = new List<string>(){UIType.UIBattle};
            return result;
        }
    }
}