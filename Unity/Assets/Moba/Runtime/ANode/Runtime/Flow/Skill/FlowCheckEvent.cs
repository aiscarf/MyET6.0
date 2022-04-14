using System;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [Serializable]
    [CreateNodeMenu("Flow/技能/事件/检测事件")]
    public class FlowCheckEvent: FlowNode
    {
        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [Input(ShowBackingValue.Always, ConnectionType.Override, TypeConstraint.Strict)]
        public string eventName;

        [NonSerialized]
        private NodePort _exitPort;

        private bool _bIsTrigger;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof (exit)).Connection;
        }

        protected override void OnStart()
        {
            eventName = this.GetInputValue<string>(nameof (eventName), eventName);

            _bIsTrigger = false;

            if (string.IsNullOrEmpty(eventName) || string.IsNullOrWhiteSpace(eventName))
            {
                return;
            }

            if (!this.FlowNodeGraph.Eventboard.ContainsEvent(eventName))
            {
                return;
            }

            _bIsTrigger = true;
        }

        protected override EFlowStatus OnUpdate()
        {
            if (!_bIsTrigger)
            {
                return EFlowStatus.ESuccess;
            }

            // DONE: 响应目标事件.
            return this.Flow.ExecuteNextPort(_exitPort);
        }

        protected override void OnEnd()
        {
            _bIsTrigger = false;
        }

        protected override void OnInterrupt()
        {
            if (!_bIsTrigger)
            {
                return;
            }

            this.Flow.InterruptPort(_exitPort);
        }
    }
}