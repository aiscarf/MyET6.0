namespace ET
{
    public enum ESessionState
    {
        ENone,
        EConnect,
        EDisconnect,
    }
    
    public sealed class SessionStateComponent : Entity
    {
        public SceneType SceneType { get; set; }
        public ESessionState SessionState { get; set; } = ESessionState.ENone;
    }
}