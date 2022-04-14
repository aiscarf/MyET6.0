using System;
using System.Collections.Generic;
using Scarf.ANode.Flow.Runtime;

namespace Scarf.Moba
{
    public abstract class Skill: CObject
    {
        public Unit Master { get; private set; }

        public SkillSignalSet SignalSet = new SkillSignalSet();
        public SkillData SkillData { get; private set; }
        public FlowNodeGraph SkillGraph { get; set; }

        public int Priority { get; private set; }
        public int Angle { get; protected set; }
        public int Range { get; protected set; }

        private ESkillState m_eSkillState = ESkillState.ENone;
        public ESkillState SkillState => this.m_eSkillState;
        public ESkillType SkillType => this.SkillData.SkillType;
        public int Id => this.SkillData.Id;

        public string CurAnimationName { get; set; }

        // DONE: 技能目标缓存.
        private List<Unit> m_lstSkillTargets = new List<Unit>();
        public IEnumerable<Unit> SkillTargets => this.m_lstSkillTargets;

        // DONE: 技能距离.
        public int SkillDistance { get; protected set; }

        public int SkillShapeArg1 => this.SkillData.IndicatorShapeArg1;

        public int SkillShapeArg2 => this.SkillData.IndicatorShapeArg2;

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
            this.SkillGraph.BindSkill(this);

            // DONE: 推送技能初始化事件.
            this.PushSkillEvent(nameof (SkillSignalSet.OnSkillInitSignal));
        }

        protected override void OnStart()
        {
        }

        protected override void OnDestroy()
        {
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            // DONE: 迭代技能图.
            this.SkillGraph.Tick();
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

        // TODO 不同的技能类型驱动模板.
        public void CastSkill(int angle, int range)
        {
            this.Angle = angle;
            this.Range = range;

            // DONE: 播放技能动画.
            this.Master.UnitAnimation.PlayAnimation(this.SkillData.SkillAnimationName);

            this.SkillGraph.Tick();
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

            // DONE: 当前技能是否处于释法状态, 是的话能否打断.
            if (!CanBreakSkill())
            {
                return false;
            }

            // DONE: 是否是单体指向技能
            if (this.SkillData.SkillTargetType == ESkillTargetType.ESingle)
            {
                var targets = FindTargets(angle, range);
                if (targets.Count <= 0)
                    return false;
            }

            return true;
        }

        public virtual List<Unit> FindTargets(int angle, int range)
        {
            var results = new List<Unit>();
            var units = this.Battle.BattleScene.AllUnits;
            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];

                // DONE: 处于黑名单的.
                if (this.SkillData.SkillTargetFilterBlacklist.Contains((int)unit.UnitType))
                {
                    continue;
                }

                // DONE: 技能过滤阵营类型.
                switch (this.SkillData.SkillTargetFilterType)
                {
                    case ESkillTargetFilterType.ENone:
                        break;
                    case ESkillTargetFilterType.EEnemy:
                        if (unit.Camp == this.Master.Camp)
                        {
                            continue;
                        }

                        break;
                    case ESkillTargetFilterType.EFriendly:
                        if (unit.Camp != this.Master.Camp)
                        {
                            continue;
                        }

                        break;
                    case ESkillTargetFilterType.EFriendlyNotSelf:
                        if (unit.Camp != this.Master.Camp)
                        {
                            continue;
                        }

                        if (unit.Uid == this.Master.Uid)
                        {
                            continue;
                        }

                        break;
                    case ESkillTargetFilterType.EAll:
                        break;
                    case ESkillTargetFilterType.EAllNotSelf:
                        if (unit.Uid == this.Master.Uid)
                        {
                            continue;
                        }

                        break;
                }

                // TODO 根据视野.

                // DONE: 根据技能指示器形状范围.
                switch (this.SkillData.IndicatorShapeType)
                {
                    case EIndicatorShapeType.ENone:
                        break;
                    case EIndicatorShapeType.ESector:
                        SVector3 sectorForward = (SQuaternion.AngleAxis(angle, SVector3.up) * this.Master.BornForward).normalizedXz;
                        SVector3 sectorCenter = this.Master.LogicPos + sectorForward * this.SkillDistance / 1000;
                        if (!CPhysics.CheckCircleAndSector(unit.LogicPos, unit.Radius, sectorCenter, sectorForward, this.SkillShapeArg1,
                                this.SkillShapeArg2))
                        {
                            continue;
                        }

                        break;
                    case EIndicatorShapeType.ECircle:
                        SVector3 sSkillForward = (SQuaternion.AngleAxis(angle, SVector3.up) * this.Master.BornForward).normalizedXz;
                        SVector3 sCircleCenter = this.Master.LogicPos + sSkillForward * this.SkillDistance / 1000;
                        if (!CPhysics.CheckCircleAndCircle(unit.LogicPos, unit.Radius, sCircleCenter, this.SkillShapeArg1))
                        {
                            continue;
                        }

                        break;
                    case EIndicatorShapeType.ERectangle:
                        SVector3 skillForward = (SQuaternion.AngleAxis(angle, SVector3.up) * this.Master.BornForward).normalizedXz;
                        SVector3 sCenter = this.Master.LogicPos + skillForward * this.SkillDistance / 2000;
                        if (!CPhysics.CheckRectangleAndCircle(sCenter, skillForward, this.SkillShapeArg2 / 2, SkillShapeArg1 / 2, unit.LogicPos,
                                unit.Radius))
                        {
                            continue;
                        }

                        break;
                }

                results.Add(unit);
            }

