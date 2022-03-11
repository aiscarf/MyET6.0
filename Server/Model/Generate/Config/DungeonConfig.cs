using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class DungeonConfigCategory : ProtoObject
    {
        public static DungeonConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DungeonConfig> dict = new Dictionary<int, DungeonConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DungeonConfig> list = new List<DungeonConfig>();
		
        public DungeonConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (DungeonConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DungeonConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DungeonConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DungeonConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DungeonConfig> GetAll()
        {
            return this.dict;
        }

        public DungeonConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DungeonConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Name { get; set; }
		[ProtoMember(3)]
		public int NeedPlayerNum { get; set; }
		[ProtoMember(4)]
		public string ConfigPath { get; set; }

	}
}
