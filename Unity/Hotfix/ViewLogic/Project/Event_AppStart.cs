namespace ET
{
    public class Event_AppStart: AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Log.Debug("App正确启动!");
            
            // 基础开发组件
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            // 加载配置
            Game.Scene.AddComponent<ResourcesComponent>();
            await ResourcesComponent.Instance.LoadBundleAsync("config.unity3d");
            Game.Scene.AddComponent<ConfigComponent>();
            ConfigComponent.Instance.Load();
            ResourcesComponent.Instance.UnloadBundle("config.unity3d");
            
            // 网络组件
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<NetThreadComponent>();
            Game.Scene.AddComponent<SessionStreamDispatcher>();
            
            // 全局场景管理器组件.
            Game.Scene.AddComponent<ZoneSceneManagerComponent>();
            
            // 全局UI管理器.
            Game.Scene.AddComponent<GlobalComponent>();
            Game.Scene.AddComponent<UIManager>();
            Game.Scene.AddComponent<UIMediatorManager>();

            // 全局声音管理器.
            
            // TODO 切换至登录场景.
            ZoneSceneManagerComponent.Instance.ChangeScene(1);

            // // TODO 切换至主场景.
            // ZoneSceneManagerComponent.Instance.ChangeScene(2);
            //
            // TODO 切换至战斗场景.
            ZoneSceneManagerComponent.Instance.ChangeScene(3);
            
            // TODO 收到服务器通知, 匹配成功 => 进入加载界面, 等待服务器通知所有人都加载完成 => 则立马开始游戏.
            var local3V3BattleLoadData = new Local3v3BattleLoadData();
            var battleLoadData = local3V3BattleLoadData.GetLoadData();
            await Game.EventSystem.PublishAsync(new EventType.EnterMobaBegin() { MobaBattleLoadData = battleLoadData });
        }
    }
}
