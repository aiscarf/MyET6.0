using System.Collections.Generic;

namespace ET
{
    public static class UIServerListSystem
    {
        public static GameServiceVO GetServiceByIndex(this UIServerListComponent self, int index)
        {
            if (index < 0 || index >= self.ServiceVos.Count)
                return null;
            return self.ServiceVos[index];
        }

        public static GameRegionVO GetRegionByIndex(this UIServerListComponent self, int index)
        {
            if (index < 0 || index >= self.RegionVos.Count)
                return null;
            return self.RegionVos[index];
        }

        public static List<GameRegionVO> GetRegionByServiceId(this UIServerListComponent self, int serviceId)
        {
            List<GameRegionVO> list = new List<GameRegionVO>();
            for (int i = 0; i < self.RegionVos.Count; i++)
            {
                if (self.RegionVos[i].RegionId / 100 == serviceId)
                {
                    list.Add(self.RegionVos[i]);
                }
            }

            return list;
        }
    }
}