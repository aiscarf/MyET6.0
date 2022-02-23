using System.Collections.Generic;

namespace ET
{
	public partial class UISelectMapComponent
	{
		public List<DungeonVO> DungeonVos = new List<DungeonVO>();
	}
	
	public class DungeonVO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ImageName { get; set; }
	}
}
