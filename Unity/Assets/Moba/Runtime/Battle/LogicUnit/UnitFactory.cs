namespace Scarf.Moba
{
    public static class UnitFactory
    {
        public static Unit CreateUnit(this BattleScene self, AttrData attrData)
        {
            var unit = new Unit();
            unit.Battle = self.Battle;

            // DONE: 添加组件.
            // TODO 添不添加移动组件, 根据配置来, 像防御塔就不需要添加移动和旋转组件.
            unit.AddComponent<UnitAttrComponent>();
            unit.AddComponent<UnitMoveComponent>();
            unit.AddComponent<UnitRotateComponent>();
            unit.AddComponent<UnitSkillComponent>();

            unit.BornPos = attrData.BornPos;
            unit.BornForward = attrData.BornForward;
            unit.TemplateId = attrData.TemplateId;
            unit.ServerId = attrData.ServerId;
            unit.SkinId = attrData.SkinId;
            unit.Nickname = attrData.NickName;
            
            unit.SetLogicPos(unit.BornPos);
            unit.SetForward(unit.BornForward);

            unit.Init();
            unit.Start();
            self.AddUnit(unit);
            return unit;
        }
    }
}