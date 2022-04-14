using System;
using System.Collections.Generic;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/Base/Sequence")]
    public class FlowSequence: FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)]
        public ControlPort exit;

        [NonSerialized]
        private List<NodePort> _lstPorts = new List<NodePort>();

        [NonSerialized]
        private int _index;

        private bool _bIsSuspend;

        protected override void OnAwake()
        {
            _lstPorts = this.GetOutputPort(nameof (exit)).GetConnections();
        }

        protected override void OnStart()
        {
            _index = 0;
            _bIsSuspend = false;
        }

        protected override EFlowStatus OnUpdate()
        {
            for (int i = _index; i < _lstPorts.Count; i++)
            {
                var childStatus = this.Flow.ExecuteNextPort(_lstPorts[i]);
                if (childStatus == EFlowStatus.ESuccess)
                {
                    continue;
                }

                if (childStatus == EFlowStatus.EFailure || childStatus == EFlowStatus.EInactive)
                {
                    return EFlowStatus.EFailure;
                }

                if (childStatus == EFlowStatus.ERunning)
                {
                    // DONE: 记录中断时的索引, 当恢复执行时继续执行下一个节点.
                    _index = i;
                    _bIsSuspend = true;
                    return EFlowStatus.ERunning;
                }
            }

            return EFlowStatus.ESuccess;
        }

        protected override void OnEnd()
        {
            _index = 0;
            _bIsSuspend = false;
        }

        protected override void OnInterrupt()
        {
            if (!_bIsSuspend)
                return;
            this.Flow.InterruptPort(_lstPorts[_index]);
        }
    }
}