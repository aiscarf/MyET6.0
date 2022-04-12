using System;
using Scarf.Moba;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [Serializable]
    public abstract class FlowNode : Node
    {
        private FlowNodeGraph _flowNodeGraph;
        public FlowNodeGraph FlowNodeGraph => _flowNodeGraph ?? (_flowNodeGraph = this.graph as FlowNodeGraph);
        public Flow Flow { get; set; }
        public Battle Battle { get; set; }

        public Skill Skill => this.FlowNodeGraph.Skill;

        public Unit Master => this.FlowNodeGraph.Skill.Master;

        public EFlowStatus CurStatus { get; private set; }

        public void Awake()
        {
            OnAwake();
        }

        public void Start()
        {
            OnStart();
            CurStatus = EFlowStatus.ERunning;
        }

        public void End()
        {
            OnEnd();
            CurStatus = EFlowStatus.EInactive;
        }

        public EFlowStatus Update()
        {
            return OnUpdate();
        }

        protected virtual void OnAwake()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual EFlowStatus OnUpdate()
        {
            return EFlowStatus.ESuccess;
        }

        protected virtual void OnEnd()
        {
        }
    }
}