using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class FrameSyncComponentAwakeSystem : AwakeSystem<FrameSyncComponent>
    {
        public override void Awake(FrameSyncComponent self)
        {
            FrameSyncComponent.Instance = self;
            self.m_nTime = 0;
            self.m_fAccumilatedTime = 0.0f;
            self.m_nCurFrame = 0;
            self.m_nNetFrame = 0;
            self.m_fTimeScale = 1f;
            self.m_fLogicFrameDelta = (float)FrameSyncComponent.LOGIC_FRAME_DELTA / (self.m_fTimeScale * 1000f);
            self.m_bTimeScale = false;
            self.m_bRunning = true;
            self.m_dicFrameData = new Dictionary<int, s2c_onFrame>(10000);
            
            // TODO 收集所有事件.
            self.m_allEvents = new List<IStepFrame>();

            HashSet<Type> types = Game.EventSystem.GetTypes(typeof(StepFrameAttribute));
            foreach (Type type in types)
            {
                IStepFrame obj = (IStepFrame)Activator.CreateInstance(type);
                if (self.m_allEvents.Contains(obj))
                    continue;
                self.m_allEvents.Add(obj);
            }
        }
    }

    [ObjectSystem]
    public class FrameSyncComponentDestroySystem : DestroySystem<FrameSyncComponent>
    {
        public override void Destroy(FrameSyncComponent self)
        {
            FrameSyncComponent.Instance = null;
            self.m_bRunning = false;
            self.m_nTime = 0;
            self.m_nCurFrame = 0;
            self.m_nNetFrame = 0;
            self.m_dicFrameData.Clear();
        }
    }

    [ObjectSystem]
    public class FrameSyncComponentUpdateSystem : UpdateSystem<FrameSyncComponent>
    {
        public override void Update(FrameSyncComponent self)
        {
            if (!self.m_bRunning)
                return;

            int offset = (self.m_nNetFrame - self.m_nCurFrame);
            int loop = offset < FrameSyncComponent.MAX_ACCELERATE_RATE
                ? offset
                : FrameSyncComponent.MAX_ACCELERATE_RATE;

            if (offset != self.m_nLastOffsetFrame)
            {
                self.m_nLastOffsetFrame = offset;

                // TODO 推送.
            }

            while (loop-- > 0)
            {
                // TODO 收集输入.
                // InputSys.instance.CollectInput();

                var data = self.GetLogicFrame(this.CurFrame);
                if (data == null)
                    return;

                // TODO 跑一帧数据.
                self.RunOneFrame(data);
            }
        }
    }

    public static class FrameSyncSystem
    {
        public static void DispatchStepFrame(this FrameSyncComponent self)
        {
            foreach (IStepFrame stepFrame in self.m_allEvents)
            {
                stepFrame.OnStepFrame();
            }
        }
        
        public static void AddLogicFrame(this FrameSyncComponent self, s2c_onFrame frameData)
        {
            lock (FrameSyncComponent.sync)
            {
                if (self.HasLogicFrame(frameData.FrameId))
                {
                    return;
                }

                if (frameData.FrameId > self.m_nNetFrame)
                {
                    self.m_nNetFrame = frameData.FrameId;
                }

                self.m_dicFrameData.Add(frameData.FrameId, frameData);
            }
        }

        public static s2c_onFrame GetLogicFrame(this FrameSyncComponent self, int frameId)
        {
            lock (FrameSyncComponent.sync)
            {
                if (!self.HasLogicFrame(frameId))
                {
                    return null;
                }

                return self.m_dicFrameData[frameId];
            }
        }

        public static bool HasLogicFrame(this FrameSyncComponent self, int frameId)
        {
            lock (FrameSyncComponent.sync)
            {
                return self.m_dicFrameData.ContainsKey(frameId);
            }
        }

        public static void RunOneFrame(this FrameSyncComponent self, s2c_onFrame data)
        {
            self.m_nTime += FrameSyncComponent.LOGIC_FRAME_DELTA;
            
            // TODO 分发一帧的操作数据下去.
            // InputSys.instance.HandleFrameData(frameData);
            
            // 所有IStepFrame调度一次
            self.DispatchStepFrame();
            // 步进一帧
            ++self.m_nCurFrame;
        }

        public static void StartTimeScale(this FrameSyncComponent self, float timeScale)
        {
            self.m_fTimeScale = timeScale;
            self.m_fLogicFrameDelta = (float)FrameSyncComponent.LOGIC_FRAME_DELTA / (self.m_fTimeScale * 1000f);
            self.m_fAccumilatedTime = 0.0f;
            self.m_bTimeScale = true;
        }

        public static void StopTimeScale(this FrameSyncComponent self)
        {
            if (!self.m_bTimeScale)
                return;
            self.m_bTimeScale = false;
            self.m_fAccumilatedTime = 0.0f;
            self.m_fTimeScale = 1f;
            self.m_fLogicFrameDelta = (float)FrameSyncComponent.LOGIC_FRAME_DELTA / (self.m_fTimeScale * 1000f);
        }

        public static void StopStep(this FrameSyncComponent self)
        {
            self.m_bRunning = false;
        }

        public static void ResumeStep(this FrameSyncComponent self)
        {
            self.m_bRunning = true;
        }
    }
}