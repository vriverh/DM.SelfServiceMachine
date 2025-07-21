using brQueryPrinterLib;
using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Graphics;
using Spire.Pdf.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DM.LocalServices.Repository.LocalRepository
{
    public class BrPrintFileRepository : IPrintFileRepository
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<BrPrintFileRepository> logger;
        private readonly QueryPrinterLib query;

        public BrPrintFileRepository(ILogger<BrPrintFileRepository> logger, IConfiguration configuration)
        {
            this.logger = logger;
            string printerName = configuration.GetValue<string>("printerName");

            this.query = new QueryPrinterLib(printerName);
        }

        public void Print(PrintFileInfo printFileInfo)
        {
            logger.LogDebug("开始准备打印，文件信息:{0}", printFileInfo.ToString());
            byte[] fileBytes;
            if (printFileInfo.TranType == 1)
            {
                fileBytes = Convert.FromBase64String(printFileInfo.FileContent);
            }
            else
            {
                fileBytes = File.ReadAllBytes(printFileInfo.FilePath);
            }

            logger.LogDebug("开始打印");
            RawPrinterHelper.SendFileToPrinter(fileBytes);
            logger.LogDebug("打印成功");

        }

        public PrinterStatus GetPrinterStatus()
        {
            PrinterStatus printerStatus = new PrinterStatus();

            string jobStatus = "";
            string jobName = "";
            string jobPages = "";
            int ret = query.getJobStatus(ref jobStatus, ref jobName, ref jobPages);
            if (ret == 0)//开始
            {
                Console.WriteLine(jobStatus);
                Console.WriteLine(jobName);
                Console.WriteLine(jobPages);
            }
            else if (ret == 1 || ret == 2)//结束或其他状态
            {
                Console.WriteLine(jobStatus);
                Console.WriteLine(jobName);
                Console.WriteLine(jobPages);
            }
            else if (ret == -1 || ret == 3)//打开打印机失败 或 没有打印任务 
            {
                if (int.TryParse(query.getPrinterStatus(), out int code))
                {
                    printerStatus.Code = code;
                    printerStatus.Desc = GetDescription(code);
                    if(code== ((int)PrinterStatusEnum.Ready))
                    {
                        printerStatus.Code = 0;
                    }
                }
            }


            return printerStatus;
        }


        private static string GetDescription(int code)
        {
            if (Enum.IsDefined(typeof(PrinterStatusEnum), code))
            {
                PrinterStatusEnum enumValue = (PrinterStatusEnum)code;
                string value = enumValue.ToString();
                System.Reflection.FieldInfo field = enumValue.GetType().GetField(value);
                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
                if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                    return value;
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
                return descriptionAttribute.Description;
            }
            else
            {
                return "未知的错误编码。";
            }
        }

        private enum PrinterStatusEnum
        {
            [Description("准备好了")]
            Ready = 10001,
            [Description("暂停")]
            Pause = 10002,
            [Description("请稍等")]
            PleaseWait = 10003,
            [Description("碳粉不足")]
            TonerLow = 10006,
            [Description("工作取消")]
            JobCancel = 10007,
            [Description("打印中")]
            Printing = 10023,
            [Description("内存不足")]
            OutofMemory = 30016,
            [Description("打印机休眠")]
            Sleep = 40000,
            [Description("无碳粉")]
            NoToner = 40010,
            [Description("打印机机盖是打开的")]
            CoverisOpen = 40021,
            [Description("更换墨粉盒")]
            ReplaceToner = 40038,
            [Description("没有纸张或纸张大小匹配")]
            NoPaperMPSizeMismatchMP = 41000,
            [Description("手动送纸")]
            ManualFeed = 41100,
            [Description("T1没有纸张或纸张大小匹配或无托盘")]
            NoPaperT1 = 41200,
            [Description("T2没有纸张或纸张大小匹配或无托盘")]
            NoPaperT2 = 41300,
            [Description("打印机机盖是打开的")]
            CoverisOpen2 = 42104,
            [Description("定影器错误")]
            FuserError = 50076,
            [Description("内存不足或存储已满")]
            OutofMemoryOrStorageFull = 60000,
            [Description("降温")]
            CoolingDown = 60003,
            [Description("硒鼓错误")]
            DrumError = 60005,
            [Description("打印机机盖是打开的")]
            CoverisOpen3 = 60021,
            [Description("DX尺寸错误")]
            SizeErrorDX = 60023,
            [Description("错误编码60030")]
            PrintUnable = 60030,
            [Description("已禁用双工")]
            DuplexDisabled = 60120,
            [Description("T1尺寸错误")]
            SizeErrorT1 = 60131,
            [Description("大小不匹配")]
            Sizemismatch = 60142,
            [Description("墨盒错误")]
            CartridgeError = 60144,
            [Description("T2尺寸错误")]
            SizeErrorT2 = 60132,
            [Description("没有纸")]
            NoPaper = 60148,
            [Description("不可用的设备")]
            UnusableDevice = 60152,
            [Description("硒鼓停止")]
            DrumStop = 60156,
            [Description("记录访问错误")]
            LogAccessError1 = 60164,
            [Description("记录访问错误")]
            LogAccessError2 = 60165,
            [Description("记录访问错误")]
            LogAccessError3 = 60166,
            [Description("记录访问错误")]
            LogAccessError4 = 60167,
            [Description("更换激光器")]
            ReplaceLaser = 62002,
            [Description("硒鼓即将用完")]
            DrumEndSoon = 62003,
            [Description("更换定影器")]
            ReplaceFuser = 62104,
            [Description("替换PF Kit1")]
            ReplacePFKit1 = 62109,
            [Description("替换PF Kit2")]
            ReplacePFKit2 = 62110,
            [Description("替换PF KitMP")]
            ReplacePFKitMP = 62116,
            [Description("更换硒鼓")]
            ReplaceDrum = 62119,
            [Description("1号托盘纸量底")]
            PaperLowTray1 = 62122,
            [Description("2号托盘纸量底")]
            PaperLowTray2 = 62123,
            [Description("3号托盘纸量底")]
            PaperLowTray3 = 62124,
            [Description("4号托盘纸量底")]
            PaperLowTray4 = 62125,
            [Description("5号托盘纸量底")]
            PaperLowTray5 = 62126,
            [Description("MP托盘卡纸")]
            JamMPTray = 65001,
            JamTray1 = 65002,
            JamTray2 = 65004,
            JamInside = 65016,
            JamRear = 65032,
            JamDuplex = 65064,
            [Description("日志已满")]
            JournalFull = 70001,
            [Description("文档卡纸")]
            DocumentJam = 70004,
            [Description("错误的纸张尺寸")]
            WrongPaperSize = 70007,
            [Description("打印机机盖是打开的")]
            CoverisOpen4 = 70014,
            [Description("扫描不能自动对焦")]
            ScanUnableAF = 70100,
            [Description("错误编码70400")]
            ScanUnableA6 = 70400,
            [Description("错误编码70420")]
            ScanUnableBF = 70420,
        }
    }
}
