using System;
using UnityEngine;

namespace ET
{
    public partial class UIBattleMediator : UIMediator<UIBattleComponent>
    {
        public override void OnInit()
        {
            self.EUI_ETCJoystick_MoveJoystick = referenceCollector.Get<GameObject>("MoveJoystick").GetComponent<ETCJoystick>();
            self.m_rtFrontArrow = self.EUI_ETCJoystick_MoveJoystick.transform.Find("Front").GetComponent<RectTransform>();
            self.m_fFrontArrowRadius = self.m_rtFrontArrow.anchoredPosition.magnitude;
            self.m_rtFrontArrow.gameObject.SetActive(false);

            self.EUI_ETCJoystick_MoveJoystick.onMove.AddListener(OnMoveHandle);
            self.EUI_ETCJoystick_MoveJoystick.onMoveEnd.AddListener(OnMoveEndHandle);
        }

        public override void OnDestroy()
        {
            self.EUI_ETCJoystick_MoveJoystick.onMove.RemoveListener(OnMoveHandle);
            self.EUI_ETCJoystick_MoveJoystick.onMoveEnd.RemoveListener(OnMoveEndHandle);
        }

        public override void OnOpen(object data)
        {
        }

        public override void OnClose()
        {
        }

        public override void OnBeCover()
        {
        }

        public override void OnUnCover()
        {
        }

        #region 移动

        void OnMoveHandle(Vector2 offset)
        {
            double a = Mathf.Atan2(offset.x, offset.y);
            a = a * (180 / Math.PI);

            Vector2 vPos2 = offset.normalized * self.m_fFrontArrowRadius;
            self.m_rtFrontArrow.anchoredPosition = vPos2;
            self.m_rtFrontArrow.localEulerAngles = new Vector3(0, 0, 360f - (float)a);
            if (!self.m_rtFrontArrow.gameObject.activeSelf)
            {
                self.m_rtFrontArrow.gameObject.SetActive(true);
            }
            
            ZoneSceneManagerComponent.Instance.CurScene.GetComponent<MobaBattleComponent>().GetComponent<InputComponent>()
                .InputOrderPriority(1001, EInputType.Move, (int)a, 0);
        }

        void OnMoveEndHandle()
        {
            self.m_rtFrontArrow.gameObject.SetActive(false);
            
            ZoneSceneManagerComponent.Instance.CurScene.GetComponent<MobaBattleComponent>().GetComponent<InputComponent>()
                .InputOrderPriority(1001, EInputType.MoveEnd, 0, 0);
        }

        #endregion
    }
}