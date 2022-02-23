using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class UnitViewComponentAwakeSystem : AwakeSystem<UnitViewComponent, Transform>
    {
        public override void Awake(UnitViewComponent self, Transform root)
        {
            self.m_cTransform = root;
            self.m_cTransform.position = self.GetParent<Unit>().LogicPos.ToUnity();
            self.m_cTransform.forward = self.GetParent<Unit>().LogicForward.ToUnity();
            self.m_cAnimator = root.GetComponentInChildren<Animator>();
            self.m_dicUnitHangPoint = new Dictionary<int, Transform>()
            {
                { (int)EHangPoint.EHeadPoint, self.m_cTransform.Find("HeadPoint") },
                { (int)EHangPoint.EBeHitPoint, self.m_cTransform.Find("HitPoint") },
                { (int)EHangPoint.EFootPoint, self.m_cTransform.Find("FootPoint") },
                { (int)EHangPoint.EBone1Point, self.m_cTransform.Find("BonePoint1") },
                { (int)EHangPoint.EBone2Point, self.m_cTransform.Find("BonePoint2") },
            };
        }
    }

    public class UnitViewComponentDestroySystem : DestroySystem<UnitViewComponent>
    {
        public override void Destroy(UnitViewComponent self)
        {
            self.m_cAnimator = null;
            self.m_cTransform = null;
            self.m_dicUnitHangPoint.Clear();
        }
    }

    public class UnitViewComponentUpdateSystem : UpdateSystem<UnitViewComponent>
    {
        public override void Update(UnitViewComponent self)
        {
            self.UpdateMove();
            self.UpdateRotate();
        }
    }

    public static class UnitViewSystem
    {
        #region 插帧移动

        public static void UpdateMove(this UnitViewComponent self)
        {
            if (!self.m_bIsMove)
                return;
            Vector3 vector3 = self.m_sTargetPos - self.m_curPos;
            if (CMath.IsZero(vector3.x) && CMath.IsZero(vector3.z))
                return;
            vector3.y = 0.0f;
            float num2 = self.m_fMoveSpeed * Time.deltaTime;
            if ((double)vector3.sqrMagnitude <= (double)num2 * (double)num2)
            {
                self.m_curPos = self.m_sTargetPos;
            }
            else
            {
                self.m_curPos += num2 * vector3.normalized;
            }

            self.SetTransformPos(self.m_curPos);
        }

        private static void SetTransformPos(this UnitViewComponent self, Vector3 pos)
        {
            self.m_cTransform.position = pos;
        }

        public static void SetTargetPosition(this UnitViewComponent self, Vector3 sTargetPos, float fTime)
        {
            fTime /= Time.timeScale;
            self.m_sTargetPos = sTargetPos;
            self.m_sTargetPos.y = self.m_curPos.y;

            if (CMath.IsZero(fTime))
            {
                self.SetTransformPos(sTargetPos);
            }
            else
            {
                Vector3 vector3 = self.m_sTargetPos - self.m_curPos;
                // TODO 修改速度.
//                float speed = vector3.magnitude / fTime;
//                if (speed > this.m_fMoveSpeed + 10)
//                {
//                    this.m_fMoveSpeed = speed;
//                }
            }
        }

        public static void SetPosition(this UnitViewComponent self, Vector3 sPos)
        {
            self.m_curPos = sPos;
            self.m_sTargetPos = sPos;
            self.SetTransformPos(self.m_curPos);
        }

        public static void BeginMove(this UnitViewComponent self, int nSpeed)
        {
            self.m_bIsMove = true;
            self.m_fMoveSpeed = nSpeed / 1000f;
        }

        public static void EndMove(this UnitViewComponent self, Vector3 sEndPos)
        {
            self.m_bIsMove = false;
            self.m_sTargetPos = sEndPos;
        }

        #endregion

        #region 插帧旋转

        public static void UpdateRotate(this UnitViewComponent self)
        {
            if (!self.m_bRotating)
                return;
            if (!CMath.IsZero(self.m_fRemainAngle))
            {
                float angle = Time.deltaTime * (float)self.m_rotateSpeed;
                if ((double)self.m_fRemainAngle > 0.0 && (double)self.m_fRemainAngle < (double)angle)
                    angle = self.m_fRemainAngle;
                self.m_fRemainAngle -= angle;

                self.m_curForward = (!self.m_bIsClockwise
                    ? Quaternion.AngleAxis(angle, -Vector3.up)
                    : Quaternion.AngleAxis(angle, Vector3.up)) * self.m_curForward;
            }
            else
            {
                self.m_curForward = self.m_sTargetForward;
                self.m_bRotating = false;
            }

            self.SetTransformForward(self.m_curForward);
        }

        public static void SetForward(this UnitViewComponent self, Vector3 sForward, bool bImmediately)
        {
            if (bImmediately)
            {
                self.m_curForward = sForward;
                self.SetTransformForward(self.m_curForward);
                self.m_bRotating = false;
            }
            else
            {
                self.m_sTargetForward = sForward;
                self.m_fRemainAngle = Vector3.SignedAngle(self.m_curForward, sForward, Vector3.up);
                self.m_bIsClockwise = (double)self.m_fRemainAngle > 0.0;
                self.m_fRemainAngle = Mathf.Abs(self.m_fRemainAngle);
                self.m_bRotating = true;
            }
        }

        private static void SetTransformForward(this UnitViewComponent self, Vector3 sForward)
        {
            self.m_cTransform.forward = sForward;
        }

        #endregion

        #region 角色挂点管理

        public static Transform GetTransformByHangPoint(this UnitViewComponent self, EHangPoint eHangPoint)
        {
            Transform result = null;
            self.m_dicUnitHangPoint.TryGetValue((int)eHangPoint, out result);
            return result;
        }

        #endregion
    }
}