using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Vanara.PInvoke.WinSpool;

namespace DM.LocalServices.Device
{
    public class RawPrinterHelper
    {
        /// <summary>
        /// 加锁obj
        /// </summary>
        private static readonly object obj = new object();
        /// <summary>
        /// 打印机句柄
        /// </summary>
        private static SafeHPRINTER hPrinter = null;
        /// <summary>
        /// 日志
        /// </summary>
        public static ILogger<RawPrinterHelper> Logger { get; set; }

        private static SafeHPRINTER HPrinter
        {
            get
            {
                lock (obj)
                {
                    if (hPrinter != null)
                    {
                        return hPrinter;
                    }
                    Logger.LogDebug("开始获取默认打印机");
                    string printerName = string.Empty;
                    StringBuilder dp = new StringBuilder(256);
                    int size = dp.Capacity;
                    if (GetDefaultPrinter(dp, ref size))
                    {
                        printerName = dp.ToString().Trim();
                        Logger.LogDebug("默认打印机名称为：{0}", printerName);
                    }
                    if (string.IsNullOrWhiteSpace(printerName))
                    {
                        string strer = "打印机发生了一个错误，没有找到默认打印机。";
                        Logger.LogWarning(strer);
                        throw new ApplicationException(strer);
                    }
                    Logger.LogDebug("开始打开默认打印机");
                    if (OpenPrinter(printerName, out hPrinter))
                    {
                        Logger.LogDebug("打开默认打印机成功");
                        return hPrinter;
                    }
                    else
                    {
                        int dwError = Marshal.GetLastWin32Error();
                        Exception exception = Marshal.GetExceptionForHR(dwError);
                        string strer = "打开打印机发生了一个错误，错误编码：" + dwError + "错误描述：" + exception?.Message;
                        Logger.LogWarning(strer); 
                        throw new ApplicationException("打开打印机发生了一个错误，错误编码：" + dwError + "错误描述：" + exception?.Message, exception);
                    }
                }

            }
        }

        /// <summary>
        /// 打印文件
        /// </summary>
        /// <param name="fileBytes">文件字节数组</param>
        /// <returns>成功时为true，失败时为false</returns>
        public static bool SendFileToPrinter(byte[] fileBytes)
        {
            Logger.LogDebug("开始将文件流保存到为非托管内存中");
            int nLength = Convert.ToInt32(fileBytes.Length);
            IntPtr ptrUnmanagedBytes = new IntPtr(0);
            ptrUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            Marshal.Copy(fileBytes, 0, ptrUnmanagedBytes, nLength);

            Logger.LogDebug("保存成功，开始正式打印");
            bool success = SendBytesToPrinter(ptrUnmanagedBytes, ((uint)nLength));
            Marshal.FreeCoTaskMem(ptrUnmanagedBytes);
            Logger.LogDebug("打印成功，释放非托管内存");
            return success;
        }

