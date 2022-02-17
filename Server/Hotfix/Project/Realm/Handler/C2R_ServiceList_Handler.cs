using System;
using System.Collections.Generic;

namespace ET
{
    public class C2R_ServiceList_Handler: AMRpcHandler<C2R_ServiceList, R2C_ServiceList>
    {
        protected override async ETTask Run(Session session, C2R_ServiceList request, R2C_ServiceList response, Action reply)
        {
            try
            {
                response.ServiceList = ServiceList;
                response.RegionList = RegionList;
                reply();
            }
            catch (Exception e)
            {
                ReplyError(response, e, reply);
            }

            await ETTask.CompletedTask;
        }

        #region 测试数据

        private static List<GameService> ServiceList = new List<GameService>()
        {
            new GameService() { ServiceId = 9001, ServiceName = "测试服1", ServiceStartTime = 18305918305918, },
            new GameService() { ServiceId = 9002, ServiceName = "测试服2", ServiceStartTime = 18305918305918, },
            new GameService() { ServiceId = 9003, ServiceName = "测试服3", ServiceStartTime = 18305918305918, },
            new GameService() { ServiceId = 1001, ServiceName = "正式服1", ServiceStartTime = 18305918305918, },
            new GameService() { ServiceId = 1002, ServiceName = "正式服2", ServiceStartTime = 18305918305918, },
            new GameService() { ServiceId = 1003, ServiceName = "正式服3", ServiceStartTime = 18305918305918, },
        };

        private static List<GameRegion> RegionList = new List<GameRegion>()
        {
            new GameRegion()
            {
                RegionId = 900101, RegionName = "测试区1-1", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 900102, RegionName = "测试区1-2", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 900103, RegionName = "测试区1-3", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 900201, RegionName = "测试区2-1", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 900301, RegionName = "测试区3-1", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 100101, RegionName = "正式区1-1", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 100102, RegionName = "正式区1-2", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 100201, RegionName = "正式区2-1", State = 1, Address = "127.0.0.1:30002",
            },
            new GameRegion()
            {
                RegionId = 100301, RegionName = "正式区3-1", State = 1, Address = "127.0.0.1:30002",
            },
        };

        #endregion
    }
}