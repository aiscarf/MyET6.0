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
            // DONE: 按顺序驱动每个流.
            foreach (var flow in m_lstFlows)
            {
                flow.Tick();
            }
        }

        public void Stop()
        {
            foreach (var flow in this.m_lstFlows)
            {
                flow.Stop();
            }
        }
    }
}