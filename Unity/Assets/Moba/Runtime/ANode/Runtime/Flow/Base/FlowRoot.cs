using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/Base/Root")]
    public class FlowRoot: FlowNode
    {
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof (exit)).Connection;
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}