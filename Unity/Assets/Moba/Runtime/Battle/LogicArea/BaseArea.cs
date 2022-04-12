using System.Collections.Generic;

namespace Scarf.Moba
{
    public class BaseArea: CObject
    {
        private bool m_bEnable;

        protected List<Unit> m_lstUnits = new List<Unit>();

        public bool IsEnable => this.m_bEnable;

        public EAreaType AreaType { get; private set; }

        public int Id { get; private set; }

        public ECamp Camp { get; private set; }

        public SVector3 LogicPos { get; private set; }
        public SVector3 LogicForward { get; private set; }

        public void SetEnable(bool b)
        {
            if (m_bEnable == b)
                return;
            m_bEnable = b;
            OnEnableChange(b);
            // TODO 发信号出去.
        }

        protected virtual void OnEnableChange(bool b)
        {
        }

        public void UnitEnter(Unit cUnit)
        {
            if (this.m_lstUnits.Contains(cUnit))
                return;
            this.m_lstUnits.Add(cUnit);
            this.OnUnitEnter(cUnit);
            // TODO 发信号出去.
        }

        public void UnitLeave(Unit cUnit)
        {
            if (!this.m_lstUnits.Contains(cUnit))
                return;
            if (!this.m_lstUnits.Remove(cUnit))
                return;
            this.OnUnitLeave(cUnit);
            // TODO 发信号出去.
        }

        protected virtual void OnUnitEnter(Unit cUnit)
        {
        }

        protected virtual void OnUnitLeave(Unit cUnit)
        {
        }

        public virtual bool IsIncludePoint(Unit cUnit)
        {
            // TODO 判断该玩家是否处于该地形.
            return false;
        }
    }
}