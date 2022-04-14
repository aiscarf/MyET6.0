using System;

namespace Scarf.Moba
{
    public class Event_UnitTargetPos_Graphic : AEvent<EventType.UnitTargetPos> {
        protected override void Run(EventType.UnitTargetPos args)
        {
            try
            {
                var unitGraphic = args.unit.GetComponent<UnitGraphicComponent>();
                float fTime = (args.targetPos - args.unit.LogicPos).magnitudeXz / (float)args.speed;
                unitGraphic.SetTargetPosition(args.targetPos.ToUnity(), fTime);
            }
            catch (Exception e)
            {
                BattleLog.Error(e);
            }
        }
    }
}