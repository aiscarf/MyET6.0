using UnityEngine.UI;

namespace ET
{
    public partial class UIMainComponent
    {
        public Image EUI_Image_Frame { get; set; }

        public string PlayerName { get; set; }
        public int CupNum { get; set; }
        public readonly DataProxy<DungeonVO> CurDungeonProxy = new DataProxy<DungeonVO>(null);

        public int CurSelectHeroId { get; set; }
    }

    public class HeroVo
    {
        public int Id { get; set; }

        public string ModelRes { get; set; }
    }
}