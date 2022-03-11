namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> CreateZoneScene(int zone, SceneType sceneType, string name, Entity parent)
        {
            Scene zoneScene = EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateInstanceId(), zone, sceneType, name, parent);
            zoneScene.AddComponent<ZoneSceneFlagComponent>();
            zoneScene.AddComponent<NetKcpComponent, int>(SessionStreamDispatcherType.SessionStreamDispatcherClientOuter);
            await ETTask.CompletedTask;
            return zoneScene;
        }
    }
}