using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ET
{
    public class Event_EnterMobaBegin_View : AEvent<EventType.EnterMobaBegin>
    {
        protected override async ETTask Run(EventType.EnterMobaBegin args)
        {
            var battleDataComponent = DataHelper.GetDataComponentFromCurScene<BattleDataComponent>();
            var battleViewDataComponent = BattleMgr.GetBattleViewDataComponent();

            // DONE: 统计任务总进度.
            int totalCount = 3 + args.PlayerInfos.Count + args.NeedLoadPanelIds.Count;
            int curCount = 0;

            Action updateProgressAction = () =>
            {
                battleViewDataComponent.LoadingProgressProxy.SetValue(++curCount / (float)totalCount);
            };

            // DONE: 加载条初始为0.
            battleViewDataComponent.LoadingProgressProxy.SetValue(0f);

            // DONE: 加载场景包体.
            await ResourcesComponent.Instance.LoadBundleAsync("mobabattle.unity3d");

            // DONE: 0.优先切换到map场景
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

            updateProgressAction.Invoke();

            // DONE: 1.加载窗体.
            var needLoadUis = args.NeedLoadPanelIds;
            for (int i = 0; i < needLoadUis.Count; i++)
            {
                await UIManager.Instance.CreateUI(needLoadUis[i]);
                updateProgressAction.Invoke();
            }

            // DONE: 2.1初始化场景地图预制体.
            await ResourcesComponent.Instance.LoadBundleAsync(args.ScenePfbPath.StringToAB());
            var mapPfb = ResourcesComponent.Instance.GetAsset(args.ScenePfbPath.StringToAB(), args.ScenePfbPath);
            var map = (GameObject)UnityEngine.Object.Instantiate(mapPfb);
            var mobaScene = SceneManager.GetSceneByName("MobaBattleScene");
            var mobaSceneRoots = mobaScene.GetRootGameObjects();

            updateProgressAction.Invoke();

            // DONE: 2.2初始化场景地图数据.
            await ResourcesComponent.Instance.LoadBundleAsync(args.MapConfigPath.StringToAB());
            TextAsset textAsset =
                (TextAsset)ResourcesComponent.Instance.GetAsset(args.MapConfigPath.StringToAB(), args.MapConfigPath);
            var mapData = JsonHelper.FromJson(typeof(MapData), textAsset.text) as MapData;
            updateProgressAction.Invoke();

            // DONE: 3.加载角色资源包.
            for (int i = 0; i < args.PlayerInfos.Count; i++)
            {
                var playerInfo = args.PlayerInfos[i];
                var heroConfig = HeroConfigCategory.Instance.Get(playerInfo.HeroId);
                await ResourcesComponent.Instance.LoadBundleAsync(heroConfig.ModelRes.StringToAB());
                updateProgressAction.Invoke();
            }

            // DONE: 资源加载完毕.
            battleViewDataComponent.LoadingProgressProxy.SetValue(0.9f);

            // DONE: 创建战斗数据层.
            var mobaBattleEntity = ZoneSceneManagerComponent.Instance.CurScene.AddChild<MobaBattleEntity>();
            battleDataComponent.BattleId = mobaBattleEntity.Id;

            // DONE: 创建挂点.
            var mobaBattleViewComponent = mobaBattleEntity.AddComponent<MobaBattleViewComponent>();
            mobaBattleViewComponent.InitRoot(mobaSceneRoots);
            map.transform.SetParent(mobaBattleViewComponent.MapRoot.transform);
            map.transform.localScale = Vector3.one;
            map.transform.localEulerAngles = Vector3.zero;
            map.transform.localPosition = Vector3.zero;

            mobaBattleEntity.Init(new MobaBattleData()
                { IsNet = true, BattleMode = args.BattleMode, MapData = mapData, Players = args.PlayerInfos });
            
            if (mobaBattleEntity.m_bIsNet)
            {
                mobaBattleEntity.AddComponent<NetMobaServerComponent>();
            }
            else
            {
                mobaBattleEntity.AddComponent<LocalMobaServerComponent>();
            }
        }
    }
}