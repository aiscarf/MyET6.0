using UnityEngine;

namespace ET
{
    public class Event_EnterMobaFinish : AEvent<EventType.EnterMobaFinish>
    {
        protected override async ETTask Run(EventType.EnterMobaFinish args)
        {
            await ETTask.CompletedTask;
            
            // 关闭加载场景的进度界面.
            await UIManager.Instance.DestroyUI(UIType.UIBattleLoading);
            
            TextAsset textAsset = (TextAsset)ResourcesComponent.Instance.GetAsset(args.MobaBattleLoadData.MapConfigPath.StringToAB(), "map_json_1");
            var mapData = JsonHelper.FromJson<MapData>(textAsset.text);
            Game.EventSystem.Publish(new EventType.MobaBattleDataInit() { mapData = mapData, bIsNet = false, eBattleMode = EBattleMode.E3v3, lstPlayerInfos = args.MobaBattleLoadData.PlayerInfos});
        }
    }
}