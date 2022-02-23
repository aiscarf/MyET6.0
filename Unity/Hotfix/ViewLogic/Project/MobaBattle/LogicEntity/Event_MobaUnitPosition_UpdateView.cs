namespace ET
{
    public class Event_MobaUnitPosition_UpdateView : AEvent<EventType.MobaUnitPosition>
    {
        protected override async ETTask Run(EventType.MobaUnitPosition args)
        {
            await ETTask.CompletedTask;

            var unitViewComponent = args.unit.GetComponent<UnitViewComponent>();
            if (unitViewComponent == null)
                return;
            unitViewComponent.SetPosition(args.pos.ToUnity());
        }
    }
}