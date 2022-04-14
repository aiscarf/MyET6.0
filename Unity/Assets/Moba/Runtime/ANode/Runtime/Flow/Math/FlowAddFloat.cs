using System;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [Serializable]
    [CreateNodeMenu("Flow/Math/FlowAddFloat")]
    public class FlowAddFloat : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public float a;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public float b;

        [Output(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public float c;

        [NonSerialized] private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            a = this.GetInputValue<float>(nameof(a));
            b = this.GetInputValue<float>(nameof(b));

            c = a + b;
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(this.c))
            {
                return (object)c;
            }

            return null;
        }

        protected override void OnEnd()
        {
            
        }

        protected override void OnInterrupt()
        {
            this.Flow.InterruptPort(this._exitPort);
        }
    }
}