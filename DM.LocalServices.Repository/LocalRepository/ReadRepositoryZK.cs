using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using DM.LocalServices.Device.Abstractions.IdCardReaderHS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DM.LocalServices.Repository.LocalRepository
{
    public class ReadRepositoryZK : IReadRepository
    {

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ReadRepositoryHS> logger;

        /// <summary>
        /// 是否正在读卡
        /// </summary>
        private bool readIdCard = false;

        /// <summary>
        /// 端口
        /// </summary>
        private int port;
        /// <summary>
        /// 是否初始化成功
        /// </summary>
        private bool initialized;

        public const int cbDataSize = 128;

        public ReadRepositoryZK(ILogger<ReadRepositoryHS> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 获取身份证信息
        /// </summary>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public ReadInfo GetReadInfo(int timeOut)
        {
            if (InitReader())
            {
                readIdCard = true;
                return GetReadInfoAsync(timeOut).Result;
            }
            else
            {
                logger.LogWarning("打开读卡器设备错误");
                GHCIdCardSDK.GHC_Dev_Disconnect();
                throw new ApplicationException("打开读卡器设备错误，请确定设备是否已经连接，或参数是否正确。");
            }

        }


        /// <summary>
        /// 取消获取身份证信息
        /// </summary>
        public void CancelGetReadInfo()
        {
            readIdCard = false;
        }

        /// <summary>
        /// 初始化读卡器
        /// </summary>
        private bool InitReader()
        {
            if (!initialized)
            {
                int AutoSearchReader = ZKTecoId200SDK.InitCommExt();
                if (AutoSearchReader > 0)
                {
                    port = AutoSearchReader;
                    initialized = true;
                    return initialized;
                }
                else
                {
                    logger.LogWarning("没有找到中控读卡器设备，请检查设备是否正常链接，驱动是否正常安装");
                    throw new ApplicationException("没有找到中控读卡器设备，请检查设备是否正常链接，驱动是否正常安装");
                }
            }
            return initialized;

        }

        private async Task<ReadInfo> GetReadInfoAsync(int timeOut)
        {
            DateTime outDateTime= DateTime.Now.AddSeconds(timeOut);
            while (DateTime.Now < outDateTime)
            {
                if(!readIdCard)
                {
                    logger.LogWarning("检测到了取消获取身份证信息接口");
                    throw new ApplicationException("获取身份证信息已取消");
                }

                await Task.Delay(500);
                try
                {
                    //卡认证
                    int FindCard = ZKTecoId200SDK.Authenticate();
                    if (FindCard != 1)
                    {
                        continue;
                    }
                    //读卡
                    int rs = ZKTecoId200SDK.Read_Content(1);
                    if (rs != 1 && rs != 2 && rs != 3)
                    {
                        continue;
                    }
                    //读卡成功
                    ReadInfo info= new ReadInfo();

                    //姓名
                    StringBuilder sb = new StringBuilder(cbDataSize);
                    ZKTecoId200SDK.getName(sb, cbDataSize);
                    info.Name = sb.ToString();
                    //地址 
                    ZKTecoId200SDK.getAddress(sb, cbDataSize);
                    string ad = sb.ToString();
                    info.Addr = ad;
                    //出生 
                    ZKTecoId200SDK.getBirthdate(sb, cbDataSize);
                    info.Birthday = sb.ToString();
                    //机关 
                    ZKTecoId200SDK.getIssue(sb, cbDataSize);
                    info.Department = sb.ToString();
                    //性别 
                    ZKTecoId200SDK.getSex(sb, cbDataSize);
                    info.Sex = sb.ToString();
                    //号码 
                    ZKTecoId200SDK.getIDNum(sb, cbDataSize);
                    info.Idcode = sb.ToString();
                    //民族/国家
                    ZKTecoId200SDK.getNation(sb, cbDataSize);
                    info.Nation = sb.ToString();
                    //有效期 
                    ZKTecoId200SDK.getEffectedDate(sb, cbDataSize);
                    info.StartDate = sb.ToString();
                    ZKTecoId200SDK.getExpiredDate(sb, cbDataSize);
                    info.EndDate = sb.ToString();

                    return info;
                }
                catch { }
            }
            logger.LogWarning("读取身份证失败，已超时");
            throw new ApplicationException("在" + timeOut + "秒内，没有读到身份证，请重试！");
        }

        private string GetString(byte[] buffer)
        {
            return Encoding.GetEncoding("GB2312").GetString(buffer).Replace("\0", "").Trim();
        }
    }
}
