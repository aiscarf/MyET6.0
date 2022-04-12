namespace Scarf.Moba
{
    public class UnitRotateComponent: CComponent
    {
        public SVector3 m_sCurForward;
        public int m_nOnFrameRotateAngle;
        public SVector3 m_sTargetForward;
        public bool m_bRotating;
        public EForwardType m_sForwardType;
        public int m_nRemainAngle;
        public bool m_bIsClockwise;
        public int m_nRotateSpeed = 720;

        public void SetForward(SVector3 sForward, bool bImmediately, EForwardType forwardType)
        {
            if (!this.CanForward(forwardType) || sForward.x == 0 && sForward.z == 0)
                return;
            sForward.NormalizeXz();
            if (sForward == this.m_sTargetForward)
                return;
            if (bImmediately)
            {
                this.m_sCurForward = sForward;
                this.m_sTargetForward = this.m_sCurForward;
                this.m_bRotating = false;
                this.m_sForwardType = EForwardType.ENone;
            }
            else
            {
                this.m_nRemainAngle = SVector3.SignedAngle(this.m_sCurForward, sForward, SVector3.up);
                this.m_bIsClockwise = this.m_nRemainAngle > 0;
                this.m_nRemainAngle = CMath.Abs(this.m_nRemainAngle);
                bool flag = this.m_nRemainAngle <= this.m_nOnFrameRotateAngle;
                this.m_bRotating = !flag;
                if (!this.m_bRotating)
                {
                    this.m_sCurForward = sForward;
                    this.m_sForwardType = EForwardType.ENone;
                }
                else
                {
                    this.m_sForwardType = forwardType;
                }

                this.m_sTargetForward = sForward;
                this.m_sCurForward = sForward;
            }
        }

        public bool CanForward(EForwardType forwardType)
        {
            return true; //(forwardType == EForwardType.EPlayer || forwardType == EForwardType.EMove);
        }

        private void UpdateRotate(int delta)
        {
            if (!this.m_bRotating)
                return;
            if (this.m_nRemainAngle != 0)
            {
                this.m_nOnFrameRotateAngle = delta * this.m_nRotateSpeed / 1000;
                if (this.m_nRemainAngle > 0 && this.m_nRemainAngle < this.m_nOnFrameRotateAngle)
                    this.m_nOnFrameRotateAngle = this.m_nRemainAngle;
                this.m_nRemainAngle -= this.m_nOnFrameRotateAngle;
                this.m_sCurForward =
                        (!this.m_bIsClockwise
                                ? SQuaternion.AngleAxis(this.m_nOnFrameRotateAngle, -SVector3.up)
                                : SQuaternion.AngleAxis(this.m_nOnFrameRotateAngle, SVector3.up)) * this.m_sCurForward;
                this.m_sCurForward.NormalizeXz();
            }
            else
            {
                this.m_sCurForward = this.m_sTargetForward;
                this.m_bRotating = false;
                this.m_sForwardType = EForwardType.ENone;
            }
        }
    }
}