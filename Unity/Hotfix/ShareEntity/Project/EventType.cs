using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        /// <summary>
        /// 启动App
        /// </summary>
        public struct AppStart
        {
        }

        public struct Reload
        {
        }

        public struct CreateZoneScene
        {
            public Scene ZoneScene;
        }

        public struct DestroyZoneScene
        {
            public Scene ZoneScene;
        }

        public struct LeaveZoneScene
        {
            public Scene LeaveZone;
            public SceneType NextSceneType;
        }

        public struct EnterZoneSceneBefore
        {
            public Scene ZoneScene;
        }

        public struct EnterZoneSceneAfter
        {
            public Scene ZoneScene;
        }

        public struct SessionDisconnect
        {
            public SceneType SceneType;
            public Session Session;
        }

        public struct PlayerKickOut
        {
            
        }
        
        public struct LoginRealmFinish
        {
        }

        public struct LoginGateFinish
        {
        }

        public struct LoginBattleFinish
        {
            public int MapId { get; set; }
            public int RandomSeed { get; set; }
            public List<MobaPlayerInfo> Players { get; set; }
        }
    }
}