namespace ET
{
    public class Event_MobaUnitTargetPos_View : AEvent<EventType.MobaUnitTargetPos>
    {
        protected override async ETTask Run(EventType.MobaUnitTargetPos args)
        {
            await ETTask.CompletedTask;

            var unitViewComponent = args.unit.GetComponent<UnitViewComponent>();
            if (unitViewComponent == null)
                return;

            float fTime = (args.targetPos - args.unit.LogicPos).magnitudeXz / (float)args.speed;
            unitViewComponent.SetTargetPosition(args.targetPos.ToUnity(), fTime);
        }
    }
}