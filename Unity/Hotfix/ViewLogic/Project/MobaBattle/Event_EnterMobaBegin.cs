using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET
{
    public class Event_EnterMobaBegin : AEvent<EventType.EnterMobaBegin>
    {
        private Dictionary<int, string> HeroLoadPath = new Dictionary<int, string>()
        {
            {1001, "hero01_model01_l"},
        };

        protected override async ETTask Run(EventType.EnterMobaBegin args)
        {
            // TODO 收到了开始游戏的数据.
            await ETTask.CompletedTask;
            
            // 打开加载场景的进度界面.
            await UIHelper.OpenUI(UIType.UIBattleLoading);
            
            // TODO 如何解耦合, 推送更新进度消息.
            UIManager.Instance.GetUI(UIType.UIBattleLoading).GetComponent<UIBattleLoadingComponent>().EUI_Slider_Slider.value = 0f;
            // 加载场景资源
            await ResourcesComponent.Instance.LoadBundleAsync("mobabattle.unity3d");

            // 切换到map场景
            SceneChangeComponent sceneChangeComponent = null;
            try
            {
                sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent, string>("MobaBattleScene");
                await sceneChangeComponent.tcs;
            }
            finally
            {
                sceneChangeComponent?.Dispose();
            }
            
            var battleLoadData = args.MobaBattleLoadData;
            
            // 等待加载窗体.
            var needLoadUis = battleLoadData.NeedLoadPanelIds;
            foreach (string needLoadUi in needLoadUis)
            {
                await UIManager.Instance.CreateUI(needLoadUi);
            }
            
            // 等待加载场景预制体的包.
            await ResourcesComponent.Instance.LoadBundleAsync(args.MobaBattleLoadData.ScenePfbPath.StringToAB());
            await ResourcesComponent.Instance.LoadBundleAsync(args.MobaBattleLoadData.MapConfigPath.StringToAB());

            var mapPfb = ResourcesComponent.Instance.GetAsset(args.MobaBattleLoadData.ScenePfbPath.StringToAB(), args.MobaBattleLoadData.ScenePfbPath);
            var map = (GameObject)UnityEngine.Object.Instantiate(mapPfb);
            
            // 初始化视图组件.
            var mobaScene = SceneManager.GetSceneByName("MobaBattleScene");
            var mobaSceneRoots = mobaScene.GetRootGameObjects();
            var mobaBattleViewComponent = ZoneSceneManagerComponent.Instance.CurScene.AddComponent<MobaBattleViewComponent>();
            mobaBattleViewComponent.InitRoot(mobaSceneRoots);

            // 设置场景挂点.
            map.transform.SetParent(mobaBattleViewComponent.MapRoot.transform);

            // 加载角色资源.
            for (int i = 0; i < args.MobaBattleLoadData.PlayerInfos.Count; i++)
            {
                var playerInfo = args.MobaBattleLoadData.PlayerInfos[i];
                await ResourcesComponent.Instance.LoadBundleAsync(HeroLoadPath[playerInfo.heroId].StringToAB());
            }

            // TODO 假的模拟进度条.
            UIManager.Instance.GetUI(UIType.UIBattleLoading).GetComponent<UIBattleLoadingComponent>().EUI_Slider_Slider.value = 0.5f;
            await TimerComponent.Instance.WaitAsync(2000);
            UIManager.Instance.GetUI(UIType.UIBattleLoading).GetComponent<UIBattleLoadingComponent>().EUI_Slider_Slider.value = 1f;
            await TimerComponent.Instance.WaitAsync(500);
            
            // TODO 通知服务器加载完毕, 等待游戏通知正式开始游戏.
            Game.EventSystem.Publish(new EventType.EnterMobaFinish() { MobaBattleLoadData = args.MobaBattleLoadData });
        }
    }
}