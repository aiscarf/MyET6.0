using System;
using System.Collections.Generic;

namespace Scarf.Moba
{
    public class TimerFrameSys
    {
        private int mTimerId = 0;

        public static int time;
        public static int DELTA_TIME;
        public void Init()
        {
            TimerFrameSys.time = 0;
            this.mTimerId = 0;
            this.m_lstAddTimerDatas.Clear();
            this.m_lstTimers.Clear();
            this.m_lstRemoveDatas.Clear();
            CObjectPool<TimerData>.instance.Init(20);

            this.m_lstAddIntervalTimerDatas.Clear();
            this.m_lstIntervalTimerDatas.Clear();
            this.m_lstRemoveIntervalTimerDatas.Clear();
            CObjectPool<IntervalTimerData>.instance.Init(20);
        }

        public void Clear()
        {
            TimerFrameSys.time = 0;
            this.mTimerId = 0;
            this.m_lstAddTimerDatas.Clear();
            this.m_lstTimers.Clear();
            this.m_lstRemoveDatas.Clear();
            CObjectPool<TimerData>.instance.Clear();

            this.m_lstAddIntervalTimerDatas.Clear();
            this.m_lstIntervalTimerDatas.Clear();
            this.m_lstRemoveIntervalTimerDatas.Clear();
            CObjectPool<IntervalTimerData>.instance.Clear();
        }

        public void OnLogicFrame(int delta)
        {
            DELTA_TIME = delta;
            TimerFrameSys.time += delta;
            DelayTimerUpdate(delta);
            IntervalTimerUpdate(delta);
        }

        #region 延时器

        private List<TimerData> m_lstAddTimerDatas = new List<TimerData>();
        private List<TimerData> m_lstTimers = new List<TimerData>();
        private List<TimerData> m_lstRemoveDatas = new List<TimerData>();

        public int AddTimerAction(int wait, Action action, Action<int, int> progressAction = null)
        {
            ++this.mTimerId;
            var timerData = CObjectPool<TimerData>.instance.GetObject();
            timerData.Init(this.mTimerId, wait, action, progressAction);
            this.m_lstAddTimerDatas.Add(timerData);
            return this.mTimerId;
        }

        public bool RemoveTimerAction(int id)
        {
            for (int i = 0; i < this.m_lstRemoveDatas.Count; i++)
            {
                if (this.m_lstRemoveDatas[i].Id == id)
                {
                    return true;
                }
            }

            for (int i = 0; i < this.m_lstAddTimerDatas.Count; i++)
            {
                if (this.m_lstAddTimerDatas[i].Id == id)
                {
                    this.m_lstAddTimerDatas.RemoveAt(i);
                    return true;
                }
            }

            for (int i = 0; i < this.m_lstTimers.Count; i++)
            {
                if (this.m_lstTimers[i].Id == id)
                {
                    this.m_lstRemoveDatas.Add(this.m_lstTimers[i]);
                    return true;
                }
            }

            return false;
        }

        void DelayTimerUpdate(int delta)
        {
            for (int i = 0; i < this.m_lstAddTimerDatas.Count; i++)
            {
                this.m_lstTimers.Add(this.m_lstAddTimerDatas[i]);
            }

            this.m_lstAddTimerDatas.Clear();

            for (int i = 0; i < this.m_lstRemoveDatas.Count; i++)
            {
                this.m_lstTimers.Remove(this.m_lstRemoveDatas[i]);
                CObjectPool<TimerData>.instance.SaveObject(this.m_lstRemoveDatas[i]);
            }

            this.m_lstRemoveDatas.Clear();

            for (int i = 0; i < this.m_lstTimers.Count; i++)
            {
                var temp = this.m_lstTimers[i];
                if (temp.IsComplete)
                {
                    this.m_lstRemoveDatas.Add(temp);
                    continue;
                }

                temp.OnLogicFrame(delta);
            }
        }

        #endregion

        #region 定时器

        private List<IntervalTimerData> m_lstAddIntervalTimerDatas = new List<IntervalTimerData>();
        private List<IntervalTimerData> m_lstIntervalTimerDatas = new List<IntervalTimerData>();
        private List<IntervalTimerData> m_lstRemoveIntervalTimerDatas = new List<IntervalTimerData>();

        public int AddIntervalTimer(int interval, Action action, int loopCount = -1)
        {
            ++mTimerId;
            var intervalTimerData = CObjectPool<IntervalTimerData>.instance.GetObject();
            intervalTimerData.Init(mTimerId, interval, action, loopCount);
            this.m_lstAddIntervalTimerDatas.Add(intervalTimerData);
            return mTimerId;
        }

