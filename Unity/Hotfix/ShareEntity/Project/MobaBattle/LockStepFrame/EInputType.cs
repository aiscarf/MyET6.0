namespace ET
{
    public enum EInputType : int
    {
        None = 0, // 无任何操作, 也是操作
        Test = 1, // 单机用

        Move = 3, // 拖动移动摇杆
        MoveEnd = 40, // 松开移动摇杆

        Attack = 5, //            普通攻击
        Skill01 = 6, // 释放技能1  角色技能
        Skill02 = 7, // 释放技能2  宠物技能
        Skill03 = 8, // 释放技能3  战场技能
        Skill04 = 9, // 释放技能4  炮台图纸

        Passive01 = 16, // 学习被动技能1
        Passive02 = 17, // 学习被动技能2
        Passive03 = 18, // 学习被动技能3

        BattleSkill01 = 20, // 学习主动技能1
        BattleSkill02 = 21, // 学习主动技能2
        BattleSkill03 = 22, // 学习主动技能3 = 随机一个战场技能

        ReinforceSkill01 = 25, // 强化主动技能1
        ReinforceSkill02 = 26, // 强化主动技能2
        ReinforceSkill03 = 27, // 强化主动技能3

        UseProp = 30, // 使用道具

        UseArea = 31, // 使用地形

        OrderGather = 32, // 集合指令
        OrderAttack = 33, // 攻击指令
        OrderRetreat = 34, // 撤退指令
        OrderAnswer = 35, // 应答指令

        UseFace = 50, // 使用表情id

        ShopBuy = 51, // 商店购买
    }
}