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
            result.PlayerInfos = new List<MobaPlayerInfo>()
            {
                new MobaPlayerInfo()
                {
                    Uid = 1001,
                    Nickname = "玩家角色",
                    HeroId = 1001,
                    HeroSkinId = 101,
                    TowerSkinId = 1001,
                    PetId = 1001,
                    PetSkinId = 1001,
                    Score = 1000,
                    ChairId = 1,
                    Camp = 1,
                    HeadId = 1,
                    FrameId = 1,
                    ShowId = 1,
                    HeroLv = 1,
                    PetLv = 1,
                    UnlockedSkill = new List<int>() { 1001, 1002 },
                }
            };
            result.NeedLoadPanelIds = new List<string>(){UIType.UIBattle};
            return result;
        }
    }
}