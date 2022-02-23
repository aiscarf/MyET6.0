namespace ET
{
    public static class DataHelper
    {
        public static T GetDataComponentFromCurScene<T>() where T : Entity
        {
            return ZoneSceneManagerComponent.Instance.CurScene?.GetComponent<T>();
        }
    }
}