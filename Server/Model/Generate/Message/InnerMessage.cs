using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(ObjectQueryResponse))]
	[Message(InnerOpcode.ObjectQueryRequest)]
	[ProtoContract]
	public partial class ObjectQueryRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(A2M_Reload))]
	[Message(InnerOpcode.M2A_Reload)]
	[ProtoContract]
	public partial class M2A_Reload: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerOpcode.A2M_Reload)]
	[ProtoContract]
	public partial class A2M_Reload: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectAddResponse))]
	[Message(InnerOpcode.ObjectAddRequest)]
	[ProtoContract]
	public partial class ObjectAddRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectAddResponse)]
	[ProtoContract]
	public partial class ObjectAddResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectLockResponse))]
	[Message(InnerOpcode.ObjectLockRequest)]
	[ProtoContract]
	public partial class ObjectLockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

		[ProtoMember(3)]
		public int Time { get; set; }

	}

	[Message(InnerOpcode.ObjectLockResponse)]
	[ProtoContract]
	public partial class ObjectLockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectUnLockResponse))]
	[Message(InnerOpcode.ObjectUnLockRequest)]
	[ProtoContract]
	public partial class ObjectUnLockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long OldInstanceId { get; set; }

		[ProtoMember(3)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectUnLockResponse)]
	[ProtoContract]
	public partial class ObjectUnLockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectRemoveResponse))]
	[Message(InnerOpcode.ObjectRemoveRequest)]
	[ProtoContract]
	public partial class ObjectRemoveRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectRemoveResponse)]
	[ProtoContract]
	public partial class ObjectRemoveResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectGetResponse))]
	[Message(InnerOpcode.ObjectGetRequest)]
	[ProtoContract]
	public partial class ObjectGetRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectGetResponse)]
	[ProtoContract]
	public partial class ObjectGetResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(R2G_PlayerOnlineResponse))]
	[Message(InnerOpcode.G2R_PlayerOnlineRequest)]
	[ProtoContract]
	public partial class G2R_PlayerOnlineRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string RealmToken { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	[Message(InnerOpcode.R2G_PlayerOnlineResponse)]
	[ProtoContract]
	public partial class R2G_PlayerOnlineResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2G_PlayerOfflineResponse))]
	[Message(InnerOpcode.G2R_PlayerOfflineRequest)]
	[ProtoContract]
	public partial class G2R_PlayerOfflineRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

		[ProtoMember(2)]
		public string RealmToken { get; set; }

	}

	[Message(InnerOpcode.R2G_PlayerOfflineResponse)]
	[ProtoContract]
	public partial class R2G_PlayerOfflineResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2R_PlayerKickOutResponse))]
	[Message(InnerOpcode.R2G_PlayerKickOutRequest)]
	[ProtoContract]
	public partial class R2G_PlayerKickOutRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

	}

	[Message(InnerOpcode.G2R_PlayerKickOutResponse)]
	[ProtoContract]
	public partial class G2R_PlayerKickOutResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2G_StartMatch))]
	[Message(InnerOpcode.G2M_StartMatch)]
	[ProtoContract]
	public partial class G2M_StartMatch: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

	}

	[Message(InnerOpcode.M2G_StartMatch)]
	[ProtoContract]
	public partial class M2G_StartMatch: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2G_CancelMatch))]
	[Message(InnerOpcode.G2M_CancelMatch)]
	[ProtoContract]
	public partial class G2M_CancelMatch: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Uid { get; set; }

	}

	[Message(InnerOpcode.M2G_CancelMatch)]
	[ProtoContract]
	public partial class M2G_CancelMatch: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(InnerOpcode.M2G_OnSuccessMatch)]
	[ProtoContract]
	public partial class M2G_OnSuccessMatch: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public List<long> Uids = new List<long>();

	}

	[ResponseType(nameof(B2G_CreateBattle))]
	[Message(InnerOpcode.G2B_CreateBattle)]
	[ProtoContract]
	public partial class G2B_CreateBattle: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public List<long> Uids = new List<long>();

	}

	[Message(InnerOpcode.B2G_CreateBattle)]
	[ProtoContract]
	public partial class B2G_CreateBattle: Object, IActorResponse
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

	}

}
