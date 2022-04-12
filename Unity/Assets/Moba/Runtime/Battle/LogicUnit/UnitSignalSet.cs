namespace Scarf.Moba
{
    public class UnitSignalSet
    {
        /// <summary>
        /// 进攻伤害阶段0
        /// </summary>
        public readonly Signal<DamageInfo> OnDamageStage0Signal = new Signal<DamageInfo>();

        /// <summary>
        /// 进攻伤害阶段1
        /// </summary>
        public readonly Signal<DamageInfo> OnDamageStage1Signal = new Signal<DamageInfo>();

        /// <summary>
        /// 进攻伤害阶段2
        /// </summary>
        public readonly Signal<DamageInfo> OnDamageStage2Signal = new Signal<DamageInfo>();

        /// <summary>
        /// 受伤阶段0
        /// </summary>
        public readonly Signal<DamageInfo> OnBeHurtStage0Signal = new Signal<DamageInfo>();

        /// <summary>
        /// 受伤阶段1
        /// </summary>
        public readonly Signal<DamageInfo> OnBeHurtStage1Signal = new Signal<DamageInfo>();

        /// <summary>
        /// 受伤阶段2
        /// </summary>
        public readonly Signal<DamageInfo> OnBeHurtStage2Signal = new Signal<DamageInfo>();

        /// <summary>
        /// 当命中敌人时, 且对它的真正扣血
        /// </summary>
        public readonly Signal<DamageInfo> OnHitTargetSignal = new Signal<DamageInfo>();

        /// <summary>
        /// 该角色受到伤害
        /// </summary>
        public readonly Signal<DamageInfo> OnBeHurtSignal = new Signal<DamageInfo>();

        /// <summary>
        /// 回血逻辑
        /// </summary>
        public readonly Signal<DamageInfo> OnRecoverHpSignal = new Signal<DamageInfo>();

        /// <summary>
        /// 当命中敌人时, 且对它造成回血
        /// </summary>
        public readonly Signal<DamageInfo> OnRecoverTargetSignal = new Signal<DamageInfo>();

        /// <summary>
        /// 击杀时
        /// </summary>
        public readonly Signal<DamageInfo> OnKillSignal = new Signal<DamageInfo>();

        /// <summary>
        /// 干预获取属性
        /// </summary>
        public readonly Signal<Unit, PreAttr> OnPreGetAttrSignal = new Signal<Unit, PreAttr>();

        /// <summary>
        /// 干预设置属性
        /// </summary>
        public readonly Signal<Unit, PreAttr> OnPreSetAttrSignal = new Signal<Unit, PreAttr>();

        public void Clear()
        {
            this.OnDamageStage0Signal.Clear();
            this.OnDamageStage1Signal.Clear();
            this.OnDamageStage2Signal.Clear();

            this.OnBeHurtStage0Signal.Clear();
            this.OnBeHurtStage1Signal.Clear();
            this.OnBeHurtStage2Signal.Clear();

            this.OnHitTargetSignal.Clear();
            this.OnBeHurtSignal.Clear();
            this.OnRecoverHpSignal.Clear();
            this.OnRecoverTargetSignal.Clear();
            this.OnKillSignal.Clear();

            this.OnPreGetAttrSignal.Clear();
            this.OnPreSetAttrSignal.Clear();
        }
    }
}