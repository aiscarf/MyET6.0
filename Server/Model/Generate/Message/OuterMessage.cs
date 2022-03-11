using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(M2C_Reload))]
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_Login))]
	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string RealmToken { get; set; }

	}

	[ResponseType(nameof(R2C_Register))]
	[Message(OuterOpcode.C2R_Register)]
	[ProtoContract]
	public partial class C2R_Register: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Register)]
	[ProtoContract]
	public partial class R2C_Register: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_ServiceList))]
	[Message(OuterOpcode.C2R_ServiceList)]
	[ProtoContract]
	public partial class C2R_ServiceList: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.R2C_ServiceList)]
	[ProtoContract]
	public partial class R2C_ServiceList: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<GameService> ServiceList = new List<GameService>();

		[ProtoMember(2)]
		public List<GameRegion> RegionList = new List<GameRegion>();

	}

	[Message(OuterOpcode.GameService)]
	[ProtoContract]
	public partial class GameService: Object
	{
		[ProtoMember(1)]
		public int ServiceId { get; set; }

		[ProtoMember(2)]
		public string ServiceName { get; set; }

		[ProtoMember(3)]
		public long ServiceStartTime { get; set; }

	}

	[Message(OuterOpcode.GameRegion)]
	[ProtoContract]
	public partial class GameRegion: Object
	{
		[ProtoMember(1)]
		public int RegionId { get; set; }

		[ProtoMember(2)]
		public string RegionName { get; set; }

		[ProtoMember(3)]
		public int State { get; set; }

		[ProtoMember(4)]
		public string Address { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string RealmToken { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateToken { get; set; }

		[ProtoMember(2)]
		public PlayerInfo PlayerInfo { get; set; }

		[ProtoMember(3)]
		public long Time { get; set; }

		[ProtoMember(4)]
		public List<FriendInfo> Friends = new List<FriendInfo>();

		[ProtoMember(5)]
		public int PlayerState { get; set; }

	}

	[ResponseType(nameof(G2C_ReconnectGate))]
	[Message(OuterOpcode.C2G_ReconnectGate)]
	[ProtoContract]
	public partial class C2G_ReconnectGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string GateToken { get; set; }

	}

	[Message(OuterOpcode.G2C_ReconnectGate)]
	[ProtoContract]
	public partial class G2C_ReconnectGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateToken { get; set; }

		[ProtoMember(2)]
		public PlayerInfo PlayerInfo { get; set; }

		[ProtoMember(3)]
		public long Time { get; set; }

		[ProtoMember(4)]
		public List<FriendInfo> Friends = new List<FriendInfo>();

	}

	[Message(OuterOpcode.G2C_OnPlayerKickOut)]
	[ProtoContract]
	public partial class G2C_OnPlayerKickOut: Object, IMessage
	{
	}

	[Message(OuterOpcode.PlayerInfo)]
	[ProtoContract]
	public partial class PlayerInfo: Object
	{
		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string Nickname { get; set; }

		[ProtoMember(3)]
		public string FriendCode { get; set; }

		[ProtoMember(4)]
		public int Gold { get; set; }

		[ProtoMember(5)]
		public int Diamond { get; set; }

		[ProtoMember(6)]
		public int HeroId { get; set; }

		[ProtoMember(7)]
		public List<HeroInfo> Heros = new List<HeroInfo>();

		[ProtoMember(8)]
		public int PetId { get; set; }

		[ProtoMember(9)]
		public List<PetInfo> Pets = new List<PetInfo>();

		[ProtoMember(10)]
		public List<int> PublicTowerSkins = new List<int>();

		[ProtoMember(11)]
		public List<int> PublicFaces = new List<int>();

		[ProtoMember(12)]
		public List<int> PublicVoices = new List<int>();

		[ProtoMember(13)]
		public List<BagInfo> Bags = new List<BagInfo>();

		[ProtoMember(14)]
		public HeadInfo HeadInfo { get; set; }

		[ProtoMember(15)]
		public List<int> HeadIdArr = new List<int>();

		[ProtoMember(16)]
		public List<int> FrameIdArr = new List<int>();

		[ProtoMember(17)]
		public List<int> ShowIdArr = new List<int>();

		[ProtoMember(18)]
		public string TeamId { get; set; }

		[ProtoMember(19)]
		public List<TeamPlayerInfo> TeamUsers = new List<TeamPlayerInfo>();

		[ProtoMember(20)]
		public int Score { get; set; }

		[ProtoMember(21)]
		public RoleWinInfo WinInfo { get; set; }

		[ProtoMember(22)]
		public int Guide { get; set; }

		[ProtoMember(23)]
		public TaskInfo TaskInfo { get; set; }

		[ProtoMember(24)]
		public List<TaskSingleInfo> TaskOverall = new List<TaskSingleInfo>();

		[ProtoMember(25)]
		public List<int> TaskIdArr = new List<int>();

		[ProtoMember(26)]
		public int Charge { get; set; }

		[ProtoMember(27)]
		public MinerInfo MinerInfo { get; set; }

		[ProtoMember(28)]
		public int MaxScore { get; set; }

		[ProtoMember(29)]
		public List<int> SeasonAward = new List<int>();

		[ProtoMember(30)]
		public int FriendAskOk { get; set; }

		[ProtoMember(31)]
		public List<MapInfo> MapList = new List<MapInfo>();

		[ProtoMember(32)]
		public int MapId { get; set; }

		[ProtoMember(33)]
		public List<int> UnlockedSkills = new List<int>();

		[ProtoMember(34)]
		public List<int> TimeLimitItems = new List<int>();

		[ProtoMember(35)]
		public List<int> TeamInviteBlack = new List<int>();

		[ProtoMember(36)]
		public int SeasonEndTime { get; set; }

		[ProtoMember(37)]
		public int IfTeamInvite { get; set; }

	}

	[Message(OuterOpcode.HeroInfo)]
	[ProtoContract]
	public partial class HeroInfo: Object
	{
		[ProtoMember(1)]
		public int HeroId { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

		[ProtoMember(3)]
		public int SkinId { get; set; }

		[ProtoMember(4)]
		public List<int> Skins = new List<int>();

		[ProtoMember(5)]
		public int TowerSkinId { get; set; }

		[ProtoMember(6)]
		public List<int> TowerSkins = new List<int>();

		[ProtoMember(7)]
		public int PieceNum { get; set; }

		[ProtoMember(8)]
		public int Exp { get; set; }

		[ProtoMember(9)]
		public List<int> Faces = new List<int>();

		[ProtoMember(10)]
		public List<int> Voices = new List<int>();

		[ProtoMember(11)]
		public List<FaceVoice> FaceVoice = new List<FaceVoice>();

		[ProtoMember(12)]
		public int TimeLimit { get; set; }

	}

	[Message(OuterOpcode.FaceVoice)]
	[ProtoContract]
	public partial class FaceVoice: Object
	{
		[ProtoMember(1)]
		public int Index { get; set; }

		[ProtoMember(2)]
		public int Face { get; set; }

		[ProtoMember(3)]
		public int Voice { get; set; }

	}

	[Message(OuterOpcode.PetInfo)]
	[ProtoContract]
	public partial class PetInfo: Object
	{
		[ProtoMember(1)]
		public int PetId { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

		[ProtoMember(3)]
		public int SkinId { get; set; }

		[ProtoMember(4)]
		public List<int> Skins = new List<int>();

		[ProtoMember(5)]
		public int PieceNum { get; set; }

		[ProtoMember(6)]
		public int TimeLimit { get; set; }

	}

	[Message(OuterOpcode.HeadInfo)]
	[ProtoContract]
	public partial class HeadInfo: Object
	{
		[ProtoMember(1)]
		public int HeadId { get; set; }

		[ProtoMember(2)]
		public int FrameId { get; set; }

		[ProtoMember(3)]
		public int ShowId { get; set; }

	}

	[Message(OuterOpcode.RoleWinInfo)]
	[ProtoContract]
	public partial class RoleWinInfo: Object
	{
		[ProtoMember(1)]
		public int MaxScore { get; set; }

		[ProtoMember(2)]
		public int Mvp { get; set; }

		[ProtoMember(3)]
		public int Kill2 { get; set; }

		[ProtoMember(4)]
		public int Kill3 { get; set; }

		[ProtoMember(5)]
		public List<int> MapMain = new List<int>();

		[ProtoMember(6)]
		public List<int> MapPk = new List<int>();

		[ProtoMember(7)]
		public List<int> MapLuan = new List<int>();

		[ProtoMember(8)]
		public List<int> MapBoss = new List<int>();

	}

	[Message(OuterOpcode.TaskInfo)]
	[ProtoContract]
	public partial class TaskInfo: Object
	{
		[ProtoMember(1)]
		public int DayExp { get; set; }

		[ProtoMember(2)]
		public int WeekExp { get; set; }

		[ProtoMember(3)]
		public int AwardGet { get; set; }

	}

	[Message(OuterOpcode.MinerInfo)]
	[ProtoContract]
	public partial class MinerInfo: Object
	{
		[ProtoMember(1)]
		public List<int> IdArr = new List<int>();

		[ProtoMember(2)]
		public int Lv { get; set; }

		[ProtoMember(3)]
		public int Exp { get; set; }

		[ProtoMember(4)]
		public List<int> Award1 = new List<int>();

		[ProtoMember(5)]
		public List<int> Award2 = new List<int>();

		[ProtoMember(6)]
		public List<int> Award3 = new List<int>();

		[ProtoMember(7)]
		public List<int> Unlocked = new List<int>();

		[ProtoMember(8)]
		public List<int> Refreshed = new List<int>();

		[ProtoMember(9)]
		public List<TaskSingleInfo> Tasks = new List<TaskSingleInfo>();

		[ProtoMember(10)]
		public bool MinerStop { get; set; }

	}

	[Message(OuterOpcode.BagInfo)]
	[ProtoContract]
	public partial class BagInfo: Object
	{
		[ProtoMember(1)]
		public int Id { get; set; }

		[ProtoMember(2)]
		public int Num { get; set; }

		[ProtoMember(3)]
		public int Time { get; set; }

	}

	[Message(OuterOpcode.TeamPlayerInfo)]
	[ProtoContract]
	public partial class TeamPlayerInfo: Object
	{
		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public int HeroId { get; set; }

		[ProtoMember(3)]
		public int SkinId { get; set; }

		[ProtoMember(4)]
		public ETeamPlayerState State { get; set; }

		[ProtoMember(5)]
		public string Nickname { get; set; }

	}

	public enum ETeamPlayerState
	{
		None									 = 0,  // 大厅界面
		Offline									 = 1,  // 离线
		HeroChoose								 = 2,  // 选择英雄
		Ready									 = 3,  // 准备就绪
		Match									 = 4,  // 匹配中
		Game									 = 5,  // 对战中
	}

	[Message(OuterOpcode.TaskSingleInfo)]
	[ProtoContract]
	public partial class TaskSingleInfo: Object
	{
		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public int Num { get; set; }

		[ProtoMember(3)]
		public int HasGet { get; set; }

	}

	[Message(OuterOpcode.MapInfo)]
	[ProtoContract]
	public partial class MapInfo: Object
	{
		[ProtoMember(1)]
		public int Id { get; set; }

		[ProtoMember(2)]
		public int Arrange { get; set; }

		[ProtoMember(3)]
		public int ID_Child { get; set; }

		[ProtoMember(5)]
		public int StartTime { get; set; }

		[ProtoMember(6)]
		public int EndTime { get; set; }

	}

	[Message(OuterOpcode.FriendInfo)]
	[ProtoContract]
	public partial class FriendInfo: Object
	{
		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string Nickname { get; set; }

		[ProtoMember(3)]
		public int HeadId { get; set; }

		[ProtoMember(4)]
		public int Score { get; set; }

		[ProtoMember(5)]
		public int Time { get; set; }

		[ProtoMember(6)]
		public EFriendState State { get; set; }

		[ProtoMember(7)]
		public EFriendFromT FromT { get; set; }

		[ProtoMember(8)]
		public int FrameId { get; set; }

		[ProtoMember(9)]
		public string TeamId { get; set; }

		[ProtoMember(10)]
		public List<string> ChatList = new List<string>();

	}

	public enum EFriendState
	{
		None									 = 0,
		Ask										 = 1,
		Friend									 = 2,
	}

	public enum EFriendFromT
	{
		None									 = 0,
		System									 = 1, // 系统推荐
		Search									 = 2, // 标签搜索
		Game									 = 3, // 对战日志
	}

	[ResponseType(nameof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[ResponseType(nameof(G2C_StartReady))]
	[Message(OuterOpcode.C2G_StartReady)]
	[ProtoContract]
	public partial class C2G_StartReady: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_StartReady)]
	[ProtoContract]
	public partial class G2C_StartReady: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_StartReady))]
	[Message(OuterOpcode.C2G_CancelReady)]
	[ProtoContract]
	public partial class C2G_CancelReady: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_CancelReady)]
	[ProtoContract]
	public partial class G2C_CancelReady: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.G2C_OnStartMatch)]
	[ProtoContract]
	public partial class G2C_OnStartMatch: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int PlayerNum { get; set; }

		[ProtoMember(2)]
		public int NeedNum { get; set; }

	}

	[Message(OuterOpcode.G2C_OnCancelMatch)]
	[ProtoContract]
	public partial class G2C_OnCancelMatch: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int PlayerUid { get; set; }

	}

	[Message(OuterOpcode.G2C_OnUpdateMatchPlayerNum)]
	[ProtoContract]
	public partial class G2C_OnUpdateMatchPlayerNum: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int PlayerNum { get; set; }

	}

