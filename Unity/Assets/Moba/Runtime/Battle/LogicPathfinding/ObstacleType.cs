namespace Scarf.Moba
{
    public enum ObstacleType : byte
    {
        StaticObstacle = 1, // 静态障碍物
        DynamicObstacleUnit = 2, // 人物障碍物
        DynamicObstacleSide = 4, // 障碍物栏
        All = 255, // 0xFF        
    }
}