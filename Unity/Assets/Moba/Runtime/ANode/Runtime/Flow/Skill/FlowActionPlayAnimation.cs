using System;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [Serializable]
    [CreateNodeMenu("Flow/技能/动画/播放动画")]
    public class FlowActionPlayAnimation : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("动画名")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public string animationName;

        [NonSerialized] private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            animationName = this.GetInputValue<string>(nameof(animationName));
            
            // DONE: 播放角色动画.
            this.Master.UnitAnimation.PlayAnimation(this.animationName);
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}