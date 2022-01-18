namespace ET
{
    public static class SceneFactory
    {
        // public static async ETTask<Scene> CreateZoneScene(int zone, string name, Entity parent)
        // {
        //     Scene zoneScene = EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateInstanceId(), zone, SceneType.Battle, name, parent);
        //     
        //     // TODO 当进入moba战斗场景时, 应加载的功能模块.
        //     zoneScene.AddComponent<ZoneSceneFlagComponent>();
        //     zoneScene.AddComponent<NetKcpComponent, int>(SessionStreamDispatcherType.SessionStreamDispatcherClientOuter);
        //     // zoneScene.AddComponent<UnitComponent>();
        //
        //     // 挂点
        //     return zoneScene;
        // }
        
        public static Scene CreateLoginScene()
        {
            return EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateInstanceId(), 1, SceneType.Login, "Login", Game.Scene);
        }

        public static Scene CreateMainScene()
        {
            return EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateInstanceId(), 2, SceneType.Main, "Main", Game.Scene);
        }
        
        public static Scene CreateBattleScene()
        {
            return EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateInstanceId(), 3, SceneType.Battle, "Battle", Game.Scene);
        }
    }
}