// 以下是战斗的协议.
	[Message(OuterOpcode.G2C_OnGameStart)]
	[ProtoContract]
	public partial class G2C_OnGameStart: Object, IMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public int MapId { get; set; }

		[ProtoMember(3)]
		public int RoomId { get; set; }

		[ProtoMember(4)]
		public int RandomSeed { get; set; }

		[ProtoMember(5)]
		public List<MobaPlayerInfo> Players = new List<MobaPlayerInfo>();

		[ProtoMember(7)]
		public string Host { get; set; }

		[ProtoMember(8)]
		public int Port { get; set; }

	}

	[Message(OuterOpcode.MobaPlayerInfo)]
	[ProtoContract]
	public partial class MobaPlayerInfo: Object
	{
		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string Nickname { get; set; }

		[ProtoMember(3)]
		public int HeroId { get; set; }

		[ProtoMember(4)]
		public int HeroSkinId { get; set; }

		[ProtoMember(5)]
		public int TowerSkinId { get; set; }

		[ProtoMember(6)]
		public int PetId { get; set; }

		[ProtoMember(7)]
		public int PetSkinId { get; set; }

		[ProtoMember(8)]
		public int Score { get; set; }

		[ProtoMember(9)]
		public int ChairId { get; set; }

		[ProtoMember(10)]
		public int Camp { get; set; }

		[ProtoMember(11)]
		public int HeadId { get; set; }

		[ProtoMember(12)]
		public int FrameId { get; set; }

		[ProtoMember(13)]
		public int ShowId { get; set; }

		[ProtoMember(14)]
		public int HeroLv { get; set; }

		[ProtoMember(15)]
		public int PetLv { get; set; }

		[ProtoMember(16)]
		public List<int> UnlockedSkill = new List<int>();

	}

	[ResponseType(nameof(B2C_GameMainEnter))]
	[Message(OuterOpcode.C2B_GameMainEnter)]
	[ProtoContract]
	public partial class C2B_GameMainEnter: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RoomId { get; set; }

		[ProtoMember(2)]
		public long Uid { get; set; }

		[ProtoMember(3)]
		public string Token { get; set; }

	}

	[Message(OuterOpcode.B2C_GameMainEnter)]
	[ProtoContract]
	public partial class B2C_GameMainEnter: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(B2C_BattleReady))]
	[Message(OuterOpcode.C2B_BattleReady)]
	[ProtoContract]
	public partial class C2B_BattleReady: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.B2C_BattleReady)]
	[ProtoContract]
	public partial class B2C_BattleReady: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(B2C_FrameMsg))]
	[Message(OuterOpcode.C2B_FrameMsg)]
	[ProtoContract]
	public partial class C2B_FrameMsg: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int FrameId { get; set; }

		[ProtoMember(2)]
		public FrameMsg Msg { get; set; }

	}

	[Message(OuterOpcode.B2C_FrameMsg)]
	[ProtoContract]
	public partial class B2C_FrameMsg: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.B2C_OnFrame)]
	[ProtoContract]
	public partial class B2C_OnFrame: Object, IMessage
	{
		[ProtoMember(1)]
		public int FrameId { get; set; }

		[ProtoMember(2)]
		public List<FrameMsg> Msg = new List<FrameMsg>();

	}

	[Message(OuterOpcode.FrameMsg)]
	[ProtoContract]
	public partial class FrameMsg: Object
	{
		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public int Optype { get; set; }

		[ProtoMember(3)]
		public int Arg1 { get; set; }

		[ProtoMember(4)]
		public int Arg2 { get; set; }

	}

	[ResponseType(nameof(B2C_Ping))]
	[Message(OuterOpcode.C2B_Ping)]
	[ProtoContract]
	public partial class C2B_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.B2C_Ping)]
	[ProtoContract]
	public partial class B2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

}
