using System.Collections.Generic;

namespace Scarf.Moba
{
    public class ActiveSkillData: IPoolable
    {
        public Skill ActiveSkill;
        public int Arg1;
        public int Arg2;

        public void Reset()
        {
            this.ActiveSkill = null;
            this.Arg1 = 0;
            this.Arg2 = 0;
        }
    }

    public class UnitSkillComponent: CComponent
    {
        public Unit Master { get; private set; }

        private List<Skill> m_lstAllSkills = new List<Skill>();

        private List<ActiveSkillData> m_lstCurCastSkill = new List<ActiveSkillData>();

        public Skill CurSkill { get; private set; }

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

        public void CastSkill(int index, int arg1, int arg2)
        {
            if (!this.CanCastSkill(index, arg1, arg2))
                return;

            if (this.CanBreakSkill())
            {
                this.BreakSkill(false);
            }

            // 待放列表超过1个, 则清空所有
            if (this.m_lstCurCastSkill.Count >= 1)
            {
                foreach (ActiveSkillData data in this.m_lstCurCastSkill)
                {
                    CObjectPool<ActiveSkillData>.instance.SaveObject(data);
                }

                this.m_lstCurCastSkill.Clear();
            }

            var skill = this.GetSkill(index);
            var activeSkill = CObjectPool<ActiveSkillData>.instance.GetObject();
            activeSkill.ActiveSkill = skill;
            activeSkill.Arg1 = arg1;
            activeSkill.Arg2 = arg2;
            this.m_lstCurCastSkill.Add(activeSkill);

            if (CurSkill == null)
            {
                NextCastSkill();
            }

            // TODO 不同技能模板实现不同的流程.

            // TODO 如何显示技能描述, 引入代码机制进行读取. 类似这样的 %Skill1Index%, 这样的支持公式的插件.

            // TODO 升级技能, 换技能图么?

            // TODO 处理技能优先级, 闪现技能优先运行.

            // TODO 处理技能释放列表, 例如闪现技能可以插队执行, 变身技能还在执行, 并不算技能释放完毕.
        }

        void NextCastSkill()
        {
            if (this.Master.IsDie)
            {
                foreach (ActiveSkillData skillData in this.m_lstCurCastSkill)
                {
                    CObjectPool<ActiveSkillData>.instance.SaveObject(skillData);
                }

                this.m_lstCurCastSkill.Clear();
                return;
            }

            if (this.m_lstCurCastSkill.Count > 0)
            {
                var activeSkill = this.m_lstCurCastSkill[0];
                this.m_lstCurCastSkill.RemoveAt(0);
                // TODO 技能链处理
                this.CurSkill = activeSkill.ActiveSkill;
                this.CurSkill.SignalSet.OnSkillEndSignal.AddListener(CastEnd);
                this.CurSkill.CastSkill(activeSkill.Arg1, activeSkill.Arg2);

                CObjectPool<ActiveSkillData>.instance.SaveObject(activeSkill);
            }
        }

        void CastEnd(Skill skill)
        {
            this.CurSkill.SignalSet.OnSkillEndSignal.RemoveListener(CastEnd);
            this.CurSkill = null;

            if (this.m_lstCurCastSkill.Count <= 0)
            {
                return;
            }

            NextCastSkill();
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

        public bool CanBreakSkill()
        {
            return this.CurSkill?.CanBreakSkill() ?? true;
        }

        public void BreakSkill(bool bForce)
        {
            this.m_lstCurCastSkill.Clear();
            this.CurSkill?.BreakSkill(bForce);
        }

        public void PushSkillEvent(string animationName, string eventName)
        {
            for (int i = 0; i < this.m_lstAllSkills.Count; i++)
            {
                var skill = this.m_lstAllSkills[i];
                if (skill.CurAnimationName != animationName)
                {
                    return;
                }

                skill.PushAnimationEvent(animationName, eventName);
            }
        }
    }
}