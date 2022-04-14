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

        public void Interrupt()
        {
            OnInterrupt();
            OnEnd();
        }

        public EFlowStatus Update()
        {
            return OnUpdate();
        }

        protected abstract void OnAwake();

        protected abstract void OnStart();

        protected virtual EFlowStatus OnUpdate()
        {
            return EFlowStatus.ESuccess;
        }

        protected abstract void OnEnd();

        protected abstract void OnInterrupt();
    }
}