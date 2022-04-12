using System.Collections.Generic;

namespace Scarf.Moba
{
    public class UnitSkillComponent: CComponent
    {
        public Unit Master { get; private set; }

        // DONE: 技能图调度器, 里面有多个技能.

        // TODO: 角色相关的所有技能数据.
        private List<Skill> m_lstAllSkills = new List<Skill>();

        // TODO 动画bind技能.

        protected override void OnInit()
        {
            this.Master = this.Parent as Unit;
        }

        protected override void OnDestroy()
        {
            for (int i = 0; i < this.m_lstAllSkills.Count; i++)
            {
                var skill = m_lstAllSkills[i];
                skill.Dispose();
            }

            this.m_lstAllSkills.Clear();
            this.Master = null;
        }

        public override void OnFrameSyncUpdate(int delta)
        {
            for (int i = 0; i < this.m_lstAllSkills.Count; i++)
            {
                var skill = m_lstAllSkills[i];
                skill.OnFrameSyncUpdate(delta);
            }
        }

        // TODO 被动技能, 学习了便天生拥有, 如何升级技能.
        public void CastSkill(int index, int arg1, int arg2)
        {
            if (!this.CanCastSkill(index, arg1, arg2))
                return;

            // TODO 
            // TODO 如何显示技能描述, 引入代码机制进行读取. 类似这样的 %Skill1Index%, 这样的支持公式的插件.
            // TODO 升级技能, 换技能图么?
            // TODO 处理技能优先级, 1技能不如2技能.
            // TODO 处理技能释放列表, 例如闪现技能可以插队执行, 变身技能还在执行, 并不算技能释放完毕.
        }

        public Skill GetSkill(int index)
        {
            if (index < 0 || index >= this.m_lstAllSkills.Count)
                return null;
            return this.m_lstAllSkills[index];
        }

        public void AddSkill(int index, Skill skill)
        {
            if (index < 0 || skill == null)
            {
                return;
            }

            while (index >= this.m_lstAllSkills.Count)
            {
                this.m_lstAllSkills.Add(null);
            }

            if (this.m_lstAllSkills[index] != null)
            {
                return;
            }

            this.m_lstAllSkills[index] = skill;

            // DONE: 如果是被动技能, 则默认释放.
            if (skill.SkillType != ESkillType.EPassive)
                return;
            CastSkill(index, 0, 0);
        }

        public void LevelSkill(int index)
        {
            // TODO 如何升级一个技能.
        }

        public bool CanCastSkill(int index, int arg1, int arg2)
        {
            var skill = this.GetSkill(index);
            if (skill == null)
                return false;

            // TODO 要释放的技能已经处于待释放列表.
            // if (m_lstCurCastSkill.Count > 0)
            // {
            //     for (int i = 0; i < m_lstCurCastSkill.Count; i++)
            //     {
            //         if (m_lstCurCastSkill[i].ActiveSkill == skill)
            //         {
            //             return false;
            //         }
            //     }
            // }

            return skill.CanCastSkill(arg1, arg2);
        }
    }
}