using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class InitAccountConfigCategory : ProtoObject
    {
        public static InitAccountConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, InitAccountConfig> dict = new Dictionary<int, InitAccountConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<InitAccountConfig> list = new List<InitAccountConfig>();
		
        public InitAccountConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (InitAccountConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public InitAccountConfig Get(int id)
        {
            this.dict.TryGetValue(id, out InitAccountConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (InitAccountConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, InitAccountConfig> GetAll()
        {
            return this.dict;
        }

        public InitAccountConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class InitAccountConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public int Gold { get; set; }
		[ProtoMember(3)]
		public int Diamond { get; set; }
		[ProtoMember(4)]
		public int ActivityMoney { get; set; }
		[ProtoMember(5)]
		public int[] Hero { get; set; }
		[ProtoMember(6)]
		public int[] Item { get; set; }
		[ProtoMember(7)]
		public int[] Head { get; set; }
		[ProtoMember(8)]
		public int[] Frame { get; set; }
		[ProtoMember(9)]
		public int[] Face { get; set; }
		[ProtoMember(10)]
		public int[] Sound { get; set; }
		[ProtoMember(11)]
		public int[] NewcomerPackage { get; set; }

	}
}
