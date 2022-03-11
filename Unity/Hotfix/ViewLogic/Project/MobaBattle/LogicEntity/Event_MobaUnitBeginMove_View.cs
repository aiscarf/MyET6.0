namespace ET
{
    public class Event_MobaUnitBeginMove_View : AEvent<EventType.MobaUnitBeginMove>
    {
        protected override async ETTask Run(EventType.MobaUnitBeginMove args)
        {
            var unitMoveComponent = args.unit.GetComponent<UnitMoveComponent>();
            var unitViewComponent = args.unit.GetComponent<UnitViewComponent>();
            if (unitViewComponent == null)
                return;
            unitViewComponent.BeginMove(unitMoveComponent.m_nSpeed);
            await ETTask.CompletedTask;
        }
    }
}