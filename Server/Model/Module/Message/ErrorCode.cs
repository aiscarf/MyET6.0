namespace ET
{
    public static partial class ErrorCode
    {
        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误
        
        // 110000以下的错误请看ErrorCore.cs
        
        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常
        
        public const int ERR_Success = 0;
        
        public const int ERR_SignError = 10000;
        
        public const int ERR_DISCONNECTED                        = 210000; // 登录-断开连接
        public const int ERR_LOGIN_BANNED                        = 210001; // 登录-被黑名单
        public const int ERR_LOGIN_ACCOUNT_OR_PASSWORD           = 210002; // 登录-不存在该账户or密码
        public const int ERR_LOGIN_UNKNOWN_ACCOUNT               = 210003; // 登录-不存在该用户
        public const int ERR_LOGIN_INCORRECT_PASSWORD            = 210004; // 登录-错误的密码
        public const int ERR_LOGIN_ALREADY_ONLINE                = 210005; // 登录-账户已经在线
        public const int ERR_LOGIN_BUSY                          = 210006; // 登录-繁忙
        public const int ERR_LOGIN_LOCKED_ENFORCED               = 210007; // 登录-账户被锁定
        public const int ERR_LOGIN_VERSION_INVALID               = 210008; // 登录-版本错误
        public const int ERR_LOGIN_REGISTER_ALREADY_ACCOUNT      = 210009; // 登录-注册-账户已经存在
        public const int ERR_LOGIN_VALID_REALMTOKEN              = 210010; // 登录-Token不存在
    }
}