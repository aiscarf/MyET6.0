using System;

namespace Scarf.Moba
{
    public class Event_UnitMoveEnd_Graphic: AEvent<EventType.UnitMoveEnd>
    {
        protected override void Run(EventType.UnitMoveEnd args)
        {
            try
            {
                var unitGraphic = args.unit.GetComponent<UnitGraphicComponent>();
                unitGraphic.EndMove(args.unit.LogicPos.ToUnity());
            }
            catch (Exception e)
            {
                BattleLog.Error(e);
            }
        }
    }
}