namespace Scarf.Moba
{
    namespace EventType
    {
        // 战斗开始
        public struct BattleStart
        {
        }

        // 战斗结束
        public struct BattleEnd
        {
        }

        public struct AnimationEvent
        {
            public Unit EventUnit;
            public string AnimationName;
            public string EventName;
        }
    }
}