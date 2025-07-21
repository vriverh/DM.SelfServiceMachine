using DM.SelfServiceMachine.LocalService.Models;
using DM.SelfServiceMachine.LocalService.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Controllers.Api.Hardware
{
    [ApiController]
    [Route("api/hardware/[controller]")]
    public class PrintController : ControllerBase
    {

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<PrintController> logger;

        private readonly IPrintFileRepository printFileRepository;

        public PrintController(ILogger<PrintController> logger,IPrintFileRepository  printFileRepository)
        {
            this.logger = logger;
            this.printFileRepository = printFileRepository;
        }

        [HttpPost("docPrint")]
        public ResultMessage<string> DocPrint(PrintFileInfo printFileInfo)
        {
            try
            {
                printFileRepository.Print(printFileInfo);
                return new ResultMessage<string>() { State = 1, Message = "成功", Row = null };
            }
            catch (Exception ex)
            {
                string exStr = "打印文件，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<string>() { State = 0, Message = exStr, Row = null };
            }
        }


        [HttpGet("getDocStatus")]
        public ResultMessage<PrinterStatus> GetDocStatus()
        {
            try
            {
                PrinterStatus printerStatus = printFileRepository.GetPrinterStatus();
                return new ResultMessage<PrinterStatus>() { State = 1, Message = "成功", Row = printerStatus };
            }
            catch (Exception ex)
            {
                string exStr = "获取打印状态，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<PrinterStatus>() { State = 0, Message = exStr, Row = null };
            }
        }
    }
}
