using UnityEngine;

namespace ET
{
    [UITag(UIType.UIBattle)]
    public class UIBattleComponent : Entity
    {
        public ETCJoystick m_moveJoystick;
        public RectTransform m_rtFrontArrow;
        public float m_fFrontArrowRadius;
    }
}