        /// <summary>
        /// 获取打印机状态
        /// </summary>
        /// <returns>状态编码，描述</returns>
        public static Tuple<int, string> GetPrinterStatus()
        {
            PRINTER_STATUS pStatus = GetPrinterStatusInt();
            string strStatus = string.Empty;
            switch (pStatus)
            {
                case 0:
                    strStatus = "准备就绪（Ready）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_BUSY:
                    strStatus = "忙(Busy）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_DOOR_OPEN:
                    strStatus = "门被打开（Printer Door Open）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_ERROR:
                    strStatus = "错误(Printer Error）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_INITIALIZING:
                    strStatus = "正在初始化(Initializing）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_IO_ACTIVE:
                    strStatus = "正在输入或输出（I/O Active）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_MANUAL_FEED:
                    strStatus = "手工送纸（Manual Feed）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_NOT_AVAILABLE:
                    strStatus = "不可用（Not Available）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_NO_TONER:
                    strStatus = "无墨粉（No Toner）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_OFFLINE:
                    strStatus = "脱机（Off Line）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_OUTPUT_BIN_FULL:
                    strStatus = "输出口已满（Output Bin Full）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_OUT_OF_MEMORY:
                    strStatus = "内存溢出（Out of Memory）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PAGE_PUNT:
                    strStatus = "当前页无法打印（Page Punt）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PAPER_JAM:
                    strStatus = "塞纸（Paper Jam）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PAPER_OUT:
                    strStatus = "打印纸用完（Paper Out）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PAPER_PROBLEM:
                    strStatus = "纸张问题（Page Problem）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PAUSED:
                    strStatus = "暂停（Paused）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PENDING_DELETION:
                    strStatus = "正在删除（Pending Deletion）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_POWER_SAVE:
                    strStatus = "打印机处于省电模式。（Power Save）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PRINTING:
                    strStatus = "正在打印（Printing）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_PROCESSING:
                    strStatus = "正在处理（Processing）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_SERVER_OFFLINE:
                    strStatus = "打印机处于脱机状态。（Server OFFLINE）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_SERVER_UNKNOWN:
                    strStatus = "打印机状态未知（Server Unknown）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_TONER_LOW:
                    strStatus = "墨粉不足（Toner Low）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_USER_INTERVENTION:
                    strStatus = "需要用户干预（User Intervention）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_WAITING:
                    strStatus = "等待（Warming）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_WARMING_UP:
                    strStatus = "正在准备（Warming Up）";
                    break;
                case PRINTER_STATUS.PRINTER_STATUS_DRIVER_UPDATE_NEEDED:
                    strStatus = "打印机驱动程序需要更新。（Driver Update Needed）";
                    break;
                default:
                    strStatus = "未知状态（Unknown Status）";
                    break;
            }
            return new Tuple<int, string>(((int)pStatus), strStatus);
        }

        /// <summary>
        /// 此函数获取打印机名称和非托管字节数组，该函数将这些字节发送到打印队列。
        /// </summary>
        /// <param name="pBytes">文件字节数组托管内存地址</param>
        /// <param name="dwCount">字节数量</param>
        /// <returns></returns>
        private static bool SendBytesToPrinter(IntPtr pBytes, uint dwCount)
        {
            uint dwWritten = 0;
            bool success = false;

            DOC_INFO_1 di = new DOC_INFO_1();
            di.pDocName = "DM.SelfServiceMachine Print Document";
            di.pDatatype = "RAW";

            Logger.LogDebug("开始设置文档打印");
            if (StartDocPrinter(HPrinter, 1, di) != 0)
            {
                Logger.LogDebug("开始进行文档内容页面打印");
                if (StartPagePrinter(HPrinter))
                {
                    Logger.LogDebug("开始写入文档内容字节流");
                    success = WritePrinter(HPrinter, pBytes, dwCount, out dwWritten);
                    EndPagePrinter(HPrinter);
                    Logger.LogDebug("写入成功，关闭文档内容页面打印");
                }
                EndDocPrinter(HPrinter);
                Logger.LogDebug("关闭文档打印");
            }

            if (success == false)
            {
                int dwError = Marshal.GetLastWin32Error();
                Exception exception = Marshal.GetExceptionForHR(dwError);
                string strer = "打印机发生了一个错误，错误编码：" + dwError + "错误描述：" + exception?.Message;
                Logger.LogWarning(strer);
                throw new ApplicationException(strer, exception);
            }
            return success;
        }

        /// <summary>
        /// 获取打印机状态
        /// </summary>
        /// <returns>打印机状态</returns>
        private static PRINTER_STATUS GetPrinterStatusInt()
        {
            Logger.LogDebug("开始获取打印机状态");
            PRINTER_STATUS pStatus = PRINTER_STATUS.PRINTER_STATUS_SERVER_UNKNOWN;
            GetPrinter(HPrinter, 2, IntPtr.Zero, 0, out uint cbNeeded);
            if (cbNeeded > 0)
            {
                IntPtr pAddr = Marshal.AllocHGlobal((int)cbNeeded);
                if (GetPrinter(HPrinter, 2, pAddr, cbNeeded, out cbNeeded))
                {
                    PRINTER_INFO_2 Info2 = new PRINTER_INFO_2();

                    Info2 = (PRINTER_INFO_2)Marshal.PtrToStructure(pAddr, typeof(PRINTER_INFO_2));

                    pStatus = Info2.Status;
                }
                Marshal.FreeHGlobal(pAddr);
            }
            Logger.LogDebug("结束获取打印机状态");
            return pStatus;
        }
    }
}
