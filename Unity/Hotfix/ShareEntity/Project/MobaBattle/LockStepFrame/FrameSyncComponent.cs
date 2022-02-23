using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        public struct StepFrame
        {
            
        }
    }
    
    public class FrameSyncComponent : Entity
    {
        public int LOGIC_FRAME_DELTA = 50; // 可以修改帧率, 比赛时使用的帧率会更高.
        public int MAX_ACCELERATE_RATE = 8; // 一帧内最多处理8帧
        public object sync = new object();
        public int m_nTime;
        public float m_fAccumilatedTime;
        public float m_fLogicFrameDelta;
        public bool m_bTimeScale;
        public float m_fTimeScale;
        public int m_nCurFrame; // 当前帧->指针
        public int m_nNetFrame; // 网络帧
        public bool m_bRunning;
        public Dictionary<int, B2C_OnFrame> m_dicFrameData;
        public int m_nLastOffsetFrame;
        
        #region 所有接口

        public List<IStepFrame> m_allEvents;

        #endregion
    }
}