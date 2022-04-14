using System;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    public enum ECompareType
    {
        [LabelText("等于")] EEqual,
        [LabelText("不等于")] ENotEqual,
        [LabelText("大于")] EGreater,
        [LabelText("小于")] ELess,
        [LabelText("大于等于")] EGreaterEqual,
        [LabelText("小于等于")] ELessEqual,
    }

    [Serializable]
    [CreateNodeMenu("Flow/Math/FlowCompareInt")]
    public class FlowCompareInt : FlowNode
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort enter;

        [Output(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)]
        public ControlPort exit;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int a;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public int b;

        [Input(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public ECompareType c;

        [Output(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)]
        public bool result;

        [NonSerialized] private NodePort _exitPort;

        protected override void OnAwake()
        {
            _exitPort = this.GetOutputPort(nameof(exit)).Connection;
        }
        
        protected override void OnStart()
        {
            a = this.GetInputValue<int>(nameof(a));
            b = this.GetInputValue<int>(nameof(b));
            c = this.GetInputValue<ECompareType>(nameof(c));

            result = false;
            switch (c)
            {
                case ECompareType.EEqual:
                    result = a == b;
                    break;
                case ECompareType.ENotEqual:
                    result = a != b;
                    break;
                case ECompareType.EGreater:
                    result = a > b;
                    break;
                case ECompareType.ELess:
                    result = a < b;
                    break;
                case ECompareType.EGreaterEqual:
                    result = a >= b;
                    break;
                case ECompareType.ELessEqual:
                    result = a <= b;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof (result))
            {
                return (object)result;
            }
            return null;
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