using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class UnitViewComponent : Entity
    {
        public Animator m_cAnimator;
        public Transform m_cTransform; // 角色模型.

        public Dictionary<int, Transform> m_dicUnitHangPoint;
        
        // 移动
        public bool m_bIsMove;
        public Vector3 m_sTargetPos;
        public float m_fMoveSpeed;
        public float m_fStartSpeedTime;
        public Vector3 m_curPos;
        
        // 旋转
        public bool m_bRotating;
        public float m_fRemainAngle;
        public float m_rotateSpeed;
        public bool m_bIsClockwise;
        public Vector3 m_sTargetForward;
        public Vector3 m_curForward;
    }
}