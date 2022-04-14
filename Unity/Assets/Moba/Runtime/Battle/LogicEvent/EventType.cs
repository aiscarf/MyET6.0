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
            public Unit unit;
            public string animationName;
            public string eventName;
        }

        public struct CreateUnit
        {
            public Unit unit;
        }
        
        public struct UnitMoveBegin
        {
            public Unit unit;
            public int speed;
        }

        public struct UnitMoveEnd
        {
            public Unit unit;
        }

        public struct UnitTargetPos
        {
            public Unit unit;
            public SVector3 targetPos;
            public int speed;
        }

        public struct UnitRotate
        {
            public Unit unit;
            public SVector3 forward;
            public bool bImmediately;
        }
    }
}