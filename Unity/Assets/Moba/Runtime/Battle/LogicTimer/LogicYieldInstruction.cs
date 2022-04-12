using System;

namespace Scarf.Moba
{
    public abstract class LogicYieldInstruction
    {
        protected abstract bool keepWaiting { get; }
        protected abstract void ReUse();

        public bool MoveNext()
        {
            bool b = this.keepWaiting;
            if (!b)
            {
                this.ReUse();
            }

            return b;
        }
    }

    public sealed class WaitLogicTimer: LogicYieldInstruction
    {
        private long m_nTimer;
        private int m_nWaitTime;

        public WaitLogicTimer(int waitTime)
        {
            // 增加容错, waitTime <= 0, 会造成死循环.
            if (waitTime <= 0)
            {
                waitTime = 1;
            }

            m_nWaitTime = waitTime;
            m_nTimer = TimerFrameSys.time + waitTime;
        }

        protected override bool keepWaiting
        {
            get
            {
                return TimerFrameSys.time < m_nTimer;
            }
        }

        protected override void ReUse()
        {
            m_nTimer = TimerFrameSys.time + m_nWaitTime;
        }
    }

    public sealed class WaitLogicUntil: LogicYieldInstruction
    {
        private Func<bool> m_Predicate;

        public WaitLogicUntil(Func<bool> predicate)
        {
            this.m_Predicate = predicate;
        }

        protected override bool keepWaiting
        {
            get
            {
                return !this.m_Predicate();
            }
        }

        protected override void ReUse()
        {
        }
    }
}