        public bool RemoveIntervalTimerAction(int id)
        {
            for (int i = 0; i < this.m_lstRemoveIntervalTimerDatas.Count; i++)
            {
                if (this.m_lstRemoveIntervalTimerDatas[i].Id == id)
                {
                    return true;
                }
            }

            for (int i = 0; i < this.m_lstAddIntervalTimerDatas.Count; i++)
            {
                if (this.m_lstAddIntervalTimerDatas[i].Id == id)
                {
                    this.m_lstAddIntervalTimerDatas.RemoveAt(i);
                    return true;
                }
            }

            for (int i = 0; i < this.m_lstIntervalTimerDatas.Count; i++)
            {
                if (this.m_lstIntervalTimerDatas[i].Id == id)
                {
                    this.m_lstRemoveIntervalTimerDatas.Add(this.m_lstIntervalTimerDatas[i]);
                    return true;
                }
            }

            return false;
        }

        void IntervalTimerUpdate(int delta)
        {
            for (int i = 0; i < this.m_lstAddIntervalTimerDatas.Count; i++)
            {
                this.m_lstIntervalTimerDatas.Add(this.m_lstAddIntervalTimerDatas[i]);
            }

            this.m_lstAddIntervalTimerDatas.Clear();

            for (int i = 0; i < this.m_lstRemoveIntervalTimerDatas.Count; i++)
            {
                this.m_lstIntervalTimerDatas.Remove(this.m_lstRemoveIntervalTimerDatas[i]);
                CObjectPool<IntervalTimerData>.instance.SaveObject(this.m_lstRemoveIntervalTimerDatas[i]);
            }

            this.m_lstRemoveIntervalTimerDatas.Clear();

            for (int i = 0; i < this.m_lstIntervalTimerDatas.Count; i++)
            {
                var temp = this.m_lstIntervalTimerDatas[i];
                if (temp.IsComplete)
                {
                    this.m_lstRemoveIntervalTimerDatas.Add(temp);
                    continue;
                }

                temp.OnLogicFrame(delta);
            }
        }

        #endregion

        private class TimerData: IPoolable
        {
            private int m_cStartTime;
            private long m_nEndTime;
            private int m_nWaitTime;
            private Action m_cAction;
            private Action<int, int> m_cProgressAction;

            public int Id { get; private set; }

            public bool IsComplete { get; private set; }

            public void Init(int id, int waitTime, Action action, Action<int, int> progressAction)
            {
                this.Id = id;
                this.m_nWaitTime = waitTime;
                this.m_cStartTime = TimerFrameSys.time;
                this.m_nEndTime = (long)TimerFrameSys.time + waitTime;
                this.m_cAction = action;
                this.m_cProgressAction = progressAction;
                this.IsComplete = false;
            }

            public void OnLogicFrame(int delta)
            {
                if (IsComplete)
                    return;

                if (this.m_cProgressAction != null)
                {
                    this.m_cProgressAction.Invoke(TimerFrameSys.time - this.m_cStartTime,
                        this.m_nWaitTime);
                }

                if (TimerFrameSys.time >= this.m_nEndTime)
                {
                    IsComplete = true;
                    this.m_cAction.Invoke();
                }
            }

            public void Reset()
            {
                this.Id = 0;
                this.IsComplete = false;
                this.m_nEndTime = 0;
                this.m_cAction = null;
                this.m_cProgressAction = null;
            }
        }

        private class IntervalTimerData: IPoolable
        {
            private int m_nStartTime;
            private int m_nIntervalTime;
            private Action m_cAction;
            private int m_nLoopCount;

            private int m_nTimer;

            public int Id { get; private set; }
            public bool IsComplete { get; private set; }

            public void Init(int uid, int intervalTime, Action action, int loopCount)
            {
                this.Id = uid;
                this.m_nIntervalTime = intervalTime;
                this.m_cAction = action;
                this.m_nLoopCount = loopCount < 0? int.MaxValue : loopCount;
                this.m_nStartTime = TimerFrameSys.time;

                this.m_nTimer = this.m_nIntervalTime;
                this.IsComplete = false;
            }

            public void OnLogicFrame(int delta)
            {
                if (IsComplete)
                    return;
                this.m_nTimer -= delta;
                if (this.m_nTimer > 0)
                    return;
                this.m_nTimer = this.m_nIntervalTime;
                this.m_cAction.Invoke();
                --this.m_nLoopCount;

                if (this.m_nLoopCount <= 0)
                {
                    this.IsComplete = false;
                }
            }

            public void Reset()
            {
                this.Id = 0;
                this.m_nStartTime = 0;
                this.m_nIntervalTime = 0;
                this.m_cAction = null;
                this.m_nTimer = 0;
                this.m_nLoopCount = 0;
                this.IsComplete = false;
            }
        }
    }
}