using System;
using System.Collections.Generic;

namespace Scarf.Moba
{
    public class UnitBuffComponent: CComponent
    {
        // DONE: 角色身上的所有Buff.
        private List<Buff> m_allBuffs = new List<Buff>();

        // DONE: Buff叠加逻辑. {叠加组, }
        private Dictionary<int, List<Buff>> m_buffStacks = new Dictionary<int, List<Buff>>();

        public void AddBuff(Buff buff)
        {
            // TODO 判断是否可以叠加
            // TODO 判断是否免疫.

            // TODO 相同类型时, 才能叠加Buff效果, 

            // switch (buff.BuffStackType)
            // {
            //     case EBuffStackType.EReplace:
            //         // TODO 移除上一个Buff效果.
            //         break;
            //     case EBuffStackType.EMaxTime:
            //         break;
            //     case EBuffStackType.EStack:
            //         break;
            // }
            m_allBuffs.Add(buff);

            // TODO 曝露事件出去.
            buff.Execute();
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            // DONE: 将时间到了的Buff移除.
            for (int i = 0; i < this.m_allBuffs.Count; i++)
            {
                var buff = this.m_allBuffs[i];
                if (buff.DeathTime < 0)
                    continue;
                if (TimerFrameSys.time < buff.DeathTime)
                    continue;
                i--;
                buff.End();
                this.m_allBuffs.Remove(buff);
            }
        }
    }
}