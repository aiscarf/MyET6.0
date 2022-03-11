using System;
using System.Collections.Generic;
using ET.EventType;

namespace ET
{
    public class Event_LoginBattleFinish_View : AEvent<EventType.LoginBattleFinish>
    {
        private static Dictionary<int, List<string>> NeedLoadPanels = new Dictionary<int, List<string>>()
        {
            { 1, new List<string>() { UIType.UIBattle } },
        };

        protected override async ETTask Run(EventType.LoginBattleFinish args)
        {
            try
            {
                // DONE: 先打开加载场景的进度界面.
                await UIHelper.OpenUI(UIType.UIBattleLoading);

                int mapId = args.MapId;
                int randomSeed = args.RandomSeed;
                var allPlayers = args.Players;

                // DONE: 根据不同模式加载不同的资源.
                var dungeonConfig = DungeonConfigCategory.Instance.Get(mapId);

                // DONE: 加载全部所需资源, 并初始化战斗数据.
                await Game.EventSystem.PublishAsync(new EnterMobaBegin()
                {
                    BattleMode = (EBattleMode)mapId,
                    PlayerInfos = allPlayers,
                    RandomSeed = randomSeed,
                    ScenePfbPath = dungeonConfig.ScenePath,
                    MapConfigPath = dungeonConfig.ConfigPath,
                    NeedLoadPanelIds = NeedLoadPanels[mapId],
                });

                // DONE: 通知服务器游戏加载完成.
                await Game.EventSystem.PublishAsync(new EnterMobaFinish());
                await BattleHelper.BattleStart();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}