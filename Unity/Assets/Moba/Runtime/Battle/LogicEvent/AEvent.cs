using System;

namespace Scarf.Moba
{
    public interface IEvent
    {
        Type GetEventType();
    }
	
    [Event]
    public abstract class AEvent<A>: IEvent where A: struct
    {
        public Type GetEventType()
        {
            return typeof (A);
        }

        protected abstract void Run(A args);

        public void Handle(A args)
        {
            try
            { 
                Run(args);
            }
            catch (Exception e)
            {
                BattleLog.Error(e);
            }
        }
    }
}