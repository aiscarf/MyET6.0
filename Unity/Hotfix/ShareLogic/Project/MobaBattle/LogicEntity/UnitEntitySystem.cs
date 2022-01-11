namespace ET
{
    [ObjectSystem]
    public class UnitEntityAwakeSystem : AwakeSystem<BaseUnit>
    {
        public override void Awake(BaseUnit self)
        {
        }
    }

    [ObjectSystem]
    public class UnitEntityDestroySystem : DestroySystem<BaseUnit>
    {
        public override void Destroy(BaseUnit self)
        {
        }
    }

    // TODO 逻辑上的Update.

    public static class UnitEntitySystem
    {
        public static void SetLogicPos(this BaseUnit self, SVector3 pos)
        {
            self.LogicPos = pos;
        }

        public static void UpdateLogicPos(this BaseUnit self, SVector3 pos)
        {
            self.LogicPos = pos;
            // TODO 通知变动.
        }

        #region 逻辑旋转

        public static void SetForward(this BaseUnit self, SVector3 sForward, bool bImmediately,
            EForwardType forwardType)
        {
            if (!self.CanForward(forwardType) || sForward.x == 0 && sForward.z == 0)
                return;
            sForward.NormalizeXz();
            if (sForward == self.m_sTargetForward)
                return;
            if (bImmediately)
            {
                self.m_sCurForward = sForward;
                self.m_sTargetForward = self.m_sCurForward;
                self.m_bRotating = false;
                self.m_sForwardType = EForwardType.ENone;
            }
            else
            {
                self.m_nRemainAngle = SVector3.SignedAngle(self.m_sCurForward, sForward, SVector3.up);
                self.m_bIsClockwise = self.m_nRemainAngle > 0;
                self.m_nRemainAngle = CMath.Abs(self.m_nRemainAngle);
                bool flag = self.m_nRemainAngle <= self.m_nOnFrameRotateAngle;
                self.m_bRotating = !flag;
                if (!self.m_bRotating)
                {
                    self.m_sCurForward = sForward;
                    self.m_sForwardType = EForwardType.ENone;
                }
                else
                {
                    self.m_sForwardType = forwardType;
                }

                self.m_sTargetForward = sForward;
                self.m_sCurForward = sForward;
            }
        }

        public static bool CanForward(this BaseUnit self, EForwardType forwardType)
        {
            return true; //(forwardType == EForwardType.EPlayer || forwardType == EForwardType.EMove);
        }

        private static void UpdateRotate(this BaseUnit self)
        {
            if (!self.m_bRotating)
                return;
            if (self.m_nRemainAngle != 0)
            {
                self.m_nOnFrameRotateAngle = FrameSyncComponent.LOGIC_FRAME_DELTA * self.m_nRotateSpeed / 1000;
                if (self.m_nRemainAngle > 0 && self.m_nRemainAngle < self.m_nOnFrameRotateAngle)
                    self.m_nOnFrameRotateAngle = self.m_nRemainAngle;
                self.m_nRemainAngle -= self.m_nOnFrameRotateAngle;
                self.m_sCurForward =
                    (!self.m_bIsClockwise
                        ? SQuaternion.AngleAxis(self.m_nOnFrameRotateAngle, -SVector3.up)
                        : SQuaternion.AngleAxis(self.m_nOnFrameRotateAngle, SVector3.up)) * self.m_sCurForward;
                self.m_sCurForward.NormalizeXz();
            }
            else
            {
                self.m_sCurForward = self.m_sTargetForward;
                self.m_bRotating = false;
                self.m_sForwardType = EForwardType.ENone;
            }
        }

        #endregion
    }
}