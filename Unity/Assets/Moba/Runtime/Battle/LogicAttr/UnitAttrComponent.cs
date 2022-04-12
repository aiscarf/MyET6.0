using System.Collections.Generic;

namespace Scarf.Moba
{
    public class PreAttr: IPoolable
    {
        public EAttrType AttrType;
        public int Value;

        public void Reset()
        {
            this.AttrType = EAttrType.ENone;
            Value = 0;
        }
    }

    public class UnitAttrComponent: ChangedContainer
    {
        private List<int> _allKeys = new List<int>();

        public Unit Master { get; private set; }

        protected override void OnInit()
        {
            this.Master = this.Parent as Unit;

            var array = System.Enum.GetValues(typeof (EAttrType));

            bool IsDefined(int key)
            {
                return _allKeys.IndexOf(key) >= 0;
            }

            foreach (var attrType in array)
            {
                _allKeys.Add((int)attrType);
            }

            foreach (var attrType in array)
            {
                int key = (int)attrType;
                int baseKey = key * 10;
                int addKey = baseKey + 1;
                int ratioKey = baseKey + 2;

                if (this.ContainsKey(key))
                {
                    continue;
                }

                if (IsDefined(baseKey) &&
                    IsDefined(addKey))
                {
                    if (IsDefined(ratioKey))
                    {
                        this.Add(key, (IRelateChanged)null);
                        this.Add(baseKey,
                            (IRelateChanged)new UnitAttrRelateChanged2(this, baseKey, addKey, ratioKey, key));
                        this.Add(addKey,
                            (IRelateChanged)new UnitAttrRelateChanged2(this, baseKey, addKey, ratioKey, key));
                        this.Add(ratioKey,
                            (IRelateChanged)new UnitAttrRelateChanged2(this, baseKey, addKey, ratioKey, key));
                    }
                    else
                    {
                        this.Add(key, (IRelateChanged)null);
                        this.Add(baseKey, (IRelateChanged)new UnitAttrRelateChanged(this, baseKey, addKey, key));
                        this.Add(addKey, (IRelateChanged)new UnitAttrRelateChanged(this, baseKey, addKey, key));
                    }
                }
                else
                {
                    this.Add(key, (IRelateChanged)null);
                }
            }

            this.OnValueChanged += new ChangedContainer.ChangedHandler(this.OnSelfValueChange);
        }

        public int GetValue(EAttrType attrType)
        {
            return this.GetValue((int)attrType);
        }

        public override int GetValue(int key)
        {
            int result = base.GetValue(key);
            var attr = CObjectPool<PreAttr>.instance.GetObject();
            attr.AttrType = (EAttrType)key;
            attr.Value = result;
            this.Master.SignalSet.OnPreGetAttrSignal.Dispatch(this.Master, attr);
            result = attr.Value;
            CObjectPool<PreAttr>.instance.SaveObject(attr);
            return result;
        }

        public void SetValue(EAttrType attrType, int value)
        {
            this.SetValue((int)attrType, value);
        }

        public override void SetValue(int key, int value)
        {
            var attr = CObjectPool<PreAttr>.instance.GetObject();
            attr.AttrType = (EAttrType)key;
            attr.Value = value;
            this.Master.SignalSet.OnPreSetAttrSignal.Dispatch(this.Master, attr);
            value = attr.Value;
            CObjectPool<PreAttr>.instance.SaveObject(attr);

            switch ((EAttrType)key)
            {
                case EAttrType.ENone:
                    break;
                case EAttrType.Hp:
                    int maxValue = this.GetValue(EAttrType.HpLimit);
                    value = value > maxValue? maxValue : value;
                    value = value < 0? 0 : value;

                    // value &= UnitAttrMediator.MASK000;
                    break;
                case EAttrType.HpLimit:
                    // value &= UnitAttrMediator.MASK000;
                    break;
                case EAttrType.AtkPhysic:
                    // int atk_min_limit = MobaGameSys.instance.BattleData.ATK_MIN_LIMIT;
                    // value = value < atk_min_limit? atk_min_limit : value;
                    break;
                case EAttrType.DefPhysic:
                case EAttrType.DefMagic:
                    // int physic_magic_deffense_max_limit =
                    //         MobaGameSys.instance.BattleData.PHYSIC_MAGIC_DEFFENSE_MAX_LIMIT;
                    // int physic_magic_deffense_min_limit =
                    //         MobaGameSys.instance.BattleData.PHYSIC_MAGIC_DEFFENSE_MIN_LIMIT;
                    // value = value > physic_magic_deffense_max_limit? physic_magic_deffense_max_limit : value;
                    // value = value < physic_magic_deffense_min_limit? physic_magic_deffense_min_limit : value;
                    break;
                case EAttrType.MoveSpeed:
                    // int speed_max_limit = MobaGameSys.instance.BattleData.MOVE_SPEED_MAX_LIMIT;
                    // value = value > speed_max_limit? speed_max_limit : value;
                    //
                    // int speed_min_limit = MobaGameSys.instance.BattleData.MOVE_SPEED_MIN_LIMIT;
                    // value = value < speed_min_limit? speed_min_limit : value;

                    break;
                case EAttrType.AtkSpeed:
                    // int atkspeed_max_limit = MobaGameSys.instance.BattleData.ATKSPEED_MAX_LIMIT;
                    // value = value > atkspeed_max_limit? atkspeed_max_limit : value;
                    break;
            }

            base.SetValue(key, value);
        }

