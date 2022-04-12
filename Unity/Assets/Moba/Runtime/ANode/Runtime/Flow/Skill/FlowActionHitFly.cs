using System;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    public enum EHitFlyDir
    {
        ENone,
        [LabelText("施法者面向")] ECasterFaceDir,
        [LabelText("击飞点圆心到击飞目标连线")] EFlyCenterFlyTargetDir,
        [LabelText("技能设定的指定方向")] ESkillDir,
    }

    [CreateNodeMenu("Flow/技能/效果/击飞效果")]
    public class FlowActionHitFly : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("击飞高度")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int flyHeight;

        [LabelText("击飞距离")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int flyDistance;

        [LabelText("击飞时间")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int flyTime;

        [LabelText("击飞方向")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public EHitFlyDir flyDir;

        [NonSerialized] private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            flyHeight = this.GetInputValue<int>(nameof(flyHeight));
            flyDistance = this.GetInputValue<int>(nameof(flyDistance));
            flyTime = this.GetInputValue<int>(nameof(flyTime));
            flyDir = this.GetInputValue<EHitFlyDir>(nameof(flyDir));

            // TODO 实现击飞效果.
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}