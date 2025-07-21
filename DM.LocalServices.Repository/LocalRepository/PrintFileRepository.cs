using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Logging;
using Spire.Pdf;
using Spire.Pdf.Widget;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Vanara.PInvoke.Gdi32;
using static Vanara.PInvoke.WinSpool;

namespace DM.LocalServices.Repository.LocalRepository
{
    public class PrintFileRepository : IPrintFileRepository
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<PrintFileRepository> logger;

        public PrintFileRepository(ILogger<PrintFileRepository> logger)
        {
            this.logger = logger;
        }

        public void Print(PrintFileInfo printFileInfo)
        {
            logger.LogDebug("开始准备打印，文件信息:{0}", printFileInfo.ToString());

            //byte[] fileBytes;
            //if (printFileInfo.TranType == 1)
            //{
            //    fileBytes = Convert.FromBase64String(printFileInfo.FileContent);
            //}
            //else
            //{
            //    fileBytes = File.ReadAllBytes(printFileInfo.FilePath);
            //}
            //logger.LogDebug("开始打印");
            //RawPrinterHelper.SendFileToPrinter(fileBytes);
            //logger.LogDebug("打印成功");

            string fileName = "";
            if (printFileInfo.TranType == 1)
            {
                byte[] fileBytes = Convert.FromBase64String(printFileInfo.FileContent);
                fileName = Path.GetTempFileName();
                File.WriteAllBytes(fileName, fileBytes);

            }
            else
            {
                fileName = printFileInfo.FilePath;
            }
            logger.LogDebug("开始打印");

            using (var document = PdfiumViewer.PdfDocument.Load(fileName))
            {
                using (var printDocument = document.CreatePrintDocument())
                {
                    //printDocument.PrinterSettings.PrinterName = "Your Printer Name"; // 设置打印机名称
                    printDocument.Print();
                }
            }
            logger.LogDebug("打印成功");



            //if (printFileInfo.FileExtend.Trim().ToLower() == "pdfd")
            //{
            //    try
            //    {
            //        fileBytes = PdfTool.RemoveSignature(fileBytes);
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError(ex, "去除印章发生了一个错误，错误详情：{0}", ex.Message);
            //    }
            //}
        }

        public PrinterStatus GetPrinterStatus()
        {
            logger.LogDebug("开始获取打印信息");
            Tuple<int, string> tuple = RawPrinterHelper.GetPrinterStatus();
            logger.LogDebug("打印信息：Code={0}，desc={1}", tuple.Item1, tuple.Item2);
            return new PrinterStatus() { Code = tuple.Item1, Desc = tuple.Item2 };
        }
    }
}