        private void OnSelfValueChange(int key, int oldValue, int newValue)
        {
            switch ((EAttrType)key)
            {
                case EAttrType.Hp:
                    // this.Master.SignalSet.OnHpChangeSignal.Dispatch(oldValue, newValue);
                    // this.Master.SignalSet.OnHpChange2Signal.Dispatch(this.Master, oldValue, newValue);
                    break;
                case EAttrType.HpLimit:
                    int dLimit = newValue - oldValue;
                    if (dLimit > 0)
                    {
                        int oldHp = this.GetValue(EAttrType.Hp);
                        this.SetValue(EAttrType.Hp, oldHp + dLimit);
                    }
                    else
                    {
                        // int curHp = this.Master.UnitAttr.GetValue(EAttrType.Hp);
                        // int offsetValue = curHp - newValue;
                        // if (offsetValue > 0)
                        // {
                        //     this.Master.UnitAttr.SetValue(EAttrType.Hp, newValue);
                        // }
                    }

                    break;
                case EAttrType.AtkPhysic:
                    break;
                case EAttrType.DefPhysic:
                    break;
                case EAttrType.DefMagic:
                    break;
                case EAttrType.MoveSpeed:
                    // this.Master.SignalSet.OnMoveSpeedChangeSignal.Dispatch(oldValue, newValue);
                    break;
                case EAttrType.AtkSpeed:
                    break;
                case EAttrType.Level:
                    // 判断是否是英雄等级提升
                    if (this.Master.UnitType == EUnitType.EHero)
                    {
                        // // 查表
                        // var heroAttributeData = TableBattleTool.GetHeroAttribute(this.Master.TemplateId, newValue);
                        // if (heroAttributeData != null)
                        // {
                        //     this.SetValue(EAttrType.HpLimitInitial, heroAttributeData.Hp);
                        //     this.SetValue(EAttrType.AtkPhysicInitial, heroAttributeData.Attack);
                        //     this.SetValue(EAttrType.DefPhysicInitial, heroAttributeData.PhysicalDefense);
                        //     this.SetValue(EAttrType.DefMagicInitial, heroAttributeData.MagicDefense);
                        //     this.SetValue(EAttrType.AtkSpeedInitial, heroAttributeData.AttackSpeed);
                        //
                        //     // 初始值 + 等级表的值
                        //     this.SetValue(EAttrType.HpLimitBase,
                        //         this.GetValue(EAttrType.HpLimitInitial) + this.GetValue(EAttrType.HpLimitUp));
                        //     this.SetValue(EAttrType.AtkPhysicBase,
                        //         this.GetValue(EAttrType.AtkPhysicInitial) + this.GetValue(EAttrType.AtkPhysicUp));
                        //     this.SetValue(EAttrType.DefPhysicBase,
                        //         this.GetValue(EAttrType.DefPhysicInitial) + this.GetValue(EAttrType.DefPhysicUp));
                        //     this.SetValue(EAttrType.DefMagicBase,
                        //         this.GetValue(EAttrType.DefMagicInitial) + this.GetValue(EAttrType.DefMagicUp));
                        //     this.SetValue(EAttrType.AtkSpeedBase,
                        //         this.GetValue(EAttrType.AtkSpeedInitial) + this.GetValue(EAttrType.AtkSpeedUp));
                        //
                        //     // 移动速度没有成长, 直接覆盖
                        //     this.SetValue(EAttrType.MoveSpeedBase, heroAttributeData.Speed);
                        // }
                    }
                    else
                    {
                        // 等级提升, 带来属性提升
                        int dLevel = newValue - 1;

                        // 初始值 + 等级成长值 * (等级 - 1)
                        this.SetValue(EAttrType.HpLimitBase,
                            this.GetValue(EAttrType.HpLimitInitial) + this.GetValue(EAttrType.HpLimitUp) * dLevel);
                        this.SetValue(EAttrType.AtkPhysicBase,
                            this.GetValue(EAttrType.AtkPhysicInitial) + this.GetValue(EAttrType.AtkPhysicUp) * dLevel);
                        this.SetValue(EAttrType.DefPhysicBase,
                            this.GetValue(EAttrType.DefPhysicInitial) + this.GetValue(EAttrType.DefPhysicUp) * dLevel);
                        this.SetValue(EAttrType.DefMagicBase,
                            this.GetValue(EAttrType.DefMagicInitial) + this.GetValue(EAttrType.DefMagicUp) * dLevel);
                        this.SetValue(EAttrType.AtkSpeedBase,
                            this.GetValue(EAttrType.AtkSpeedInitial) + this.GetValue(EAttrType.AtkSpeedUp) * dLevel);
                        this.SetValue(EAttrType.KilledExp,
                            this.GetValue(EAttrType.KilledExpInitial) + this.GetValue(EAttrType.KilledExpUp) * dLevel);
                    }

                    // this.Master.SignalSet.OnLevelUpSignal.Dispatch(oldValue, newValue);
                    break;
                case EAttrType.KilledExp:
                    break;
                case EAttrType.Exp:
                    // this.Master.SignalSet.OnUpdateExpSignal.Dispatch(this.Master, oldValue, newValue);
                    break;
                case EAttrType.Shield:
                    // this.Master.SignalSet.OnShieldChangeSignal.Dispatch(oldValue, newValue);
                    break;
            }
        }

        protected override int GetTime()
        {
            return TimerFrameSys.time;
        }
    }
}