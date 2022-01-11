namespace ET
{
    public class BaseUnit : Entity
    {
        public int TemplateId;
        public int ServerId;
        public int SkinId;
        public int EntityId;
        public string NickName;
        public bool IsDie;
        public SVector3 LogicPos;
        public SVector3 LogicForward;
        public int Radius;
        public int DetectRange;
        public ECamp Camp;

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