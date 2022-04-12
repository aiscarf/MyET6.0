using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/Base/Debugger")]
    public class FlowDebugger : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public string content;

        private NodePort _exitPort;
        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            content = this.GetInputValue<string>(nameof(content));
        }

        protected override EFlowStatus OnUpdate()
        {
            UnityEngine.Debug.Log(content);
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}