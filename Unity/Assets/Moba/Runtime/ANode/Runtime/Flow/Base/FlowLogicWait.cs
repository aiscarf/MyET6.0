using System;
using Scarf.Moba;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/Base/LogicWait")]
    public class FlowLogicWait: FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("等待时间ms")]
        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int time;

        [NonSerialized]
        private NodePort _exitPort;

        private int timeEnd;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof (exit)).Connection;
        }

        protected override void OnStart()
        {
            time = this.GetInputValue<int>(nameof (time));
            timeEnd = TimerFrameSys.time + time;
        }

        protected override EFlowStatus OnUpdate()
        {
            if (TimerFrameSys.time < timeEnd)
            {
                return EFlowStatus.ERunning;
            }

            return this.Flow.ExecuteNextPort(this._exitPort);
        }
    }
}