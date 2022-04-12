using System.Collections.Generic;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    public class FlowScheduler
    {
        private List<Flow> m_lstFlows = new List<Flow>();
        private EFlowStatus m_flowStatus = EFlowStatus.EInactive;

        public FlowScheduler(FlowNodeGraph flowNodeGraph)
        {
            // TODO 触发事件时, 动态创建一个流对象和该流所有相关的节点.
            foreach (Node node in flowNodeGraph.nodes)
            {
                // DONE: 找线头.
                if (node is FlowRoot flowRoot)
                {
                    this.m_lstFlows.Add(new Flow(flowNodeGraph, flowRoot));
                }
                else if (node is FlowCheckEvent flowCheckEvent)
                {
                    m_lstFlows.Add(new Flow(flowNodeGraph, flowCheckEvent));
                }
            }
        }

        public void Tick()
        {
            // TODO 检测事件的头放置在这里.

            // DONE: 按顺序驱动每个流.
            foreach (var flow in m_lstFlows)
            {
                flow.Tick();
            }
        }
    }
}