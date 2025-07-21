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
        /// 读卡器设备
        /// </summary>
        private IdCardDev idCardDev;

        /// <summary>
        /// 头像图片临时路径
        /// </summary>
        private byte[] headphotoPathTmp;

        /// <summary>
        /// 是否正在读卡
        /// </summary>
        private bool readIdCard = false;

        public ReadRepository(IConfiguration configuration, ILogger<ReadRepository> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// 获取身份证信息
        /// </summary>
        /// <param name="timeOut">超时时间</param>
        /// <returns></returns>
        public ReadInfo GetReadInfo(int timeOut)
        {
            readIdCard = true;
            idCardDev = new IdCardDev();
            configuration.GetSection(IdCardDev.Position).Bind(idCardDev);
            logger.LogDebug("输入参数为：Port：{0},Extport：{1},Baud：{2}", idCardDev.Port, idCardDev.Extport, idCardDev.Baud);
            int ret =  GHCIdCardSDK.GHC_Dev_Connect(idCardDev.Port, Encoding.GetEncoding("GB2312").GetBytes(idCardDev.Extport)[0], idCardDev.Baud);
            if (ret == 0)
            {
                //ret = GHCIdCardSDK.GHC_IDCard_ReadCard_Fp(timeOut * 1000);

                ret = GHCIdCardSDK.GHC_IDCard_ReadCard(timeOut * 1000);
                if(!readIdCard)
                {
                    GHCIdCardSDK.GHC_Dev_Disconnect();
                    logger.LogWarning("检测到了取消获取身份证信息接口");
                    throw new ApplicationException("获取身份证信息已取消");
                }
                if (ret == 0)
                {
                    ReadInfo readInfo = new ReadInfo();
                    byte[] wzInfo = new byte[256];

                    GHCIdCardSDK.GHC_IDCard_GetCardInfo(0, wzInfo);
                    // 0x49=I,代表外国人
                    if (wzInfo[0] == 0x49)
                    {
                        readInfo.Name = GetStrByIndex(5);
                        readInfo.Sex = GetStrByIndex(2);
                        readInfo.Nation = GetStrByIndex(4);
                        readInfo.Birthday = GetStrByIndex(8);
                        readInfo.Addr = "";
                        readInfo.Idcode = GetStrByIndex(3);
                        readInfo.Department = GetStrByIndex(10);
                        readInfo.StartDate = GetStrByIndex(6);
                        readInfo.EndDate = GetStrByIndex(7);
                        readInfo.AppendMsg = "";
                    }
                    // 0x4A=J,代表港澳台同胞
                    else if (wzInfo[0] == 0x4A)
                    {
                        readInfo.Name = GetStrByIndex(1);
                        readInfo.Sex = GetStrByIndex(2);
                        readInfo.Nation = "";
                        readInfo.Birthday = GetStrByIndex(3);
                        readInfo.Addr = GetStrByIndex(4);
                        readInfo.Idcode = GetStrByIndex(5);
                        readInfo.Department = GetStrByIndex(6);
                        readInfo.StartDate = GetStrByIndex(7);
                        readInfo.EndDate = GetStrByIndex(8);
                        readInfo.AppendMsg = GetStrByIndex(9);
                    }
                    // 中国人
                    else
                    {
                        readInfo.Name = GetStrByIndex(1);
                        readInfo.Sex = GetStrByIndex(2);
                        readInfo.Nation = GetStrByIndex(3);
                        readInfo.Birthday = GetStrByIndex(4);
                        readInfo.Addr = GetStrByIndex(5);
                        readInfo.Idcode = GetStrByIndex(6);
                        readInfo.Department = GetStrByIndex(7);
                        readInfo.StartDate = GetStrByIndex(8);
                        readInfo.EndDate = GetStrByIndex(9);
                    }

                    //// 加载指纹
                    //byte[] fpInfo = new byte[4096];
                    //int fpInfoLen = 0;
                    //GHCIdCardSDK.GHC_IDCard_FpInfo(fpInfo, ref fpInfoLen);
                    //byte[] fpInfoBy = new byte[fpInfoLen];
                    //Array.Copy(fpInfo, 0, fpInfoBy, 0, fpInfoLen);
                    //readInfo.FprImageStr = Convert.ToBase64String(fpInfo);
                    readInfo.FprImageStr = "";

                    // 加载头像照片
                    if (headphotoPathTmp == null)
                    {
                        headphotoPathTmp = new byte[256];
                        headphotoPathTmp = Encoding.GetEncoding("GB2312").GetBytes(Path.GetTempFileName());
                    }
                    GHCIdCardSDK.GHC_IDCard_CreateHeadPhoto(0, headphotoPathTmp);

                    byte[] base64data = new byte[100 * 1024];
                    int outbase64dataLen = 0;
                    GHCIdCardSDK.GHC_GetFileBase64Buffer(headphotoPathTmp, base64data, 100 * 1024, ref outbase64dataLen);
                    readInfo.ImageStr = System.Text.Encoding.GetEncoding("GB2312").GetString(base64data, 0, outbase64dataLen);

                    GHCIdCardSDK.GHC_Dev_Disconnect();
                    return readInfo;
                }
                else
                {
                    logger.LogWarning("读取身份证失败，错误编码：{0}", ret);
                    GHCIdCardSDK.GHC_Dev_Disconnect();
                    return null;
                    //throw new ApplicationException("在" + timeOut + "秒内，没有读到身份证，请重试！");
                }
            }
            else
            {
                logger.LogWarning("打开读卡器设备错误，错误编码：{0}", ret);
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

        private string GetStrByIndex(int index)
        {
            byte[] wzInfo = new byte[256];
            GHCIdCardSDK.GHC_IDCard_GetCardInfo(index, wzInfo);
            return Encoding.GetEncoding("GB2312").GetString(wzInfo).TrimEnd('\0');
        }
    }
}
