using System.Collections.Generic;

namespace Scarf.Moba
{
    public class UnitStateComponent: CComponent
    {
        public Unit Master { get; private set; }

        private int abilityState = int.MaxValue; // 1个int存储32种状态 {0: 代表禁用, 1代表启用} (默认启用)

        private Dictionary<int, List<int>> m_dicStateTimes;

        protected override void OnInit()
        {
            this.Master = this.Parent as Unit;

            this.m_dicStateTimes = new Dictionary<int, List<int>>();

            var array = System.Enum.GetValues(typeof (EUnitState));

            foreach (var stateType in array)
            {
                int key = (int)stateType;
                this.m_dicStateTimes.Add(key, new List<int>());
            }
        }

        protected override void OnDestroy()
        {
            this.m_dicStateTimes.Clear();
            this.m_dicStateTimes = null;
            this.Master = null;
        }

        public bool GetState(EUnitState eUnitState)
        {
            return ((int)eUnitState & abilityState) == (int)eUnitState;
        }

        public void SetState(EUnitState eUnitState, bool b)
        {
            bool bState = this.GetState(eUnitState);
            if (bState != b)
            {
                this.ChangeState(eUnitState, b);
            }
        }

        public int RegisterChangeState(EUnitState eUnitState)
        {
            int stateUid = this.Battle.GenerateStateId();
            RegisterStateTime(eUnitState, stateUid);
            SetState(eUnitState, false);
            return stateUid;
        }

        public void RemoveChangeState(EUnitState eUnitState, int stateUid)
        {
            RemoveStateTime(eUnitState, stateUid);
            if (IsForbidByState(eUnitState))
                return;
            this.SetState(eUnitState, true);
        }

        private void ChangeState(EUnitState eUnitState, bool b)
        {
            int mask = (int)eUnitState;
            if (b)
            {
                this.abilityState = this.abilityState | mask;
            }
            else
            {
                this.abilityState = this.abilityState & ~mask;
            }
        }

        private bool IsForbidByState(EUnitState eUnitState)
        {
            if (this.m_dicStateTimes.TryGetValue((int)eUnitState, out var list))
            {
                return list.Count > 0;
            }

            return false;
        }

        private void RegisterStateTime(EUnitState eUnitState, int uid)
        {
            if (!this.m_dicStateTimes.TryGetValue((int)eUnitState, out var list))
            {
                return;
            }

            list.Add(uid);
        }

        private void RemoveStateTime(EUnitState eUnitState, int uid)
        {
            if (!this.m_dicStateTimes.TryGetValue((int)eUnitState, out var list))
            {
                return;
            }

            list.Remove(uid);
        }
    }
}