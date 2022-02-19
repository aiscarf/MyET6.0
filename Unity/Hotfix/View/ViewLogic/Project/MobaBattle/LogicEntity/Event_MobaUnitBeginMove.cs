namespace ET
{
    public class Event_MobaUnitBeginMove : AEvent<EventType.MobaUnitBeginMove>
    {
        protected override async ETTask Run(EventType.MobaUnitBeginMove args)
        {
            await ETTask.CompletedTask;
            var unitMoveComponent = args.unit.GetComponent<UnitMoveComponent>();
            var unitViewComponent = args.unit.GetComponent<UnitViewComponent>();
            if (unitViewComponent == null)
                return;
            unitViewComponent.BeginMove(unitMoveComponent.m_nSpeed);
        }
    }
}