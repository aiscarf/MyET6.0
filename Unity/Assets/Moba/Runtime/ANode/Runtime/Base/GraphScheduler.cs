using System;
using XNode;
using System.Collections.Generic;

namespace Scarf.ANode.Flow.Runtime
{
    public class GraphScheduler: IDisposable
    {
        private List<FlowNodeGraph> m_lstFlowGraphs = new List<FlowNodeGraph>();

        private bool m_isValid;

        public GraphScheduler(List<NodeGraph> graphDatas)
        {
            foreach (var graphData in graphDatas)
            {
                if (string.IsNullOrEmpty(graphData.TypeQualifiedName) ||
                    string.IsNullOrWhiteSpace(graphData.TypeQualifiedName))
                {
                    continue;
                }

                // var type = Type.GetType(graphData.TypeQualifiedName);
                // if (type != typeof(FlowNodeGraph))
                // {
                //     continue;
                // }

                var flowNodeGraph = Activator.CreateInstance<FlowNodeGraph>();
                flowNodeGraph.nodes = graphData.nodes;
                flowNodeGraph.TypeQualifiedName = graphData.TypeQualifiedName;
                flowNodeGraph.Id = graphData.Id;
                flowNodeGraph.Init();

                m_lstFlowGraphs.Add(flowNodeGraph);
            }

            m_isValid = true;
        }

        public FlowNodeGraph GetFlowNodeGraph(int id)
        {
            return m_lstFlowGraphs.Find(graph => graph.Id == id);
        }

        public void Dispose()
        {
            // TODO 销毁所有的图和所有的节点.
        }

        public void Tick()
        {
            if (!m_isValid)
                return;

            // DONE: 按顺序驱动每张图.
            foreach (var flowNodeGraph in m_lstFlowGraphs)
            {
                flowNodeGraph.Tick();
            }
        }
    }
}