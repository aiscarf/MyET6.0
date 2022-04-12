using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    public class Flow
    {
        private FlowNodeGraph m_flowNodeGraph;
        private FlowNode m_flowHead;
        private EFlowStatus m_flowStatus = EFlowStatus.EInactive;
        private bool bIsFlow;

        public EFlowStatus CurStatus => m_flowStatus;

        public Flow(FlowNodeGraph flowNodeGraph, FlowNode flowHead)
        {
            m_flowNodeGraph = flowNodeGraph;
            m_flowHead = flowHead;
        }

        public EFlowStatus Tick()
        {
            return ExecuteNextPort(m_flowHead);
        }

        public void InterruptPort(NodePort port)
        {
            var flowNode = port.node as FlowNode;
            if (flowNode == null)
            {
                return;
            }

            if (flowNode.CurStatus != EFlowStatus.ERunning)
            {
                return;
            }

            flowNode.End();
        }

        public EFlowStatus ExecuteNextPort(NodePort port)
        {
            if (port == null)
            {
                return EFlowStatus.ESuccess;
            }
            
            return ExecuteNextPort(port.node as FlowNode);
        }

        private EFlowStatus ExecuteNextPort(FlowNode flowNode)
        {
            if (flowNode == null)
            {
                return EFlowStatus.ESuccess;
            }

            flowNode.Flow = this;

            if (flowNode.CurStatus == EFlowStatus.EInactive)
            {
                flowNode.Start();
            }

            // TODO 性能堪忧, 逻辑也不清晰, 甚至容易爆栈.
            // TODO 很容易栈溢出, 之后一定要想办法更改流线逻辑, 暂时先这样吧, 浪费太多时间了.
            var childStatus = flowNode.Update();

            // DONE: 出栈.
            if (childStatus != EFlowStatus.ERunning)
            {
                flowNode.End();
            }

            // DONE: 如实返回节点执行状态.
            return childStatus;
        }
    }
}