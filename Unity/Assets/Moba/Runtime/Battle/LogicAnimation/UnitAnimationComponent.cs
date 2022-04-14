using System;
using System.Collections.Generic;

namespace Scarf.Moba
{
    public class UnitAnimationComponent: CComponent
    {
        public Unit Master { get; private set; }

        private Dictionary<string, AnimationData> m_dicAnimationDatas;

        private string curAniname;

        private int curTime;

        private AnimationData curClip;

        private int curClipIndex;

        #region 生命周期

        protected override void OnInit()
        {
            Master = this.Parent as Unit;
            m_dicAnimationDatas = new Dictionary<string, AnimationData>();

            // DONE: 查询所有数据.
            var list = this.Battle.BattleData.GetAnimationDatas(this.Master.TemplateId);
            for (int i = 0; i < list.Count; i++)
            {
                var animationData = list[i];
                this.m_dicAnimationDatas.Add(animationData.AnimationName, animationData);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnDestroy()
        {
            m_dicAnimationDatas.Clear();
            m_dicAnimationDatas = null;
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            if (curClip == null || curClip.AnimationEvents == null)
            {
                return;
            }

            curTime += delta;

            OnTick();
        }

        #endregion

        #region 播放动画

        public void PlayAnimation(string name)
        {
            if (!m_dicAnimationDatas.ContainsKey(name))
            {
                return;
            }

            // DONE: 不用判断当前的动画是否能打断, 被动执行.
            this.curAniname = name;
            this.curClip = this.m_dicAnimationDatas[name];
            this.curTime = 0;
            this.curClipIndex = 0;

            OnTick();
        }

        private void OnTick()
        {
            while (curClipIndex < curClip.AnimationEvents.Count)
            {
                var animationEvent = curClip.AnimationEvents[curClipIndex];
                if (curTime < animationEvent.Time)
                {
                    return;
                }

                // DONE: 曝露动画事件.
                this.Battle.EventMgr.Publish(new EventType.AnimationEvent()
                {
                    animationName = this.curAniname, eventName = animationEvent.Name, unit = this.Master
                });

                ++curClipIndex;

                if (curClipIndex >= curClip.AnimationEvents.Count)
                {
                    curAniname = String.Empty;
                    curClip = null;
                    return;
                }
            }
        }

        #endregion
    }
}