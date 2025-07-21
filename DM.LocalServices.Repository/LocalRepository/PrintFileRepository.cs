using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace DM.LocalServices.Repository.LocalRepository
{
    public class PrintFileRepository : IPrintFileRepository
    {
        private readonly ILogger<PrintFileRepository> logger;
        private readonly IPrinterService printerService;

        public PrintFileRepository(ILogger<PrintFileRepository> logger, IPrinterService printerService)
        {
            this.logger = logger;
            this.printerService = printerService;
        }

        public void Print(PrintFileInfo printFileInfo)
        {
            try
            {
                logger.LogInformation("开始打印文件");

                if (string.IsNullOrEmpty(printFileInfo.FileContent))
                {
                    logger.LogWarning("打印内容为空");
                    return;
                }

                byte[] printData;
                if (!string.IsNullOrEmpty(printFileInfo.FileContent))
                {
                    // 如果是Base64编码的内容
                    try
                    {
                        printData = Convert.FromBase64String(printFileInfo.FileContent);
                    }
                    catch
                    {
                        // 如果不是Base64，当作普通文本处理
                        printData = Encoding.UTF8.GetBytes(printFileInfo.FileContent);
                    }
                }
                else
                {
                    logger.LogWarning("打印文件路径和内容均为空");
                    return;
                }

                printerService.SendBytesToPrinter("默认打印机", printData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "打印文件时发生错误");
            }
        }

        public PrinterStatus GetPrinterStatus()
        {
            try
            {
                logger.LogInformation("获取打印机状态");
                // 简化实现，实际应该检查打印机状态
                return new PrinterStatus
                {
                    Code = 0, // 0表示正常
                    Desc = "打印机正常"
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取打印机状态时发生错误");
                return new PrinterStatus
                {
                    Code = -1,
                    Desc = "获取打印机状态失败：" + ex.Message
                };
            }
        }
    }
}