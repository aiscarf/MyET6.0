using System;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [Serializable]
    [CreateNodeMenu("Flow/技能/事件/发出事件")]
    public class FlowActionEmitEvent : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("事件名")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public string eventName;

        [NonSerialized] private NodePort _exitPort;
        
        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            eventName = this.GetInputValue<string>(nameof(eventName));
            
            // TODO 发出事件名.
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}