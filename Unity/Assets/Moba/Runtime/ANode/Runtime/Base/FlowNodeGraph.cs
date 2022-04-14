using System;
using Scarf.Moba;
using Sirenix.OdinInspector;
using XNode;

namespace Scarf.ANode.Flow.Runtime
{
    public class FlowNodeGraph: NodeGraph
    {
        private FlowScheduler _flowScheduler;

        // [UnityEngine.SerializeField, ShowInInspector]
        // // DONE: 参数黑板, 编辑器可编辑.
        // private Blackboard _blackboard = new Blackboard();

        // public Blackboard Blackboard
        // {
        //     get => _blackboard;
        //     set => _blackboard = value;
        // }

        // DONE: 事件黑板
        [NonSerialized]
        private Eventboard _eventboard = new Eventboard();

        public Eventboard Eventboard => _eventboard;

        public Skill Skill { get; private set; }

        protected override void OnInit()
        {
            _flowScheduler = new FlowScheduler(this);

            foreach (var node in nodes)
            {
                if (!(node is FlowNode flowNode))
                {
                    continue;
                }

                flowNode.Awake();
            }
        }

        public void BindSkill(Skill skill)
        {
            this.Skill = skill;
            foreach (var node in nodes)
            {
                if (!(node is FlowNode flowNode))
                {
                    continue;
                }

                flowNode.Battle = Skill.Battle;
            }
        }

        public void Stop()
        {
            this._flowScheduler.Stop();
        }

        public void Tick()
        {
            _flowScheduler.Tick();
            _eventboard.Clear();
        }
    }
}