            return results;
        }

        public void BreakSkill(bool bForce = false)
        {
            // DONE: 二次验证.
            if (!bForce && !this.CanBreakSkill())
                return;

            // DONE: 打断技能图.
            this.SkillGraph.Stop();

            if (bForce)
            {
                // DONE: 推送技能被强制打断事件.
                this.PushSkillEvent(nameof (SkillSignalSet.OnSkillBeForceBreakSignal));
            }
            else
            {
                // DONE: 推送技能被打断事件.
                this.PushSkillEvent(nameof (SkillSignalSet.OnSkillBeBreakSignal));
            }
        }

        private void CastEnd()
        {
            // DONE: 预防多次技能结束
            if (this.m_eSkillState == ESkillState.ECastEnd)
                return;
            this.ChangeSkillState(ESkillState.ECastEnd);

            // DONE: 推送技能结束事件.
            this.PushSkillEvent(nameof (SkillSignalSet.OnSkillEndSignal));

            // 如果技能没有冷却功能则进入技能就绪状态.
            if (this.IsCooling)
                return;
            this.ChangeSkillState(ESkillState.EReady);
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

        public void PushAnimationEvent(string animationName, string eventName)
        {
            // TODO 动画名 + 事件名的组合.
            this.SkillGraph.Eventboard.SetEvent(eventName);
        }

        private static readonly Dictionary<string, Func<Skill, Signal<Skill>>> SkillEventTable = new Dictionary<string, Func<Skill, Signal<Skill>>>()
        {
            { nameof (SkillSignalSet.OnSkillInitSignal), (skill) => skill.SignalSet.OnSkillInitSignal },
            { nameof (SkillSignalSet.OnSkillUnsealSignal), (skill) => skill.SignalSet.OnSkillUnsealSignal },
            { nameof (SkillSignalSet.OnSkillStartSignal), (skill) => skill.SignalSet.OnSkillStartSignal },
            { nameof (SkillSignalSet.OnSkillHitSignal), (skill) => skill.SignalSet.OnSkillHitSignal },
            { nameof (SkillSignalSet.OnSkillMissileSignal), skill => skill.SignalSet.OnSkillMissileSignal },
            { nameof (SkillSignalSet.OnSkillBeBreakSignal), (skill) => skill.SignalSet.OnSkillBeBreakSignal },
            { nameof (SkillSignalSet.OnSkillBeForceBreakSignal), (skill) => skill.SignalSet.OnSkillBeForceBreakSignal },
            { nameof (SkillSignalSet.OnSkillEndSignal), (skill) => skill.SignalSet.OnSkillEndSignal },
            { nameof (SkillSignalSet.OnSkillDestroySignal), (skill) => skill.SignalSet.OnSkillDestroySignal },
            { nameof (SkillSignalSet.OnCastToggleOnSignal), (skill) => skill.SignalSet.OnCastToggleOnSignal },
            { nameof (SkillSignalSet.OnCastToggleOffSignal), (skill) => skill.SignalSet.OnCastToggleOffSignal },
            { nameof (SkillSignalSet.OnCastXuLiReleaseSignal), (skill) => skill.SignalSet.OnCastXuLiReleaseSignal },
            { nameof (SkillSignalSet.OnDuTiaoBeBreakSignal), (skill) => skill.SignalSet.OnDuTiaoBeBreakSignal },
            { nameof (SkillSignalSet.OnDuTiaoEndSignal), (skill) => skill.SignalSet.OnDuTiaoEndSignal },
            { nameof (SkillSignalSet.OnChannelThinkSignal), (skill) => skill.SignalSet.OnChannelThinkSignal },
            { nameof (SkillSignalSet.OnChannelFinishSignal), (skill) => skill.SignalSet.OnChannelFinishSignal },
        };

        protected void PushSkillEvent(string eventName)
        {
            if (!SkillEventTable.TryGetValue(eventName, out var func))
            {
                return;
            }

            func.Invoke(this).Dispatch(this);
            this.SkillGraph.Eventboard.SetEvent(eventName.Replace("Signal", ""));
        }

        #endregion
    }
}