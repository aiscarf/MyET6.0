using UnityEngine;
using DG.Tweening;

namespace ET
{
    public class ChaseCamera: MonoBehaviour
    {
        private int m_nShakeSpeed = 10000;
        private Camera m_cMainCamera;
        private Transform m_cCameraTrans;
        private Transform m_cTargetTrans;
        private float m_fMoveTime;
        private float m_fMoveSpeed;
        private float m_fCameraOrthoSize;
        private float m_fStartShakeTime;
        private float m_fShakeTime;
        private int m_nShakeDelta;
        private int m_nCurOffset;
        private bool m_bIsDirChange;
        private bool m_bIsDestroy;
        private bool m_bIsRoam; // 是否镜头漫游

        public Camera mainCamera => this.m_cMainCamera;
        public float CameraOrthoSize => this.m_fCameraOrthoSize;

        public static Vector3 DefaultPos = new Vector3(0.5f, 16f, -11f);
        public static Vector3 DefaultEuler = new Vector3(50f, 0, 0);

        public void Init()
        {
            this.m_cCameraTrans = this.transform;
            this.m_cMainCamera = this.m_cCameraTrans.Find("SceneCamera").GetComponent<Camera>();
            //        this.m_fStartShakeTime = (float) FrameSyncSys.time;
            DefaultPos = this.m_cMainCamera.transform.localPosition;
            DefaultEuler = this.m_cMainCamera.transform.localEulerAngles;

            this.m_fMoveSpeed = 20f;
            this.m_fCameraOrthoSize = this.m_cMainCamera.orthographicSize;
            this.m_bIsDestroy = false;
        }

        public void Clear()
        {
            this.m_cMainCamera = null;
            this.m_cCameraTrans = null;
            this.m_bIsDestroy = true;
        }

        public void SetFollower(Transform target, float time = 0.0f)
        {
            this.m_cTargetTrans = target;
            if (this.m_cTargetTrans == null)
                return;
            this.m_fMoveTime = time;
            if ((double)this.m_fMoveTime > 0.0)
                return;
            this.m_cCameraTrans.position = this.m_cTargetTrans.position;
        }

        public void SetForward(Vector3 forward)
        {
            this.m_cCameraTrans.forward = forward;
        }

        public void DoMove(Vector3 targetPos, float time = 0.0f)
        {
            if (time <= 0.0f)
            {
                this.m_cMainCamera.transform.localPosition = targetPos;
                return;
            }

            this.m_cMainCamera.transform.DOLocalMove(targetPos, time);
        }

        public void DoRotate(Vector3 targetRot, float time = 0.0f)
        {
            if (time <= 0.0f)
            {
                this.m_cMainCamera.transform.localEulerAngles = targetRot;
                return;
            }

            this.m_cMainCamera.transform.DOLocalRotate(targetRot, time);
        }

        private void LateUpdate()
        {
            if (this.m_bIsDestroy)
                return;
            if (this.m_cTargetTrans == null)
                return;
            if (this.m_cCameraTrans == null)
                return;
            if (this.m_cMainCamera == null)
                return;
            if (this.m_bIsDragCamera)
                return;
            if ((double)this.m_fMoveTime > 0.0)
            {
                float num = (this.m_cTargetTrans.transform.position - this.m_cCameraTrans.position).magnitude / this.m_fMoveTime;
                this.m_fMoveTime -= Time.deltaTime;
                this.m_cCameraTrans.position = Vector3.MoveTowards(this.m_cCameraTrans.position,
                    this.m_cTargetTrans.transform.position, num * Time.deltaTime);

                return;
            }

            this.m_cCameraTrans.position = this.m_cTargetTrans.transform.position;
        }

        #region 镜头抖动

        public void Shake(int nTime, int nShakeDelta, int nShakeSpeed)
        {
            this.m_fStartShakeTime = Time.time;
            this.m_fShakeTime = (float)nTime / 1000f;
            this.m_nShakeDelta = nShakeDelta;
            this.m_nCurOffset = 0;
            this.m_bIsDirChange = false;
            this.m_nShakeSpeed = nShakeSpeed;
        }

        private Vector3 GetShakeOffset()
        {
            if ((double)Time.time - (double)this.m_fStartShakeTime > (double)this.m_fShakeTime)
                return Vector3.zero;
            int num = (int)((double)Time.time * 1000.0 * (double)this.m_nShakeSpeed / 1000.0);
            if (this.m_bIsDirChange)
            {
                this.m_nCurOffset -= num;
                if (this.m_nCurOffset <= -this.m_nShakeDelta)
                {
                    this.m_nCurOffset = -this.m_nShakeDelta;
                    this.m_bIsDirChange = false;
                }
            }
            else
            {
                this.m_nCurOffset += num;
                if (this.m_nCurOffset >= this.m_nShakeDelta)
                {
                    this.m_nCurOffset = this.m_nShakeDelta;
                    this.m_bIsDirChange = true;
                }
            }

            return new Vector3((float)this.m_nCurOffset / 1000f, (float)-this.m_nCurOffset / 1000f, 0.0f);
        }

        #endregion

        #region 镜头漫游

        private const float MAX_ROAM_TIME = 5f;

        public void StartRoam()
        {
            this.m_bIsRoam = true;
            this.m_cCameraTrans.transform.position = Vector3.zero;
            this.m_fMoveTime = MAX_ROAM_TIME;
        }

        public void StopRoam()
        {
            this.m_bIsRoam = false;
            this.m_fMoveTime = 0;
        }

        #endregion

        #region 镜头拖拽

        private bool m_bIsDragCamera;

        public void SetDragCameraPos(Vector3 worldPos, float duration = 0f)
        {
            this.m_bIsDragCamera = true;

            this.m_cCameraTrans.DOKill();
            this.m_cCameraTrans.DOMove(worldPos, duration);
        }

        public void ResetDragCameraPos()
        {
            this.m_bIsDragCamera = false;
            this.m_cCameraTrans.DOKill();

            if (this.m_cTargetTrans == null)
                return;
            this.m_cCameraTrans.position = this.m_cTargetTrans.transform.position;
        }

        #endregion
    }
}