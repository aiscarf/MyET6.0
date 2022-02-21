using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [BsonIgnoreExtraElements]
    public class DBAccountInfo : Entity
    {
        // 用户名
        public string Account { get; set; }

        // 密码
        public string Password { get; set; }
    }
}