namespace ET
{
    public class Event_MobaUnitEndMove_View : AEvent<EventType.MobaUnitEndMove>
    {
        protected override async ETTask Run(EventType.MobaUnitEndMove args)
        {
            var unitViewComponent = args.unit.GetComponent<UnitViewComponent>();
            if (unitViewComponent == null)
                return;
            unitViewComponent.EndMove(args.unit.LogicPos.ToUnity());
            await ETTask.CompletedTask;
        }
    }
}