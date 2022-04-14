using System;

namespace Scarf.Moba
{
    public class Event_AnimationEvent: AEvent<EventType.AnimationEvent>
    {
        protected override void Run(EventType.AnimationEvent args)
        {
            try
            {
                // DONE: 将动画事件推送至技能图中.
                var unit = args.unit;
                if (unit.UnitSkill == null)
                    return;
                unit.UnitSkill.PushSkillEvent(args.animationName, args.eventName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}