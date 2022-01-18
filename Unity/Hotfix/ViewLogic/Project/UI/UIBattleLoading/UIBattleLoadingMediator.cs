using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIBattleLoadingMediator : UIMediator<UIBattleLoadingComponent>
    {
        public override void OnInit()
        {
            self.m_sliderProgress =
                referenceCollector.Get<GameObject>("Slider").GetComponent<Slider>();
        }

        public override void OnDestroy()
        {
        }

        public override void OnOpen(object data)
        {
            this.self.m_testTimer = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.TestTimer, this.self);
        }

        public override void OnClose()
        {
            TimerComponent.Instance.Remove(ref this.self.m_testTimer);
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
        }
    }
    
    [Timer(TimerType.TestTimer)]
    public class TestTimer: ATimer<UIBattleLoadingComponent>
    {
        public override void Run(UIBattleLoadingComponent self)
        {
            try
            {
                Log.Debug("每隔1s打印一次, 该功能进行测试");
            }
            catch (Exception e)
            {
                Log.Error($"TestTimer error: {self.Id}\n{e}");
            }
        }
    }
}