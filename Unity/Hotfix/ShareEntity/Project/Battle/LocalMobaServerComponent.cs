using System;

namespace ET
{
    public class LocalMobaServerComponent : Entity
    {
        public float m_fAccumilatedTime;
        public float m_fLogicFrameDelta;
        public int m_nNetFrameId;

        public const int DELAY_FRAME_COUNT = 40;

        public B2C_OnFrame m_curFrameData;
        public Random m_rand = new Random();
        public Action m_clientHeartBeatCallback;
    }
}