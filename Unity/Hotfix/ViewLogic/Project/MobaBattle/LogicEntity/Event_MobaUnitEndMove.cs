namespace ET
{
    public class Event_MobaUnitEndMove : AEvent<EventType.MobaUnitEndMove>
    {
        protected override async ETTask Run(EventType.MobaUnitEndMove args)
        {
            await ETTask.CompletedTask;

            var unitViewComponent = args.unit.GetComponent<UnitViewComponent>();
            if (unitViewComponent == null)
                return;
            // unitViewComponent.EndMove(args.unit.LogicPos.ToUnity());
        }
    }
}