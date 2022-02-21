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

}
