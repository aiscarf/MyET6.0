using System;

namespace Scarf.Moba
{
    public class Event_AnimationEvent: AEvent<EventType.AnimationEvent>
    {
        protected override void Run(EventType.AnimationEvent args)
        {
            try
            {
                // TODO 将动画事件同步到技能状态机的事件图中.
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}