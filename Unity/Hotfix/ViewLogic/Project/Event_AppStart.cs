namespace ET
{
    public class Event_AppStart: AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Log.Debug("App正确启动!");
            
            // DONE: 添加基础开发组件
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            // DONE: 加载配置
            Game.Scene.AddComponent<ResourcesComponent>();
            await ResourcesComponent.Instance.LoadBundleAsync("config.unity3d");
            Game.Scene.AddComponent<ConfigComponent>();
            ConfigComponent.Instance.Load();
            ResourcesComponent.Instance.UnloadBundle("config.unity3d");
            
            // DONE: 添加网络组件
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<NetThreadComponent>();
            Game.Scene.AddComponent<SessionStreamDispatcher>();
            
            // DONE: 添加全局场景管理器组件.
            Game.Scene.AddComponent<ZoneSceneManagerComponent>();
            
            // DONE: 添加全局UI管理器.
            Game.Scene.AddComponent<GlobalComponent>();
            Game.Scene.AddComponent<UIManager>();
            Game.Scene.AddComponent<UIMediatorManager>();

            // TODO: 添加全局声音管理器.
            
            // DONE: 切换至登录场景.
            await ZoneSceneManagerComponent.Instance.ChangeScene(SceneType.Login);
        }
    }
}
