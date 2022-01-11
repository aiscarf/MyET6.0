namespace ET
{
    [ObjectSystem]
    public class UnitMoveComponentAwakeSystem : AwakeSystem<UnitMoveComponent>
    {
        public override void Awake(UnitMoveComponent self)
        {
            self.Master = self.GetParent<BaseUnit>();
        }
    }
    
    [ObjectSystem]
    public class UnitMoveComponentDestroySystem : DestroySystem<UnitMoveComponent>
    {
        public override void Destroy(UnitMoveComponent self)
        {
            self.m_quePath.Clear();
        }
    }
    
    // TODO 同步的Update

    public static class UnitMoveSystem
    {
        public static void OnLogicFrame(this UnitMoveComponent self)
        {
            if (self.m_bIsMoving)
                return;
            byte mask = self.GetMoveMask();
            int num = self.m_nSpeed;
            SVector3 svector3_1 = self.m_sNextPosition - self.Master.LogicPos;
            SVector3 svector3_2 = svector3_1.normalizedXz * num / 1000 * FrameSyncComponent.LOGIC_FRAME_DELTA / 1000;
        }
        
        public static byte GetMoveMask(this UnitMoveComponent self)
        {
            byte result = byte.MaxValue;
            switch (self.m_eMoveType)
            {
                case EMoveType.ENone:
                    break;
                case EMoveType.ENormal:
                    break;
                case EMoveType.ESystem:
                    break;
                case EMoveType.EHurtMove:
                    break;
                case EMoveType.EHurtMoveIgnoreAll:
                    result = 0;
                    break;
                case EMoveType.ESkillMove:
                    break;
                case EMoveType.ESkillMoveIgnoreTerrain:
                case EMoveType.ESystemIgnoreTerrain:
                    result = 0;
                    break;
                case EMoveType.EFear:
                    break;
            }

            return result;
        }
    }
}