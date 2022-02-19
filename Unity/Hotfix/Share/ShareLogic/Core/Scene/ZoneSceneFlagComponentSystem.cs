namespace ET
{
    [ObjectSystem]
    public class ZoneSceneFlagComponentAwakeSystem : AwakeSystem<ZoneSceneFlagComponent>
    {
        public override void Awake(ZoneSceneFlagComponent self)
        {
            // 场景创建的事件.
            var scene = self.GetParent<Scene>();
            Game.EventSystem.Publish(new EventType.CreateZoneScene() { ZoneScene = scene });
            ZoneSceneManagerComponent.Instance.Add(scene);
        }
    }

    [ObjectSystem]
    public class ZoneSceneFlagComponentDestroySystem : DestroySystem<ZoneSceneFlagComponent>
    {
        public override void Destroy(ZoneSceneFlagComponent self)
        {
            // 场景销毁的事件.
            var scene = self.DomainScene();
            Game.EventSystem.Publish(new EventType.DestroyZoneScene() { ZoneScene = scene });
            ZoneSceneManagerComponent.Instance.Remove(scene.Zone);
        }
    }
}