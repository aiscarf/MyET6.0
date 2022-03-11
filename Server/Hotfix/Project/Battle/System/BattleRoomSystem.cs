using System;

namespace ET
{
    [Timer(TimerType.BattleFrameSyncTimer)]
    public class BattleRoomFrameUpdate: ATimer<BattleRoom>
    {
        public override void Run(BattleRoom self)
        {
            try
            {
                self.FrameUpdate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    public sealed class BattleRoomAwakeSystem: AwakeSystem<BattleRoom, int, int, string, int>
    {
        public override void Awake(BattleRoom self, int a, int b, string c, int d)
        {
            self.Awake(a, b, c, d);

            // DONE: 添加游戏检测开始组件.
            self.AddComponent<BattleCheckStartComponent>().BattleRoom = self;
        }
    }

    public sealed class BattleRoomDestroySystem: DestroySystem<BattleRoom>
    {
        public override void Destroy(BattleRoom self)
        {
            LogHelper.Console(SceneType.Battle, $"房间[{self.RoomId}]已销毁.");
            self.Stop();
        }
    }

    public static class BattleRoomSystem
    {
        public static async void StartCountdownDestroy(this BattleRoom self)
        {
            self.cancellationToken = new ETCancellationToken();
            self.IsCountdownDestroy = true;

            // DONE: 一分钟后销毁该房间.
            var b = await TimerComponent.Instance.WaitAsync(60 * 1000, self.cancellationToken);
            if (!b)
            {
                self.IsCountdownDestroy = false;
                return;
            }

            self.GetParent<BattleComponent>().DestroyBattleRoom(self.RoomId);
        }

        public static void CancelCountdownDestroy(this BattleRoom self)
        {
            self.cancellationToken.Cancel();
            self.IsCountdownDestroy = false;
        }

        public static void Start(this BattleRoom self)
        {
            int frameSyncTime = 100; // 50ms 即1s20帧.
            self.m_nextFrameOpt = new B2C_OnFrame() { FrameId = 0 };

            // DONE: 游戏正式开始.
            var mobaBattleEntity = self.GetChild<MobaBattleEntity>(self.BattleId);
            mobaBattleEntity.AddComponent<FrameSyncComponent>().LOGIC_FRAME_DELTA = frameSyncTime;
            mobaBattleEntity.AddComponent<InputComponent>();
            mobaBattleEntity.m_battleProcess.Start().Coroutine();
            LogHelper.Console(SceneType.Battle, $"房间{self.RoomId}的战斗正式开始!");
            // DONE: 启动50ms的帧循环.
            self.TimerId = TimerComponent.Instance.NewRepeatedTimer(frameSyncTime, TimerType.BattleFrameSyncTimer, self);
        }

        public static void Stop(this BattleRoom self)
        {
            TimerComponent.Instance.Remove(ref self.TimerId);
        }

        public static void FrameUpdate(this BattleRoom self)
        {
            var mobaBattleEntity = self.GetChild<MobaBattleEntity>(self.BattleId);
            var frameSyncComponent = mobaBattleEntity.GetComponent<FrameSyncComponent>();
            frameSyncComponent.AddLogicFrame(self.m_nextFrameOpt);

            // DONE: 向房间所有客户端广播帧操作.
            var allFighters = self.GetAllFighters();
            foreach (var kv in allFighters)
            {
                if (!kv.Value.IsConnected)
                {
                    continue;
                }

                kv.Value.ClientSession?.Send(self.m_nextFrameOpt);
            }

            int nextFrameId = ++self.m_nCurFrameId;
            self.m_nextFrameOpt = new B2C_OnFrame() { FrameId = nextFrameId };
        }

        public static void ReceiveClientOper(this BattleRoom self, long uid, C2B_FrameMsg c2BFrameMsg)
        {
            if (c2BFrameMsg.FrameId + 5 < self.m_nCurFrameId)
                return;
            if (self.m_nextFrameOpt.Msg.Find((fMsg) => fMsg.Uid == uid) != null)
                return;
            self.m_nextFrameOpt.Msg.Add(c2BFrameMsg.Msg);
        }
    }
}