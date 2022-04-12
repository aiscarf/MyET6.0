using System;

namespace Scarf.Moba
{
    public static class UnitAttrMediator
    {
        public static int MASK000 = Int32.MaxValue << 3;

        public static DamageInfo CreateDamageInfo()
        {
            return CObjectPool<DamageInfo>.instance.GetObject();
        }

        public static void DamageHandle(DamageInfo damageInfo)
        {
            int defenserCurHp = damageInfo.Defenser.UnitAttr.GetValue(EAttrType.Hp);
            if (defenserCurHp < 1000)
            {
                CObjectPool<DamageInfo>.instance.SaveObject(damageInfo);
                return;
            }

            // 伤害阶段0 ------> 用于强化伤害阶段
            damageInfo.Attacker.SignalSet.OnDamageStage0Signal.Dispatch(damageInfo);
            damageInfo.Defenser.SignalSet.OnBeHurtStage0Signal.Dispatch(damageInfo);

            // 伤害阶段1 ------> 用于抵扣伤害阶段
            damageInfo.Attacker.SignalSet.OnDamageStage1Signal.Dispatch(damageInfo);
            damageInfo.Defenser.SignalSet.OnBeHurtStage1Signal.Dispatch(damageInfo);

            int num1 = damageInfo.DamageValue;

            if (damageInfo.DamageValue < 0)
            {
                // 角色扣防御
                int num2 = 0;
                switch (damageInfo.DamageType)
                {
                    case EDamageType.ENone:
                        break;
                    case EDamageType.EMagic:
                        num2 = damageInfo.Defenser.UnitAttr.GetValue(EAttrType.DefMagic);
                        break;
                    case EDamageType.EPhysic:
                        num2 = damageInfo.Defenser.UnitAttr.GetValue(EAttrType.DefPhysic);
                        break;
                    case EDamageType.EReal:
                        break;
                }

                num1 = (int)((long)num1 * (1000L - num2) / 1000L);

                // 防御承伤
                int num3 = -(damageInfo.DamageValue - num1);
                damageInfo.DamageReductionValue += num3;
            }

            // 实际造成了多少伤害
            damageInfo.DamageRealValue = num1;

            if (damageInfo.DamageRealValue < 0)
            {
                // 伤害阶段2 ------> 用于加工扣除防御后最终伤害的阶段
                damageInfo.Attacker.SignalSet.OnDamageStage2Signal.Dispatch(damageInfo);
                damageInfo.Defenser.SignalSet.OnBeHurtStage2Signal.Dispatch(damageInfo);
            }

            // 伤害取整
            damageInfo.DamageRealValue &= MASK000;

            if (damageInfo.DamageRealValue != 0)
            {
                int oldHp = damageInfo.Defenser.UnitAttr.GetValue(EAttrType.Hp);

                int remainHp = oldHp + damageInfo.DamageRealValue;
                if (damageInfo.DamageRealValue < 0)
                {
                    remainHp = remainHp < 1000? 0 : remainHp;
                }
                else
                {
                    int hpLimit = damageInfo.Defenser.UnitAttr.GetValue(EAttrType.HpLimit);
                    remainHp = remainHp > hpLimit? hpLimit : remainHp;
                }

                damageInfo.Defenser.UnitAttr.SetValue(EAttrType.Hp, remainHp);

                // 伤害阶段3
                if (damageInfo.DamageRealValue <= 0)
                {
                    damageInfo.Defenser.SignalSet.OnBeHurtSignal.Dispatch(damageInfo);
                    damageInfo.Attacker.SignalSet.OnHitTargetSignal.Dispatch(damageInfo);
                }
                else if (damageInfo.DamageRealValue > 0)
                {
                    damageInfo.Defenser.SignalSet.OnRecoverHpSignal.Dispatch(damageInfo);
                    damageInfo.Attacker.SignalSet.OnRecoverTargetSignal.Dispatch(damageInfo);
                }
            }

            // TLogger.Debug(
            //     $"攻击者{damageInfo.Attacker.EntityId}对受击者{damageInfo.Defenser.EntityId}预期造成<b><color=#ffffff>[{damageInfo.DamageValue}]</color></b>血量数值影响, 实际造成<b><color=#ffffff>[{damageInfo.DamageRealValue}]</color></b>血量数值影响");

            // 目标死亡
            int curHp = damageInfo.Defenser.UnitAttr.GetValue(EAttrType.Hp);
            if (curHp < 1000)
            {
                damageInfo.Defenser.Die(damageInfo);
                if (damageInfo.Defenser.IsDie)
                {
                    damageInfo.Attacker.SignalSet.OnKillSignal.Dispatch(damageInfo);
                }
            }

            CObjectPool<DamageInfo>.instance.SaveObject(damageInfo);
        }
    }
}