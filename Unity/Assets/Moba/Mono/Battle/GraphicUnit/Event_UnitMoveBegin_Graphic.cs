using System;

namespace Scarf.Moba
{
    public class Event_UnitMoveBegin_Graphic: AEvent<EventType.UnitMoveBegin>
    {
        protected override void Run(EventType.UnitMoveBegin args)
        {
            try
            {
                var unitGraphic = args.unit.GetComponent<UnitGraphicComponent>();
                unitGraphic.BeginMove(args.speed);
            }
            catch (Exception e)
            {
                BattleLog.Error(e);
            }
        }
    }
}