using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DM.LocalServices.Repository.LocalRepository
{

    public class DevInfoRepository : IDevInfoRepository
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<DevInfoRepository> logger;
        /// <summary>
        /// 配置参数
        /// 
        /// </summary>
        private readonly IConfiguration configuration;
        /// <summary>
        /// 机器信息服务
        /// </summary>
        private readonly IMachineInfoService machineInfoService;

        /// <summary>
        /// Represents the device information associated with the current context.
        /// </summary>
        /// <remarks>This field stores metadata or configuration details about the device.  It is intended
        /// for internal use and should not be accessed directly by external code.</remarks>
        private DevInfo? devInfo;

        public DevInfoRepository(ILogger<DevInfoRepository> logger, IConfiguration configuration, IMachineInfoService machineInfoService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.machineInfoService = machineInfoService;
        }


        public DevInfo GetDevInfo()
        {
            logger.LogDebug("开始请求获取设备信息");
            if (devInfo == null)
            {
                devInfo = machineInfoService.GetDeviceInfo();
            }
            logger.LogDebug("设备信息详情：{DevInfo}", devInfo);
            return devInfo;
        }
    }
}
