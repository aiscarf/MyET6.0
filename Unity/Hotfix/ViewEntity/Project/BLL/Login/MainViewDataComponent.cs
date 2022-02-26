namespace ET
{
    public class MainViewDataComponent : Entity
    {
        public readonly DataProxy<DungeonVO> CurSelectDungeonProxy = new DataProxy<DungeonVO>(null);

        public int CurSelectHeroId;
    }
}