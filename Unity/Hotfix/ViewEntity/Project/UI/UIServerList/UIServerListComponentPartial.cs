using System.Collections.Generic;

namespace ET
{
	public partial class UIServerListComponent
	{
		public int m_nSelectServiceIndex;
		public GameRegionVO CurSelectRegion = null;
		public List<GameServiceVO> ServiceVos = new List<GameServiceVO>();
		public List<GameRegionVO> RegionVos = new List<GameRegionVO>();
	}
}
