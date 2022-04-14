using System;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    public enum EHangPointType
    {
        ENone,
        [LabelText("脚底")] EFoot,
        [LabelText("头顶")] EHead,
        [LabelText("胸部")] EChest,
        [LabelText("腹部")] EBelly,
        [LabelText("左手")] ELeftHand,
        [LabelText("右手")] ERightHand,
    }

    [Serializable]
    [CreateNodeMenu("Flow/技能/Graphic/添加特效")]
    public class FlowGraphicAddEffect : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [LabelText("特效路径名")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public string effectName;

        [LabelText("特效挂接点")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public EHangPointType effectHangPointType;

        [LabelText("特效大小")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public float effectScale = 1.0f;

        [LabelText("特效持续时间")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public float effectTime = 1.0f;

        [LabelText("是否跟随旋转")] [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public bool bIsFollowRotation;
        
        [NonSerialized] private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }

        protected override void OnStart()
        {
            effectName = this.GetInputValue<string>(nameof(effectName));
            effectHangPointType = this.GetInputValue<EHangPointType>(nameof(effectHangPointType));
            effectScale = this.GetInputValue<float>(nameof(effectScale));
            effectTime = this.GetInputValue<float>(nameof(effectTime));
            bIsFollowRotation = this.GetInputValue<bool>(nameof(bIsFollowRotation));

            // TODO 使用Dotween插件配合, 动态添加跟技能不相关的特效, 特效可跟随也可不跟随.
            
            // TODO 查找角色模型.
        }

        protected override EFlowStatus OnUpdate()
        {
            return this.Flow.ExecuteNextPort(_exitPort);
        }

        protected override void OnEnd()
        {
            
        }

        protected override void OnInterrupt()
        {
            this.Flow.InterruptPort(_exitPort);
        }
    }
}