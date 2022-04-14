using System;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/Base/For")]
    public class FlowFor: FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int firstIndex;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int lastIndex;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int step;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort body;

        [NonSerialized]
        private NodePort _exitPort;

        [NonSerialized]
        private NodePort _bodyPort;

        [NonSerialized]
        private int recordIndex;

        private bool bIsSuspend;
        
        protected override void OnAwake()
        {
            this._exitPort = this.GetOutputPort(nameof (exit)).Connection;
            this._bodyPort = this.GetOutputPort(nameof (_bodyPort)).Connection;
        }

        protected override void OnStart()
        {
            this.firstIndex = this.GetInputValue<int>(nameof (firstIndex));
            this.lastIndex = this.GetInputValue<int>(nameof (lastIndex));
            this.step = this.GetInputValue<int>(nameof (step));

            this.recordIndex = this.firstIndex;
        }

        protected override EFlowStatus OnUpdate()
        {
            bIsSuspend = false;
            for (int i = recordIndex; i < lastIndex; i += step)
            {
                var bodyStatus = this.Flow.ExecuteNextPort(_bodyPort);

                // DONE: 只有遇到挂起状态需要特殊处理.
                if (bodyStatus == EFlowStatus.ERunning)
                {
                    recordIndex = i;
                    bIsSuspend = true;
                    return EFlowStatus.ERunning;
                }
            }

            return this.Flow.ExecuteNextPort(_exitPort);
        }

        protected override void OnEnd()
        {
            
        }

        protected override void OnInterrupt()
        {
            if (bIsSuspend)
            {
                this.Flow.InterruptPort(_bodyPort);
            }
            
            this.Flow.InterruptPort(_exitPort);
        }
    }
}