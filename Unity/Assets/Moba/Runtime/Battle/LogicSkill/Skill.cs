using System.Collections.Generic;
using Scarf.ANode.Flow.Runtime;

namespace Scarf.Moba
{
    public sealed class Skill: CObject
    {
        public Unit Master { get; private set; }
        public SkillData SkillData { get; private set; }

        public FlowNodeGraph SkillGraph { get; set; }

        // TODO 技能优先级, 魔化凯的2技能不能连续使用, 但是可以搭配大招使用.
        public int Priority { get; private set; }
        public int Angle { get; protected set; }
        public int Range { get; protected set; }

        private ESkillState m_eSkillState = ESkillState.ENone;
        public ESkillType SkillType => this.SkillData.SkillType;

        public int Id => this.SkillData.Id;

        // DONE: 技能目标缓存.
        private List<Unit> m_lstSkillTargets = new List<Unit>();
        public IEnumerable<Unit> SkillTargets => this.m_lstSkillTargets;

        public Skill(SkillData skillData)
        {
            this.SkillData = skillData;
            // DONE: 查表获取技能图.
            this.SkillGraph = this.Battle.BattleData.GetFlowNodeGraph(this.Id);
            this.SkillCoolBase = skillData.SkillCD;
        }

        #region 生命周期

        protected override void OnInit()
        {
            this.SkillGraph.Init();
        }

        protected override void OnStart()
        {
        }

        protected override void OnDestroy()
        {
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            // TODO 迭代技能图.
            // TODO 迭代动画线.
        }

        #endregion

        #region 技能冷却

        private int logicCoolCastTime; // 技能冷却时长

        public bool IsCooling => this.m_bIsCooling;

        private bool m_bIsCooling;

        private int m_nSkillCooling;
        public int SkillCooling => m_nSkillCooling;

        private int m_nSkillCoolBase;

        public int SkillCoolBase
        {
            get => this.m_nSkillCoolBase;
            protected set
            {
                this.m_nSkillCoolBase = value;
                this.ReCalCooling();
            }
        }

        private int m_nSkillCoolRatio;

        public int SkillCoolRatio
        {
            get => this.m_nSkillCoolRatio;
            set
            {
                this.m_nSkillCoolRatio = value;
                this.ReCalCooling();
            }
        }

        private int m_nSkillCoolAdd;

        public int SkillCoolAdd
        {
            get => this.m_nSkillCoolAdd;
            set
            {
                this.m_nSkillCoolAdd = value;
                this.ReCalCooling();
            }
        }

        void ReCalCooling()
        {
            int lastSkillCooling = this.m_nSkillCooling;
            this.m_nSkillCooling = SkillCoolBase * (1000 + SkillCoolRatio) / 1000 + this.m_nSkillCoolAdd;

            // 冷却重新计算, 需要实时更新冷却时间
            if (this.m_nSkillCooling == 0)
            {
                this.logicCoolCastTime = 0;
            }
            else if (TimerFrameSys.time < this.logicCoolCastTime)
            {
                this.logicCoolCastTime = this.logicCoolCastTime * lastSkillCooling / this.m_nSkillCooling;
            }
        }

        public void ReduceSkillCoolTemp(int cool)
        {
            this.logicCoolCastTime -= cool;
        }

        public void ResetSkillCool()
        {
            this.logicCoolCastTime = 0;
        }

        private int intervalTimerId = -1;

        protected internal void EnterSkillCooling()
        {
            this.logicCoolCastTime = TimerFrameSys.time + this.SkillCooling;
            if (this.SkillCooling <= 0)
                return;
            this.m_bIsCooling = true;
            this.intervalTimerId = this.Battle.TimerFrameSys.AddIntervalTimer(0, UpdateSkillCooling);
        }

        void LeaveSkillCooling()
        {
            if (this.intervalTimerId > 0)
            {
                this.Battle.TimerFrameSys.RemoveIntervalTimerAction(this.intervalTimerId);
            }

            this.intervalTimerId = -1;

            this.m_bIsCooling = false;

            if (this.m_eSkillState != ESkillState.ECastEnd)
                return;
            ChangeSkillState(ESkillState.EReady);
        }

        void UpdateSkillCooling()
        {
            // 技能冷却更新
            int dValue = this.logicCoolCastTime - TimerFrameSys.time;
            if (dValue < 0)
            {
                this.LeaveSkillCooling();
                return;
            }

            // TODO 通知技能冷却更新.
        }

        private void ChangeSkillState(ESkillState eSkillState)
        {
            this.m_eSkillState = eSkillState;
        }

        #endregion

        #region 技能释放

        public void CastSkill(int angle, int range)
        {
            this.Angle = angle;
            this.Range = range;

            // TODO 释放此次技能, 但上一次的技能还未结束, 如何解决Flow冲突.
            // TODO 释放技能, 先待命技能图, 再播放技能动画驱动, 技能动画播放完毕, 便是技能结束.
            
            // TODO 不同的技能类型驱动模板.
        }

        public bool CanCastSkill(int angle, int range)
        {
            if (this.Master.IsDie)
            {
                return false;
            }

            // DONE: 尚未过冷却时长
            if (IsCooling)
            {
                return false;
            }

            // 当前技能是否处于释法状态, 如果是能否打断.
            if (!CanBreakSkill())
            {
                return false;
            }

            // 是否是指向技能
            return true;
        }

        public void BreakSkill(bool bForce = false)
        {
            // DONE: 二次验证.
            if (!bForce && !this.CanBreakSkill())
                return;
            // TODO 打断技能 
        }

        public bool CanBreakSkill()
        {
            bool result = true;
            // switch (this.m_eSkillState)
            // {
            //     case ESkillState.EForwardSwing:
            //         result = CanBreakInForwardSwing();
            //         break;
            //     case ESkillState.ECasting:
            //         result = CanBreakInCasting();
            //         break;
            //     case ESkillState.EBackSwing:
            //         result = CanBreakInBackSwing();
            //         break;
            // }
            return result;
        }

        #endregion
    }
}