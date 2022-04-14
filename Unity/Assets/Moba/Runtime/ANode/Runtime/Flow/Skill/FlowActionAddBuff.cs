using System;
using Scarf.Moba;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    public enum EBuffTargetType
    {
        [LabelText("对自己")]
        ESelf,

        [LabelText("技能目标")]
        ETarget,
    }

    [Serializable]
    [CreateNodeMenu("Flow/技能/Buff/添加Buff")]
    public class FlowActionAddBuff: FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("Buff目标")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public EBuffTargetType buffTargetType;

        [LabelText("BuffId")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int buffId;

        [NonSerialized]
        private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof (exit)).Connection;
        }

        protected override void OnStart()
        {
            buffId = this.GetInputValue<int>(nameof (buffId));

            // DONE: 将buff添加至目标.
            var buffData = this.Battle.BattleData.GetBuffData(this.buffId);
            var list = this.FlowNodeGraph.Skill.SkillTargets;
            foreach (Unit unit in list)
            {
                var buff = BuffFactory.CreateBuff(buffData, this.Skill.Master, unit);
                unit.UnitBuff.AddBuff(buff);
            }
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }

        protected override void OnEnd()
        {
            
        }

        protected override void OnInterrupt()
        {
            this.Flow.InterruptPort(_exitPort);
        }
    }
}