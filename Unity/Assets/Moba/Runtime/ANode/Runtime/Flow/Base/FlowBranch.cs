using System;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/Base/Branch")]
    public class FlowBranch : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public bool condition;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort ifTure;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort ifFalse;

        [NonSerialized] private NodePort _nextPort;

        protected override void OnAwake()
        {
            
        }

        protected override void OnStart()
        {
            condition = this.GetInputValue<bool>(nameof(condition));
            _nextPort = this.GetOutputPort(this.condition ? nameof(ifTure) : nameof(ifFalse)).Connection;
        }

        protected override EFlowStatus OnUpdate()
        {
            if (_nextPort == null)
            {
                // DONE: 该节点完成已完成.
                return EFlowStatus.ESuccess;
            }

            return this.Flow.ExecuteNextPort(_nextPort);
        }

        protected override void OnEnd()
        {
            
        }

        protected override void OnInterrupt()
        {
            this.Flow.InterruptPort(_nextPort);
        }
    }
}