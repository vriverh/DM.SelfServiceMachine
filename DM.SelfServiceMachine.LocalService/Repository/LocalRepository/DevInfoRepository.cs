using DM.SelfServiceMachine.LocalService.Models;
using DM.SelfServiceMachine.LocalService.Repository.IRepository;
using DM.SelfServiceMachine.LocalService.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Repository.LocalRepository
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
        /// Represents the device information associated with the current context.
        /// </summary>
        /// <remarks>This field stores metadata or configuration details about the device.  It is intended
        /// for internal use and should not be accessed directly by external code.</remarks>
        private DevInfo devInfo;

        public DevInfoRepository(ILogger<DevInfoRepository> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }


        public DevInfo GetDevInfo()
        {
            logger.LogDebug("开始请求获取设备信息");
            if (devInfo == null)
            {
                devInfo = new DevInfo();

                devInfo.Mac = MachineInfo.Mac;
                devInfo.Ip = MachineInfo.Ip;

                devInfo.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();

                devInfo.Code = configuration.GetValue<string>("RegistrationOfficeCode");

                devInfo.DeviceName = configuration.GetValue<string>("DeviceName");

                devInfo.QueueCenterURL = configuration.GetValue<string>("QueueCenterURL");

                devInfo.QueueWebURL = configuration.GetValue<string>("QueueWebURL");
            }
            logger.LogDebug("设备信息详情：" + devInfo.ToString());
            return devInfo;
        }
    }
}
