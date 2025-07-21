using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using Microsoft.Extensions.Logging;
using System;

namespace DM.LocalServices.Repository.VirtualRepository
{
    public class VirtualPrintFileRepository : IPrintFileRepository
    {
        private readonly ILogger<VirtualPrintFileRepository> logger;

        public VirtualPrintFileRepository(ILogger<VirtualPrintFileRepository> logger)
        {
            this.logger = logger;
        }

        public void Print(PrintFileInfo printFileInfo)
        {
            try
            {
                logger.LogInformation("虚拟打印文件，文件类型：{FileExtend}", printFileInfo.FileExtend);
                // 模拟打印成功
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "虚拟打印文件时发生错误");
            }
        }

        public PrinterStatus GetPrinterStatus()
        {
            logger.LogInformation("获取虚拟打印机状态");
            return new PrinterStatus
            {
                Code = 0,
                Desc = "虚拟打印机正常"
            };
        }
    }
}