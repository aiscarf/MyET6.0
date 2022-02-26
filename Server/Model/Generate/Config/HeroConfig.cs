using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class HeroConfigCategory : ProtoObject
    {
        public static HeroConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, HeroConfig> dict = new Dictionary<int, HeroConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<HeroConfig> list = new List<HeroConfig>();
		
        public HeroConfigCategory()
        {
            Instance = this;
        }
		
        public override void EndInit()
        {
            foreach (HeroConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public HeroConfig Get(int id)
        {
            this.dict.TryGetValue(id, out HeroConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (HeroConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, HeroConfig> GetAll()
        {
            return this.dict;
        }

        public HeroConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class HeroConfig: ProtoObject, IConfig
	{
		[ProtoMember(1)]
		public int Id { get; set; }
		[ProtoMember(2)]
		public string Name { get; set; }
		[ProtoMember(3)]
		public int Camp { get; set; }
		[ProtoMember(5)]
		public string Occupation { get; set; }
		[ProtoMember(6)]
		public int[] HeroSkin { get; set; }
		[ProtoMember(7)]
		public int HeadId { get; set; }
		[ProtoMember(8)]
		public int FastfaceId { get; set; }
		[ProtoMember(9)]
		public int FastSound { get; set; }
		[ProtoMember(10)]
		public int AttackType { get; set; }
		[ProtoMember(11)]
		public int Attack { get; set; }
		[ProtoMember(12)]
		public int Speed { get; set; }
		[ProtoMember(13)]
		public int Hp { get; set; }
		[ProtoMember(14)]
		public int PhysicalDefense { get; set; }
		[ProtoMember(15)]
		public int MagicDefense { get; set; }
		[ProtoMember(16)]
		public int AttackSpeed { get; set; }
		[ProtoMember(17)]
		public int AttackId { get; set; }
		[ProtoMember(18)]
		public int SkillId { get; set; }
		[ProtoMember(19)]
		public int[] Hero_Face { get; set; }
		[ProtoMember(20)]
		public int[] Hero_Line { get; set; }
		[ProtoMember(22)]
		public int HpUpgrade { get; set; }
		[ProtoMember(23)]
		public int AttackUpgrade { get; set; }
		[ProtoMember(24)]
		public int PhysicalDefUpgrade { get; set; }
		[ProtoMember(25)]
		public int MagicDefUpgrade { get; set; }
		[ProtoMember(26)]
		public int AttackSpeedUpgrade { get; set; }

	}
}
