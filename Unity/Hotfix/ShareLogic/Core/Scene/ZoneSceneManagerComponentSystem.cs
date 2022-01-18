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

        public static void ChangeScene(this ZoneSceneManagerComponent self, int zone)
        {
            if (self.CurScene != null)
            {
                Game.EventSystem.Publish(new EventType.LeaveZoneScene() { ZoneScene = self.CurScene });
            }
            
            var scene = self.Get(zone);
            if (scene == null)
            {
                switch (zone)
                {
                    case 1:
                        scene = SceneFactory.CreateLoginScene();
                        break;
                    case 2:
                        scene = SceneFactory.CreateMainScene();
                        break;
                    case 3:
                        scene = SceneFactory.CreateBattleScene();
                        break;
                }
            }

            self.CurScene = scene;
            Game.EventSystem.Publish(new EventType.EnterZoneScene() { ZoneScene = self.CurScene });
        }
    }
}