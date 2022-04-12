using System;
using System.Collections.Generic;

namespace Scarf.Moba
{
    public class EventMgr
    {
        // TODO 如何处理技能监听顺序, 哪个先, 哪个后.
        private Dictionary<Type, List<object>> allEvents;

        public void Init()
        {
            allEvents = new Dictionary<Type, List<object>>();

            var list = this.GetType().Assembly.GetTypes();
            foreach (Type type in list)
            {
                var objects = type.GetCustomAttributes(typeof (EventAttribute), true);
                if (objects.Length <= 0)
                {
                    continue;
                }

                IEvent obj = Activator.CreateInstance(type) as IEvent;
                if (obj == null)
                {
                    throw new Exception($"type not is AEvent: {obj.GetType().Name}");
                }

                Type eventType = obj.GetEventType();
                if (!this.allEvents.ContainsKey(eventType))
                {
                    this.allEvents.Add(eventType, new List<object>());
                }

                this.allEvents[eventType].Add(obj);
            }
        }

        public void Clear()
        {
            allEvents.Clear();
            allEvents = null;
        }

        public void Publish<T>(T a) where T : struct
        {
            List<object> iEvents;
            if (!this.allEvents.TryGetValue(typeof (T), out iEvents))
            {
                return;
            }

            foreach (object obj in iEvents)
            {
                if (!(obj is AEvent<T> aEvent))
                {
                    BattleLog.Error($"event error: {obj.GetType().Name}");
                    continue;
                }

                aEvent.Handle(a);
            }
        }
    }
}