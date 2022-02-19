namespace ET
{
    public class Event_MobaUnitForward_UpdateView : AEvent<EventType.MobaUnitForward>
    {
        protected override async ETTask Run(EventType.MobaUnitForward args)
        {
            await ETTask.CompletedTask;

            var unitViewComponent = args.unit.GetComponent<UnitViewComponent>();
            if (unitViewComponent == null)
                return;
            unitViewComponent.SetForward(args.forward.ToUnity(), args.bImmediately);
        }
    }
}