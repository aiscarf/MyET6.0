namespace Scarf.Moba
{
    /// <summary>
    /// 单位类型
    /// 可附加组件: 移动, 属性, 技能, Buff, 复活, 旋转, 状态机组件, 视图组件, 技能视图组件, Buff视图组件, 
    /// </summary>
    public class Unit: CObject
    {
        public UnitSignalSet SignalSet = new UnitSignalSet();
        public int TemplateId;
        public long ServerId;
        public int SkinId;
        public int Uid;
        public string Nickname;
        public bool IsDie;
        public SVector3 LogicPos { get; private set; }
        public SVector3 LogicForward { get; private set; }
        public int Radius;
        public int DetectRange;
        public ECamp Camp;
        public EUnitType UnitType;
        public SVector3 BornPos;
        public SVector3 BornForward;

        public void SetLogicPos(SVector3 pos)
        {
            this.LogicPos = pos;
        }

        public void UpdateLogicPos(SVector3 pos)
        {
            this.LogicPos = pos;
            // TODO 通知变动.
        }

        public void SetForward(SVector3 forward)
        {
            this.LogicForward = forward;
        }

        public UnitMoveComponent UnitMove => this.GetComponent<UnitMoveComponent>();
        public UnitRotateComponent UnitRotate => this.GetComponent<UnitRotateComponent>();
        public UnitAttrComponent UnitAttr => this.GetComponent<UnitAttrComponent>();
        public UnitSkillComponent UnitSkill => this.GetComponent<UnitSkillComponent>();
        public UnitStateComponent UnitState => this.GetComponent<UnitStateComponent>();
        public UnitBuffComponent UnitBuff => this.GetComponent<UnitBuffComponent>();
        public UnitAnimationComponent UnitAnimation => this.GetComponent<UnitAnimationComponent>();

        public void Die(DamageInfo dmg)
        {
        }
    }
}