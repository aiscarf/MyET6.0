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

        public struct LeaveZoneScene
        {
            public Scene ZoneScene;
        }

        public struct EnterZoneSceneBefore
        {
            public Scene ZoneScene;
        }

        public struct EnterZoneSceneAfter
        {
            public Scene ZoneScene;
        }

        public struct LoginFinish
        {
            
        }
    }
}