using System;

namespace Scarf.Moba
{
    public class Event_UnitRotate_Graphic: AEvent<EventType.UnitRotate>
    {
        protected override void Run(EventType.UnitRotate args)
        {
            try
            {
                var unitGraphic = args.unit.GetComponent<UnitGraphicComponent>();
                unitGraphic.SetForward(args.forward.ToUnity(), args.bImmediately);
            }
            catch (Exception e)
            {
                BattleLog.Error(e);
            }
        }
    }
}