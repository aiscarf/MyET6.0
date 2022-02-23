using System.Collections.Generic;

namespace ET
{
    public static class LoginDataSystem
    {
        public static GameService GetServiceByIndex(this LoginDataComponent self, int index)
        {
            if (index < 0 || index >= self.GameServices.Count)
                return null;
            return self.GameServices[index];
        }

        public static GameRegion GetRegionByIndex(this LoginDataComponent self, int index)
        {
            if (index < 0 || index >= self.GameRegions.Count)
                return null;
            return self.GameRegions[index];
        }

        public static List<GameRegion> GetRegionByServiceId(this LoginDataComponent self, int serviceId)
        {
            List<GameRegion> list = new List<GameRegion>();
            for (int i = 0; i < self.GameRegions.Count; i++)
            {
                if (self.GameRegions[i].RegionId / 100 == serviceId)
                {
                    list.Add(self.GameRegions[i]);
                }
            }

            return list;
        }
    }
}