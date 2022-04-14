using UnityEngine;

namespace Scarf.Moba
{
    public class BulletGraphicComponent: GraphicComponent
    {
        private Transform m_cTransform;
        private Vector3 m_sTargetPos;
        private float m_fSpeed;
        private bool m_bInit;
        private BaseBullet m_cBullet;

        public void BindGraphic(Transform root, Vector3 sOrgPos)
        {
            this.m_bInit = true;
            this.m_cTransform = root;
            this.m_cTransform.position = sOrgPos;
            this.m_cBullet = this.Parent as BaseBullet;
        }

        public void SetTargetPosition(Vector3 sTargetPos, float fSpeed)
        {
            this.m_sTargetPos = sTargetPos;
            this.m_fSpeed = fSpeed;
        }

        public void LookAtTarget(Vector3 sTargetPos)
        {
            this.m_cTransform.LookAt(sTargetPos);
        }

        protected override void OnUnityUpdate()
        {
            if (!this.m_bInit)
                return;
            Vector3 vector3 = this.m_sTargetPos - this.m_cTransform.position;
            if (vector3.x == 0 && vector3.z == 0 && vector3.y == 0)
                return;
            float num = this.m_fSpeed * Time.deltaTime;
            if ((double)vector3.sqrMagnitude <= (double)num * (double)num)
                this.m_cTransform.position = this.m_sTargetPos;
            else
                this.m_cTransform.position += num * vector3.normalized;
        }
    }
}