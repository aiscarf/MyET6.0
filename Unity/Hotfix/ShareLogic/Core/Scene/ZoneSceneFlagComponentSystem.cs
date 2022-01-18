namespace ET
{
    [ObjectSystem]
    public class ZoneSceneFlagComponentAwakeSystem : AwakeSystem<ZoneSceneFlagComponent>
    {
        public override void Awake(ZoneSceneFlagComponent self)
        {
            ZoneSceneManagerComponent.Instance.Add(self.GetParent<Scene>());
        }
    }

    [ObjectSystem]
    public class ZoneSceneFlagComponentDestroySystem : DestroySystem<ZoneSceneFlagComponent>
    {
        public override void Destroy(ZoneSceneFlagComponent self)
        {
            ZoneSceneManagerComponent.Instance.Remove(self.DomainZone());
        }
    }
}