namespace ET
{
	public partial class UILoginSceneComponent
	{
		public DataProxy<bool> IsLoginRealmProxy = new DataProxy<bool>(false);
		public DataProxy<GameRegionVO> CurSelectRegionVoProxy = new DataProxy<GameRegionVO>(null);
	}
}
