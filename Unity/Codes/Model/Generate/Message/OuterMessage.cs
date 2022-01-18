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
		public string Address { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

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
		public long PlayerId { get; set; }

	}

// Match服下发通知客户端.
	[Message(OuterOpcode.M2C_OnGameStart)]
	[ProtoContract]
	public partial class M2C_OnGameStart: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int code { get; set; }

		[ProtoMember(2)]
		public int gameT { get; set; }

		[ProtoMember(3)]
		public int roomId { get; set; }

		[ProtoMember(4)]
		public int token { get; set; }

		[ProtoMember(5)]
		public int randomSeed { get; set; }

		[ProtoMember(6)]
		public List<PlayerInfo> players = new List<PlayerInfo>();

		[ProtoMember(7)]
		public string host { get; set; }

		[ProtoMember(8)]
		public int port { get; set; }

	}

	[Message(OuterOpcode.PlayerInfo)]
	[ProtoContract]
	public partial class PlayerInfo: Object
	{
		[ProtoMember(1)]
		public int uid { get; set; }

		[ProtoMember(2)]
		public string nickname { get; set; }

		[ProtoMember(3)]
		public int heroId { get; set; }

		[ProtoMember(4)]
		public int heroSkinId { get; set; }

		[ProtoMember(5)]
		public int towerSkinId { get; set; }

		[ProtoMember(6)]
		public int petId { get; set; }

		[ProtoMember(7)]
		public int petSkinId { get; set; }

		[ProtoMember(8)]
		public int score { get; set; }

		[ProtoMember(9)]
		public int chairId { get; set; }

		[ProtoMember(10)]
		public int camp { get; set; }

		[ProtoMember(11)]
		public int headId { get; set; }

		[ProtoMember(12)]
		public int frameId { get; set; }

		[ProtoMember(13)]
		public int showId { get; set; }

		[ProtoMember(14)]
		public int heroLv { get; set; }

		[ProtoMember(15)]
		public int petLv { get; set; }

		[ProtoMember(16)]
		public List<int> unlockedSkill = new List<int>();

	}

// moba战斗部分使用的消息.
	[Message(OuterOpcode.C2B_GameMainEnter)]
	[ProtoContract]
	public partial class C2B_GameMainEnter: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RoomId { get; set; }

		[ProtoMember(2)]
		public int Uid { get; set; }

		[ProtoMember(3)]
		public int Token { get; set; }

	}

	[Message(OuterOpcode.B2C_GameMainEnter)]
	[ProtoContract]
	public partial class B2C_GameMainEnter: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.C2B_FrameMsg)]
	[ProtoContract]
	public partial class C2B_FrameMsg: Object, IActorLocationRequest
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
	public partial class B2C_FrameMsg: Object, IActorLocationResponse
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
	public partial class B2C_OnFrame: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

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
		public int Uid { get; set; }

		[ProtoMember(2)]
		public int Optype { get; set; }

		[ProtoMember(3)]
		public int Arg1 { get; set; }

		[ProtoMember(4)]
		public int Arg2 { get; set; }

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

}
