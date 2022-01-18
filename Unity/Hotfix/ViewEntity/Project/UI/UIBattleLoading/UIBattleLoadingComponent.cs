using UnityEngine.UI;

namespace ET
{
    [UITag(UIType.UIBattleLoading)]
    public class UIBattleLoadingComponent : Entity
    {
        public Slider m_sliderProgress;
        public long m_testTimer;
    }
}