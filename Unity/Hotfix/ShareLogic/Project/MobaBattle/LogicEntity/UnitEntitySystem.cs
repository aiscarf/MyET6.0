namespace ET
{
    [ObjectSystem]
    public class UnitEntityAwakeSystem : AwakeSystem<Unit>
    {
        public override void Awake(Unit self)
        {
        }
    }

    [ObjectSystem]
    public class UnitEntityDestroySystem : DestroySystem<Unit>
    {
        public override void Destroy(Unit self)
        {
        }
    }

    public static class UnitEntitySystem
    {
        #region 逻辑移动

        public static void SetLogicPos(this Unit self, SVector3 pos)
        {
            self.LogicPos = pos;
        }

        public static void UpdateLogicPos(this Unit self, SVector3 pos)
        {
            self.LogicPos = pos;
            // TODO 通知变动.
        }
        
        #endregion
        
        #region 逻辑旋转

        public static void SetForward(this Unit self, SVector3 sForward, bool bImmediately,
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
                
                Game.EventSystem.Publish(new EventType.MobaUnitForward() { unit = self, forward = self.m_sCurForward, bImmediately = true });
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

                Game.EventSystem.Publish(new EventType.MobaUnitForward() { unit = self, forward = self.m_sTargetForward, bImmediately = false });
            }
        }

        public static bool CanForward(this Unit self, EForwardType forwardType)
        {
            return true; //(forwardType == EForwardType.EPlayer || forwardType == EForwardType.EMove);
        }

        private static void UpdateRotate(this Unit self)
        {
            if (!self.m_bRotating)
                return;
            if (self.m_nRemainAngle != 0)
            {
                self.m_nOnFrameRotateAngle = self.GetFrameSyncComponent().LOGIC_FRAME_DELTA * self.m_nRotateSpeed / 1000;
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
                
                // 抛出Command, 关注的人, 主动监听.
                Game.EventSystem.Publish(new EventType.MobaUnitForward() { unit = self, forward = self.m_sCurForward, bImmediately = true});
            }
        }

        #endregion

        public static void OnFrameSyncUpdate(this Unit self)
        {
            self.UpdateRotate();
            var unitMoveComponent = self.GetComponent<UnitMoveComponent>();
            if (unitMoveComponent == null)
                return;
            unitMoveComponent.OnLogicFrame();
            // TODO 处理复活.
            // self.UpdateReborn();
        }
    }
}