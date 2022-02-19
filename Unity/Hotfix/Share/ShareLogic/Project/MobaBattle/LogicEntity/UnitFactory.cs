namespace ET
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(this BattleSceneComponent self, AttrData attrData)
        {
            var unit = self.AddChild<Unit>();
            // TODO 添加属性组件.
            // TODO 赋值属性.
            unit.BornPos = attrData.BornPos;
            unit.BornForward = attrData.BornForward;
            unit.TemplateId = attrData.TemplateId;
            unit.ServerId = attrData.ServerId;
            unit.SkinId = attrData.SkinId;
            unit.NickName = attrData.NickName;
            unit.AddComponent<UnitMoveComponent>();
            self.AddUnit(unit);
            
            unit.SetLogicPos(unit.BornPos);
            unit.SetForward(unit.BornForward, true, EForwardType.ENone);
            
            Game.EventSystem.Publish(new EventType.MobaCreateUnit() { unit = unit });
            return unit;
        }
    }
}