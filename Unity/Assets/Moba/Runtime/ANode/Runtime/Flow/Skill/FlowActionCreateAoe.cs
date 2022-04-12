using System;
using Scarf.Moba;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/技能/子弹/创建Aoe子弹")]
    public class FlowActionCreateAoe: FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("Aoe形状")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public EAoeShapeType shapeType;

        [LabelText("Aoe形状参数1")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int aoeShapeArg1;

        [LabelText("Aoe形状参数2")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int aoeShapeArg2;

        [LabelText("Aoe持续的时间")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int aoeTime;

        [LabelText("Aoe持续伤害段数")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int aoeDamageNum;

        [LabelText("特效名字")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int effectName;

        [LabelText("特效挂点")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public EHangPointType effectPointType;

        [LabelText("特效是否跟随")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public EAoeFollowType effectFollowType;

        [NonSerialized]
        private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof (exit)).Connection;
        }

        protected override void OnStart()
        {
            shapeType = this.GetInputValue<EAoeShapeType>(nameof (shapeType));
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}