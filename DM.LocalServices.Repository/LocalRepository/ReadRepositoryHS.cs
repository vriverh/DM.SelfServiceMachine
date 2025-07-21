using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device;
using DM.LocalServices.Device.IdCardReaderHS;
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
    public class ReadRepositoryHS : IReadRepository
    {
        ///// <summary>
        ///// 配置参数
        ///// </summary>
        //private readonly IConfiguration configuration;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ReadRepositoryHS> logger;

        /// <summary>
        /// 是否正在读卡
        /// </summary>
        private bool readIdCard = false;

        /// <summary>
        /// 读卡器端口类型
        /// </summary>
        private IdCardReaderPortTypes idCardReaderPortType;
        /// <summary>
        /// 端口
        /// </summary>
        private int port;
        /// <summary>
        /// 是否初始化成功
        /// </summary>
        private bool initialized;

        public ReadRepositoryHS(ILogger<ReadRepositoryHS> logger)
        {
            //this.configuration = configuration;
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
                for (int i = 1001; i <= 1016; i++)
                {
                    if (IdCardReaderSdk.CVR_InitComm(i) == 1)
                    {
                        idCardReaderPortType = IdCardReaderPortTypes.USB;
                        port = i;
                        initialized = true;
                        return true;
                    }
                }
                for (int i = 1; i <= 4; i++)
                {
                    if (IdCardReaderSdk.CVR_InitComm(i) == 1)
                    {
                        idCardReaderPortType = IdCardReaderPortTypes.SeiralPort;
                        port = i;
                        initialized = true;
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return initialized;
            }
            
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
                try
                {
                    if (IdCardReaderSdk.CVR_Authenticate() != 1)
                        throw new IdCardNotFoundException();
                    if (IdCardReaderSdk.CVR_Read_Content(4) != 1)
                        throw new IdCardReaderReadException();
                    byte[] name = new byte[30];
                    int length = 30;
                    IdCardReaderSdk.GetPeopleName(ref name[0], ref length);
                    byte[] number = new byte[30];
                    length = 36;
                    IdCardReaderSdk.GetPeopleIDCode(ref number[0], ref length);
                    byte[] people = new byte[30];
                    length = 3;
                    IdCardReaderSdk.GetPeopleNation(ref people[0], ref length);
                    byte[] validtermOfStart = new byte[30];
                    length = 16;
                    IdCardReaderSdk.GetStartDate(ref validtermOfStart[0], ref length);
                    byte[] birthday = new byte[30];
                    length = 16;
                    IdCardReaderSdk.GetPeopleBirthday(ref birthday[0], ref length);
                    byte[] address = new byte[30];
                    length = 70;
                    IdCardReaderSdk.GetPeopleAddress(ref address[0], ref length);
                    byte[] validtermOfEnd = new byte[30];
                    length = 16;
                    IdCardReaderSdk.GetEndDate(ref validtermOfEnd[0], ref length);
                    byte[] department = new byte[30];
                    length = 30;
                    IdCardReaderSdk.GetDepartment(ref department[0], ref length);
                    byte[] sex = new byte[30];
                    length = 3;
                    IdCardReaderSdk.GetPeopleSex(ref sex[0], ref length);
                    byte[] samid = new byte[32];
                    IdCardReaderSdk.CVR_GetSAMID(ref samid[0]);
                    var encoding = Encoding.GetEncoding("GB2312");
                    var info = new ReadInfo()
                    {
                        Name = GetString(name),
                        Addr = GetString(address),
                        Birthday = GetString(birthday),
                        Department = GetString(department),
                        Sex = GetString(sex),
                        Idcode = GetString(number)?.ToUpper(),
                        Nation = GetString(people),
                        StartDate = GetString(validtermOfStart),
                        EndDate = GetString(validtermOfEnd)
                    };
                    return info;
                }
                catch { }
                finally
                {
                    await Task.Delay(500);
                }
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
