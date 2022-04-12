using System;
using System.Collections.Generic;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    /// <summary>
    /// 并行流节点将同时运行所有子节点.
    /// 当并行流节点的所有子节点都返回成功时, 并行流节点才返回成功.
    /// 如果一个子节点返回失败, 并行节点将结束所有子节点并返回失败.
    /// </summary>
    [CreateNodeMenu("Flow/Base/Parallel")]
    public class FlowParallel : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)]
        public ControlPort exit;

        [NonSerialized] private List<NodePort> _lstPorts = new List<NodePort>();

        [NonSerialized] private List<EFlowStatus> _lstStatus = new List<EFlowStatus>();

        private int _portCount;

        protected override void OnAwake()
        {
            _lstPorts = this.GetOutputPort(nameof(exit)).GetConnections();
            _portCount = _lstPorts.Count;
            while (_lstStatus.Count < _portCount)
            {
                _lstStatus.Add(EFlowStatus.EInactive);
            }
        }

        protected override void OnStart()
        {
            for (int i = 0; i < _portCount; i++)
            {
                _lstStatus[i] = EFlowStatus.EInactive;
            }
        }

        protected override EFlowStatus OnUpdate()
        {
            bool bHasRunning = false;
            for (int i = 0; i < _portCount; i++)
            {
                var childStatus = _lstStatus[i];
                if (childStatus == EFlowStatus.ESuccess)
                {
                    continue;
                }

                if (childStatus == EFlowStatus.EFailure)
                {
                    return EFlowStatus.EFailure;
                }

                var childPort = _lstPorts[i];
                childStatus = _lstStatus[i] = this.Flow.ExecuteNextPort(childPort);
                if (childStatus == EFlowStatus.EFailure)
                {
                    // DONE: 打断其他几个节点的运行, 调用其OnEnd.
                    Interrupt();
                    return EFlowStatus.EFailure;
                }

                if (childStatus == EFlowStatus.ERunning)
                {
                    bHasRunning = true;
                }
            }

            if (bHasRunning)
            {
                return EFlowStatus.ERunning;
            }

            return EFlowStatus.ESuccess;
        }

        private void Interrupt()
        {
            for (int i = 0; i < _portCount; i++)
            {
                var childStatus = _lstStatus[i];
                var childPort = _lstPorts[i];
                if (childStatus == EFlowStatus.ERunning)
                {
                    // DONE: 中断该节点.
                    this.Flow.InterruptPort(childPort);
                }
            }
        }

        protected override void OnEnd()
        {
            for (int i = 0; i < _portCount; i++)
            {
                _lstStatus[i] = EFlowStatus.EInactive;
            }
        }
    }
}