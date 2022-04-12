using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/技能/位移/闪现位移")]
    public class FlowActionFlash : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("是否可穿墙")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public bool bCrossWall;

        private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            bCrossWall = this.GetInputValue<bool>(nameof(bCrossWall));
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}