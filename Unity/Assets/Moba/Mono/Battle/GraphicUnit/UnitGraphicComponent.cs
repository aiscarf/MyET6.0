using System.Collections.Generic;
using Scarf.ANode.Flow.Runtime;
using UnityEngine;

namespace Scarf.Moba
{
    public class UnitGraphicComponent: GraphicComponent
    {
        private Animator m_cAnimator;
        private Transform m_cTransform;

        private Dictionary<int, Transform> m_dicUnitHangPoint;

        // 移动
        private bool m_bIsMove;
        private Vector3 m_sTargetPos;
        private float m_fMoveSpeed;
        private float m_fStartSpeedTime;
        private Vector3 m_curPos;

        // 旋转
        private bool m_bRotating;
        private float m_fRemainAngle;
        private float m_rotateSpeed;
        private bool m_bIsClockwise;
        private Vector3 m_sTargetForward;
        private Vector3 m_curForward;

        private Unit m_cUnit;

        public void BindUnity(Transform root)
        {
            this.m_cTransform = root;
            this.m_cAnimator = root.GetComponentInChildren<Animator>();
            this.m_dicUnitHangPoint = new Dictionary<int, Transform>()
            {
                { (int)EHangPointType.EFoot, this.m_cTransform.Find("FootPoint") },
                { (int)EHangPointType.EHead, this.m_cTransform.Find("HeadPoint") },
                { (int)EHangPointType.EChest, this.m_cTransform.Find("ChestPoint") },
                { (int)EHangPointType.EBelly, this.m_cTransform.Find("BellyPoint") },
                { (int)EHangPointType.ELeftHand, this.m_cTransform.Find("LeftHandPoint") },
                { (int)EHangPointType.ERightHand, this.m_cTransform.Find("RightHandPoint") },
            };
        }

        protected override void OnInit()
        {
            this.m_cUnit = this.Parent as Unit;
            this.m_cTransform.position = m_cUnit.LogicPos.ToUnity();
            this.m_cTransform.forward = m_cUnit.LogicForward.ToUnity();
        }

        protected override void OnUnityUpdate()
        {
            this.UpdateMove();
            this.UpdateRotate();
        }

        private string lastState = string.Empty;

        public void SetAnimation(string state, int layer = -1, float normalizedTimer = 0f)
        {
            if (this.m_cAnimator == null)
                return;

            if (!string.IsNullOrEmpty(lastState))
            {
                this.m_cAnimator.ResetTrigger(lastState);
            }

            this.m_cAnimator.SetTrigger(state);
            this.lastState = state;
        }
        
        public void SetParameters(string parameterName, float f)
        {
            if (this.m_cAnimator == null)
                return;
            this.m_cAnimator.SetFloat(parameterName, f);
        }

        #region 插帧移动

        private void UpdateMove()
        {
            if (!this.m_bIsMove)
                return;
            Vector3 vector3 = this.m_sTargetPos - this.m_curPos;
            if (CMath.IsZero(vector3.x) && CMath.IsZero(vector3.z))
                return;
            vector3.y = 0.0f;
            float num2 = this.m_fMoveSpeed * Time.deltaTime;
            if ((double)vector3.sqrMagnitude <= (double)num2 * (double)num2)
            {
                this.m_curPos = this.m_sTargetPos;
            }
            else
            {
                this.m_curPos += num2 * vector3.normalized;
            }

            this.SetTransformPos(this.m_curPos);
        }

        private void SetTransformPos(Vector3 pos)
        {
            this.m_cTransform.position = pos;
        }

        public void SetTargetPosition(Vector3 sTargetPos, float fTime)
        {
            fTime /= Time.timeScale;
            this.m_sTargetPos = sTargetPos;
            this.m_sTargetPos.y = this.m_curPos.y;

            if (CMath.IsZero(fTime))
            {
                this.SetTransformPos(sTargetPos);
            }
            else
            {
                Vector3 vector3 = this.m_sTargetPos - this.m_curPos;
            }
        }

        public void SetPosition(Vector3 sPos)
        {
            this.m_curPos = sPos;
            this.m_sTargetPos = sPos;
            this.SetTransformPos(this.m_curPos);
        }

        public void BeginMove(int nSpeed)
        {
            this.m_bIsMove = true;
            this.m_fMoveSpeed = nSpeed / 1000f;
        }

        public void EndMove(Vector3 sEndPos)
        {
            this.m_bIsMove = false;
            this.m_sTargetPos = sEndPos;
        }

        #endregion

        #region 插帧旋转

        private void UpdateRotate()
        {
            if (!this.m_bRotating)
                return;
            if (!CMath.IsZero(this.m_fRemainAngle))
            {
                float angle = Time.deltaTime * (float)this.m_rotateSpeed;
                if ((double)this.m_fRemainAngle > 0.0 && (double)this.m_fRemainAngle < (double)angle)
                    angle = this.m_fRemainAngle;
                this.m_fRemainAngle -= angle;

                this.m_curForward = (!this.m_bIsClockwise
                        ? Quaternion.AngleAxis(angle, -Vector3.up)
                        : Quaternion.AngleAxis(angle, Vector3.up)) * this.m_curForward;
            }
            else
            {
                this.m_curForward = this.m_sTargetForward;
                this.m_bRotating = false;
            }

            this.SetTransformForward(this.m_curForward);
        }

        public void SetForward(Vector3 sForward, bool bImmediately)
        {
            if (bImmediately)
            {
                this.m_curForward = sForward;
                this.SetTransformForward(this.m_curForward);
                this.m_bRotating = false;
            }
            else
            {
                this.m_sTargetForward = sForward;
                this.m_fRemainAngle = Vector3.SignedAngle(this.m_curForward, sForward, Vector3.up);
                this.m_bIsClockwise = (double)this.m_fRemainAngle > 0.0;
                this.m_fRemainAngle = Mathf.Abs(this.m_fRemainAngle);
                this.m_bRotating = true;
            }
        }

        private void SetTransformForward(Vector3 sForward)
        {
            this.m_cTransform.forward = sForward;
        }

        #endregion
    }
}