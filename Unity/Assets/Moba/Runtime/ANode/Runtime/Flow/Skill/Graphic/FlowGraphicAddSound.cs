using System;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    [CreateNodeMenu("Flow/技能/Graphic/添加音效")]
    public class FlowGraphicAddSound : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("音效资源名")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public string soundName;

        [LabelText("特效持续时间")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public float soundTime = 1.0f;
        
        [NonSerialized] private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            soundName = this.GetInputValue<string>(nameof(soundName));
            soundTime = this.GetInputValue<float>(nameof(soundTime));

            // TODO 添加音效.
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }
    }
}