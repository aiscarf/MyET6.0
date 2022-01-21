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
            public Scene ZoneScene;
        }

        #region Moba事件流

        /// <summary>
        /// 开始加载moba场景资源
        /// </summary>
        public struct EnterMobaBegin
        {
            public MobaBattleLoadData MobaBattleLoadData;
        }

        // 结束加载moba场景资源
        public struct EnterMobaFinish
        {
            public MobaBattleLoadData MobaBattleLoadData;
        }
        
        #endregion
    }
}