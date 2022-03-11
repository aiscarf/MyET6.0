namespace ET
{
    [UIEventTag(UIType.UIBattle)]
    public class UIBattleEvent : UIEvent<UIBattleComponent>
    {
        public override async ETTask PreOpen(object args)
        {
            // DONE: 根据Uid找寻自己操纵的玩家的InputComponent.
            var battleScene = ZoneSceneManagerComponent.Instance.CurScene;
            var battleDataComponent = battleScene.GetComponent<BattleDataComponent>();
            var mobaBattleEntity = battleScene.GetChild<MobaBattleEntity>(battleDataComponent.BattleId);
            var myUnit = mobaBattleEntity.GetComponent<BattleSceneComponent>().GetUnitByServerId(battleDataComponent.Uid);
            self.m_inputComonent = mobaBattleEntity.GetComponent<InputComponent>();
            self.Uid = battleDataComponent.Uid;
            self.m_sUnitForward = myUnit.BornForward.ToUnity();
            self.m_fCameraAngleY = mobaBattleEntity.GetComponent<MobaBattleViewComponent>().ChaseCamera.mainCamera
                .transform.eulerAngles.y;
            await UIManager.Instance.OpenUI(UIType.UIBattle);
        }
    }
}