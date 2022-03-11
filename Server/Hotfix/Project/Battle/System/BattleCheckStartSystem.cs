using System;

namespace ET
{
    [Timer(TimerType.BattleCheckReady)]
    public class BattleCheckReadyTimer: ATimer<BattleCheckStartComponent>
    {
        public override void Run(BattleCheckStartComponent self)
        {
            try
            {
                if (self.BattleRoom.IsReady == false && TimeHelper.ServerNow() < self.MaxTime)
                {
                    return;
                }

                self.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    public class BattleCheckStartComponentAwakeSystem: AwakeSystem<BattleCheckStartComponent>
    {
        public override void Awake(BattleCheckStartComponent self)
        {
            self.TimerId = TimerComponent.Instance.NewRepeatedTimer(2000, TimerType.BattleCheckReady, self);
            self.MaxTime = TimeHelper.ServerNow() + 60 * 1000;
        }
    }

    public class BattleCheckStartComponentDestroySystem: DestroySystem<BattleCheckStartComponent>
    {
        public override void Destroy(BattleCheckStartComponent self)
        {
            TimerComponent.Instance.Remove(ref self.TimerId);
            // DONE: 通知正式开始游戏.
            self.BattleRoom.Start();
        }
    }

    public static class BattleCheckStartSystem
    {

    }
}