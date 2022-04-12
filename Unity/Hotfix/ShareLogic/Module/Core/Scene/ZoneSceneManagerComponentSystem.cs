﻿namespace ET
{
    [ObjectSystem]
    public class ZoneSceneManagerComponentAwakeSystem : AwakeSystem<ZoneSceneManagerComponent>
    {
        public override void Awake(ZoneSceneManagerComponent self)
        {
            ZoneSceneManagerComponent.Instance = self;
        }
    }

    [ObjectSystem]
    public class ZoneSceneManagerComponentDestroySystem : DestroySystem<ZoneSceneManagerComponent>
    {
        public override void Destroy(ZoneSceneManagerComponent self)
        {
            self.ZoneScenes.Clear();
        }
    }

    public static class ZoneSceneManagerComponentSystem
    {
        public static Scene ZoneScene(this Entity entity)
        {
            return ZoneSceneManagerComponent.Instance.Get(entity.DomainZone());
        }

        public static void Add(this ZoneSceneManagerComponent self, Scene zoneScene)
        {
            self.ZoneScenes.Add(zoneScene.Zone, zoneScene);
        }

        public static Scene Get(this ZoneSceneManagerComponent self, int zone)
        {
            self.ZoneScenes.TryGetValue(zone, out Scene scene);
            return scene;
        }

        public static void Remove(this ZoneSceneManagerComponent self, int zone)
        {
            self.ZoneScenes.Remove(zone);
        }

        public static async ETTask ChangeScene(this ZoneSceneManagerComponent self, SceneType sceneType)
        {
            await Game.EventSystem.PublishAsync(new EventType.LeaveZoneScene() { LeaveZone = self.CurScene, NextSceneType = sceneType });

            var scene = self.Get((int)sceneType);
            if (scene == null)
            {
                scene = await SceneFactory.CreateZoneScene((int)sceneType, sceneType, sceneType.ToString(), Game.Scene);
            }

            self.CurScene = scene;

            await Game.EventSystem.PublishAsync(new EventType.EnterZoneSceneBefore() { ZoneScene = self.CurScene });
            await Game.EventSystem.PublishAsync(new EventType.EnterZoneSceneAfter() { ZoneScene = self.CurScene });
        }
    }
}