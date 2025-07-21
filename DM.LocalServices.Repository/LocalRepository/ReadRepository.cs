using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.LocalServices.Repository.LocalRepository
{
    public class ReadRepository : IReadRepository
    {
        /// <summary>
        /// 配置参数
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ReadRepository> logger;

        /// <summary>
        /// ID卡读取服务
        /// </summary>
        private readonly IIdCardReaderService idCardReaderService;

        /// <summary>
        /// 是否正在读卡
        /// </summary>
        private bool readIdCard = false;

        public ReadRepository(IConfiguration configuration, ILogger<ReadRepository> logger, IIdCardReaderService idCardReaderService)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.idCardReaderService = idCardReaderService;
        }

        /// <summary>
        /// 获取身份证信息
        /// </summary>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public ReadInfo GetReadInfo(int timeOut)
        {
            try
            {
                readIdCard = true;
                logger.LogDebug("开始读取身份证信息，超时时间：{TimeOut}秒", timeOut);
                
                if (!idCardReaderService.Initialize())
                {
                    throw new ApplicationException("初始化读卡器失败");
                }

                var readInfo = idCardReaderService.ReadCard();
                if (!readIdCard)
                {
                    logger.LogWarning("检测到了取消获取身份证信息接口");
                    throw new ApplicationException("获取身份证信息已取消");
                }

                return readInfo;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "读取身份证信息时发生错误");
                throw;
            }
            finally
            {
                idCardReaderService.Close();
                readIdCard = false;
            }
        }

        /// <summary>
        /// 取消获取身份证信息
        /// </summary>
        public void CancelGetReadInfo()
        {
            readIdCard = false;
        }
    }
}
