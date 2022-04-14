namespace Scarf.Moba
{
    public class SkillSignalSet
    {
        /// <summary>
        /// 技能初始化
        /// </summary>
        public readonly Signal<Skill> OnSkillInitSignal = new Signal<Skill>();

        /// <summary>
        /// 技能解封
        /// </summary>
        public readonly Signal<Skill> OnSkillUnsealSignal = new Signal<Skill>();

        /// <summary>
        /// 技能施法开始
        /// </summary>
        public readonly Signal<Skill> OnSkillStartSignal = new Signal<Skill>();

        /// <summary>
        /// 技能发出打击
        /// </summary>
        public readonly Signal<Skill> OnSkillHitSignal = new Signal<Skill>();

        /// <summary>
        /// 技能发射子弹
        /// </summary>
        public readonly Signal<Skill> OnSkillMissileSignal = new Signal<Skill>();

        /// <summary>
        /// 技能被打断
        /// </summary>
        public readonly Signal<Skill> OnSkillBeBreakSignal = new Signal<Skill>();

        /// <summary>
        /// 技能被强制打断
        /// </summary>
        public readonly Signal<Skill> OnSkillBeForceBreakSignal = new Signal<Skill>();

        /// <summary>
        /// 技能施法结束
        /// </summary>
        public readonly Signal<Skill> OnSkillEndSignal = new Signal<Skill>();

        /// <summary>
        /// 技能销毁
        /// </summary>
        public readonly Signal<Skill> OnSkillDestroySignal = new Signal<Skill>();

        /// <summary>
        /// 开关类技能: 技能开
        /// </summary>
        public readonly Signal<Skill> OnCastToggleOnSignal = new Signal<Skill>();

        /// <summary>
        /// 开关类技能: 技能关
        /// </summary>
        public readonly Signal<Skill> OnCastToggleOffSignal = new Signal<Skill>();

        /// <summary>
        /// 蓄力技能: 释放
        /// </summary>
        public readonly Signal<Skill> OnCastXuLiReleaseSignal = new Signal<Skill>();

        /// <summary>
        /// 读条技能: 读条被打断
        /// </summary>
        public readonly Signal<Skill> OnDuTiaoBeBreakSignal = new Signal<Skill>();

        /// <summary>
        /// 读条技能: 读条结束
        /// </summary>
        public readonly Signal<Skill> OnDuTiaoEndSignal = new Signal<Skill>();

        /// <summary>
        /// 引导技能: 引导的Tick事件
        /// </summary>
        public readonly Signal<Skill> OnChannelThinkSignal = new Signal<Skill>();

        /// <summary>
        /// 引导技能: 结束
        /// </summary>
        public readonly Signal<Skill> OnChannelFinishSignal = new Signal<Skill>();

        public void Clear()
        {
            this.OnSkillInitSignal.Clear();
            this.OnSkillUnsealSignal.Clear();
            this.OnSkillStartSignal.Clear();
            this.OnSkillHitSignal.Clear();
            this.OnSkillMissileSignal.Clear();
            this.OnSkillBeBreakSignal.Clear();
            this.OnSkillBeForceBreakSignal.Clear();
            this.OnSkillEndSignal.Clear();
            this.OnSkillDestroySignal.Clear();
            this.OnCastToggleOnSignal.Clear();
            this.OnCastToggleOffSignal.Clear();
            this.OnCastXuLiReleaseSignal.Clear();
            this.OnDuTiaoBeBreakSignal.Clear();
            this.OnDuTiaoEndSignal.Clear();
            this.OnChannelThinkSignal.Clear();
            this.OnChannelFinishSignal.Clear();
        }
    }
}