using UnityEngine;

namespace ET
{
    public partial class UIBattleComponent
    {
        public RectTransform m_rtFrontArrow;
        public float m_fFrontArrowRadius;

        public InputComponent m_inputComonent;
        public Vector3 m_sUnitForward;
        public float m_fCameraAngleY;
        public long Uid { get; set; }
    }
}