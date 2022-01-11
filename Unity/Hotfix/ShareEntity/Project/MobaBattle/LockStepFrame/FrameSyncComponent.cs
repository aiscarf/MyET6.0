using System.Collections.Generic;

namespace ET
{
    namespace EventType
    {
        public struct StepFrame
        {
            
        }
    }
    
    public class FrameSyncComponent
    {
        public static FrameSyncComponent Instance { get; set; }
        
        public static int LOGIC_FRAME_DELTA = 50;
        public static int MAX_ACCELERATE_RATE = 8; // 一帧内最多处理8帧
        public static object sync = new object();
        
        public int m_nTime;
        public float m_fAccumilatedTime;
        public float m_fLogicFrameDelta;
        public bool m_bTimeScale;
        public float m_fTimeScale;
        public int m_nCurFrame; // 当前帧->指针
        public int m_nNetFrame; // 网络帧
        public bool m_bRunning;
        public Dictionary<int, s2c_onFrame> m_dicFrameData;
        public int m_nLastOffsetFrame;

        #region 所有接口

        public List<IStepFrame> m_allEvents;

        #endregion
    }
}