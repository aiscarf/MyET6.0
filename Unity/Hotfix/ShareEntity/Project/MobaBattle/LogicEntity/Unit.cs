namespace ET
{
    /// <summary>
    /// 单位类型
    /// 可附加组件: 移动, 属性, 技能, Buff, 复活, 旋转, 状态机组件, 视图组件, 技能视图组件, Buff视图组件, 
    /// 需要实现接口, IStepFrame
    /// </summary>
    public class Unit : Entity
    {
        public int TemplateId;
        public int ServerId;
        public int SkinId;
        public int EntityId;
        public string NickName;
        public bool IsDie;
        public SVector3 LogicPos;
        public SVector3 LogicForward => m_sCurForward;
        public int Radius;
        public int DetectRange;
        public ECamp Camp;
        public EUnitType UnitType;

        #region 旋转
        
        public SVector3 m_sCurForward;
        public int m_nOnFrameRotateAngle;
        public SVector3 m_sTargetForward;
        public bool m_bRotating;
        public EForwardType m_sForwardType;
        public int m_nRemainAngle;
        public bool m_bIsClockwise;
        public int m_nRotateSpeed = 720;
        
        #endregion

        #region 复活

        public SVector3 BornPos;
        public SVector3 BornForward;

        #endregion

    }
}