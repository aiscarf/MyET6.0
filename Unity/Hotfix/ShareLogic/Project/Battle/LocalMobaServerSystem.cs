using System;

namespace ET
{
    public class LocalMobaServerComponentAwakeSystem : AwakeSystem<LocalMobaServerComponent>
    {
        public override void Awake(LocalMobaServerComponent self)
        {
            self.m_nNetFrameId = 0;
            self.m_curFrameData = new B2C_OnFrame();
            self.m_curFrameData.FrameId = 0;
            self.m_curFrameData.Msg.Clear();
            self.m_fLogicFrameDelta =
                self.GetParent<MobaBattleEntity>().GetComponent<FrameSyncComponent>().LOGIC_FRAME_DELTA / 1000f;
        }
    }

    public class LocalMobaServerComponentDestroySystem : DestroySystem<LocalMobaServerComponent>
    {
        public override void Destroy(LocalMobaServerComponent self)
        {
            self.m_nNetFrameId = 0;
            self.m_curFrameData.FrameId = 0;
            self.m_curFrameData.Msg.Clear();
        }
    }

    public class LocalMobaServerComponentUpdateSystem : UpdateSystem<LocalMobaServerComponent>
    {
        public override void Update(LocalMobaServerComponent self)
        {
            // TODO Time.deltaTime = 0.03f;
            for (self.m_fAccumilatedTime += 0.03f;
                 (double)self.m_fAccumilatedTime >= (double)self.m_fLogicFrameDelta;
                 self.m_fAccumilatedTime -= self.m_fLogicFrameDelta)
            {
                self.SendCurNetFrameData();
            }
        }
    }

    public static class LocalMobaServerSystem
    {
        public static void ReceiveOperation(this LocalMobaServerComponent self, long uid, C2B_FrameMsg frameMsg)
        {
            float delay = self.GetUplinkDelay();
            if (delay <= 0f)
            {
                self.InsertOperation(uid, frameMsg);
            }
            else
            {
                Action action = () => { self.InsertOperation(uid, frameMsg); };
                TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + (int)(delay * 1000),
                    TimerType.LocalDownlinkDelayTimer, action);
            }
        }

        private static void InsertOperation(this LocalMobaServerComponent self, long uid, C2B_FrameMsg frameMsg)
        {
            // 收集客户端数据
            if (self.m_curFrameData.FrameId < self.m_nNetFrameId - LocalMobaServerComponent.DELAY_FRAME_COUNT)
            {
                return;
            }

            for (int i = 0; i < self.m_curFrameData.Msg.Count; i++)
            {
                // 已经收集过了, 不要了
                if (self.m_curFrameData.Msg[i].Uid == uid)
                {
                    return;
                }
            }

            var data = new FrameMsg
                { Uid = uid, Arg1 = frameMsg.Msg.Arg1, Arg2 = frameMsg.Msg.Arg2, Optype = frameMsg.Msg.Optype };
            self.m_curFrameData.Msg.Add(data);
        }

        public static void SendCurNetFrameData(this LocalMobaServerComponent self)
        {
            B2C_OnFrame frameData = new B2C_OnFrame();
            frameData.FrameId = self.m_curFrameData.FrameId;
            for (int i = 0; i < self.m_curFrameData.Msg.Count; i++)
            {
                frameData.Msg.Add(self.m_curFrameData.Msg[i]);
            }

            float delay = self.GetDownlinkDelay();

            Action action = () =>
            {
                self.GetParent<MobaBattleEntity>().GetComponent<FrameSyncComponent>().AddLogicFrame(frameData);
            };

            if (delay <= 0f)
            {
                action.Invoke();
            }
            else
            {
                TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + (int)(delay * 1000),
                    TimerType.LocalUplinkDelayTimer, action);
            }

            ++self.m_nNetFrameId;
            self.m_curFrameData.FrameId = self.m_nNetFrameId;
            self.m_curFrameData.Msg.Clear();
        }

        public static float GetDownlinkDelay(this LocalMobaServerComponent self)
        {
            uint minDownlinkValue = 0;
            uint maxDownlinkValue = 0;

            if (minDownlinkValue <= 0f && minDownlinkValue == maxDownlinkValue)
            {
                return 0f;
            }

            if (minDownlinkValue >= maxDownlinkValue)
            {
                return minDownlinkValue / 1000f;
            }

            int nRandomValue = self.m_rand.Next((int)minDownlinkValue, (int)maxDownlinkValue);
            return nRandomValue / 1000f;
        }

        public static float GetUplinkDelay(this LocalMobaServerComponent self)
        {
            uint minUplinkValue = 0;
            uint maxUplinkValue = 1;

            if (minUplinkValue <= 0f && minUplinkValue == maxUplinkValue)
            {
                return 0f;
            }

            if (minUplinkValue >= maxUplinkValue)
            {
                return minUplinkValue / 1000f;
            }

            int nRandomValue = self.m_rand.Next((int)minUplinkValue, (int)maxUplinkValue);
            return nRandomValue / 1000f;
        }

        #region 模拟心跳包
        

        #endregion
    }

    [Timer(TimerType.LocalDownlinkDelayTimer)]
    public class LocalDownlinkDelayTimer : ATimer<Action>
    {
        public override void Run(Action self)
        {
            try
            {
                self.Invoke();
            }
            catch (Exception e)
            {
                Log.Error($"LocalDownlinkDelayTimer ERROR:\n{e}");
            }
        }
    }

    [Timer(TimerType.LocalUplinkDelayTimer)]
    public class LocalUplinkDelayTimer : ATimer<Action>
    {
        public override void Run(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Log.Error($"LocalUplinkDelayTimer ERROR:\n{e}");
            }
        }
    }
}