namespace ET
{
    public sealed class SessionStateComponentAwakeSystem : AwakeSystem<SessionStateComponent, SceneType>
    {
        public override void Awake(SessionStateComponent self, SceneType sceneType)
        {
            self.SceneType = sceneType;
            
            // DONE: 默认连接上.
            self.SessionState = ESessionState.EConnect;
        }
    }

    public sealed class SessionStateComponentDestroySystem : DestroySystem<SessionStateComponent>
    {
        public override void Destroy(SessionStateComponent self)
        {
            self.SessionState = ESessionState.EDisconnect;

            // DONE: 发送Session断开连接的消息.
            Game.EventSystem.Publish(new EventType.SessionDisconnect() { SceneType = self.SceneType, Session = self.GetParent<Session>() });
        }
    }

    public static class SessionStateComponentSystem
    {
    }
}