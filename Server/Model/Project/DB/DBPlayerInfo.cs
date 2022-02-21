using System.Collections.Generic;

namespace ET
{
    public class DBPlayerInfo: Entity
    {
        public string Nickname { get; set; }
        public int Gold { get; set; }
        public int Diamond { get; set; }
        public int Charge { get; set; }
        public int HeroId { get; set; }
        
        public List<HeroInfo> HeroInfos { get; set; }
        public int PetId { get; set; }
        public List<PetInfo> PetInfos { get; set; }
        public List<BagInfo> BagInfos { get; set; }
        public HeadInfo HeadInfo { get; set; }
        
        /// <summary> 头像Id, 头像框Id, 展示Id </summary>
        public List<int> HeadIds { get; set; }
        public List<int> FrameIds { get; set; }
        public List<int> ShowIds { get; set; }
        
        /// <summary> 本赛季杯数 </summary>
        public int Score { get; set; }
        
        /// <summary> 战绩信息 </summary>
        public RoleWinInfo RoleWinInfo { get; set; }
        
        /// <summary> 当前新手引导的步骤 </summary>
        public int Guide { get; set; }
        
        /// <summary> 任务相关信息 </summary>
        public TaskInfo TaskInfo { get; set; }
        
        /// <summary> 各任务进度及领取情况 </summary>
        public List<TaskSingleInfo> Tasks { get; set; }
        
        /// <summary> 本赛季达到的最高分 </summary>
        public int MaxScore { get; set; }
        
        /// <summary> 本赛季积分奖励领取情况 </summary>
        public List<int> SeasonAwards { get; set; }
        
        /// <summary> 当前选择的副本Id </summary>
        public int MapId { get; set; }
        
        /// <summary> 已解锁的技能Id </summary>
        public List<int> UnlockedSkills { get; set; }
        
        /// <summary> 限时体验的东西 </summary>
        public List<int> TimeLimitItems { get; set; }
        
        /// <summary> 组队邀请的屏蔽列表 </summary>
        public List<int> TeamInviteBlack { get; set; }
        
        /// <summary> 是否接收好友请求 1接收 0不接收 </summary>
        public int FriendAsOk { get; set; }
        
        /// <summary> 是否接收组队邀请 1接收 0拒绝 </summary>
        public int IfTeamInvite { get; set; }
    }
}