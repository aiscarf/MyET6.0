using System.Collections.Generic;

namespace Scarf.Moba
{
    public sealed class Buff: IPoolable
    {
        private int add = 0;
        private int ratio = 0;
        private int refDir;
        private int d_value = 0;
        private EAttrType attrType;
        private EAttrType setType;

        public int Uid { get; private set; }
        public BuffData BuffData { get; private set; }
        public Unit Caster { get; private set; }
        public Unit Owner { get; private set; }
        public int DeathTime { get; private set; }

        public EBuffStackType BuffStackType => this.BuffData.BuffStackType;

        private List<int> m_lstStateIds = new List<int>();

        public void Init(int uid, BuffData buffData, Unit caster, Unit owner)
        {
            this.Uid = uid;
            this.BuffData = buffData;
            this.Caster = caster;
            this.Owner = owner;

            // DONE: 判断是否永久Buff.
            long waitTime = (long)TimerFrameSys.time + buffData.Time;
            this.DeathTime = buffData.Time < 0? -1 : waitTime > int.MaxValue? int.MaxValue : (int)waitTime;
            this.m_lstStateIds.Clear();

            // DONE: 参数赋值.
            this.add = this.BuffData.ChangeAttrData.FixedNum;
            this.ratio = this.BuffData.ChangeAttrData.RatioNum;
            this.attrType = this.BuffData.ChangeAttrData.RefAttrType;
            this.setType = this.BuffData.ChangeAttrData.TgtAttrType;
            this.refDir = this.BuffData.ChangeAttrData.RefTarget;
            this.d_value = 0;
        }

        public void Execute()
        {
            // DONE: 修改属性.
            int refValue = this.refDir > 0? this.Caster.UnitAttr.GetValue(this.attrType) : this.Owner.UnitAttr.GetValue(this.attrType);
            int addValue = add + (int)((long)ratio * refValue / 1000);
            int oldValue = this.Owner.UnitAttr.GetValue(setType);
            int newValue = oldValue + addValue;
            this.Owner.UnitAttr.SetValue(setType, newValue);
            int realValue = this.Owner.UnitAttr.GetValue(setType);
            this.d_value = realValue - oldValue;

            // DONE: 修改状态.
            for (int i = 0; i < this.BuffData.ChangeStates.Count; i++)
            {
                EUnitState state = (EUnitState)this.BuffData.ChangeStates[i];
                this.m_lstStateIds.Add(this.Owner.UnitState.RegisterChangeState(state));
            }
        }

        public void End()
        {
            // DONE: 还原属性.
            int curValue = this.Owner.UnitAttr.GetValue(setType);
            curValue -= this.d_value;
            this.Owner.UnitAttr.SetValue(setType, curValue);

            // DONE: 还原状态.
            for (int i = 0; i < this.BuffData.ChangeStates.Count; i++)
            {
                EUnitState state = (EUnitState)this.BuffData.ChangeStates[i];
                var stateId = this.m_lstStateIds[i];
                this.Owner.UnitState.RemoveChangeState(state, stateId);
            }
        }

        public void Stack(Buff buff)
        {
            if (this.attrType != buff.attrType)
                return;
            if (this.setType != buff.setType)
                return;
            // TODO 叠层逻辑.
        }

        public void Reset()
        {
            this.Caster = null;
            this.Owner = null;
            this.m_lstStateIds.Clear();
        }
    }
}