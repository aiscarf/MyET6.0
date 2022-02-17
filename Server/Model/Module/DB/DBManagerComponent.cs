namespace ET
{
    public class DBManagerComponent: Entity
    {
        public static DBManagerComponent Instance;

        public DBComponent[] DBComponents = new DBComponent[IdGenerater.MaxZone];
    }
}