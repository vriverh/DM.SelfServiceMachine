using DM.SelfServiceMachine.LocalService.Models;
using DM.SelfServiceMachine.LocalService.Repository.IRepository;
using DM.SelfServiceMachine.LocalService.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Repository.VirtualRepository
{
    public class VirtualReadRepository : IReadRepository
    {
        /// <summary>
        /// 配置参数
        /// </summary>
        private readonly IConfiguration configuration;
        public VirtualReadRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 获取身份证信息
        /// </summary>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public ReadInfo GetReadInfo(int timeOut)
        {
            //return null;
            ReadInfo readInfo = new ReadInfo();
            configuration.GetSection("ReadInfo").Bind(readInfo);
            string image = readInfo.ImageStr;
            if (File.Exists(image))
            {
                byte[] imageByte = File.ReadAllBytes(image);
                readInfo.ImageStr = Convert.ToBase64String(imageByte);
            }
            Task.Delay(1000 * 3).Wait();
            return readInfo;
        }


        /// <summary>
        /// 取消获取身份证信息
        /// </summary>
        public void CancelGetReadInfo()
        {

        }
    }
}
