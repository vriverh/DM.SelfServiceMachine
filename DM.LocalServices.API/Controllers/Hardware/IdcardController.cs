using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.API.Controllers.Api.Hardware
{
    [ApiController]
    [Route("api/hardware/[controller]")]
    public class IdcardController : Controller
    {
        
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<IdcardController> logger;

        private readonly IReadRepository readRepository;

        public IdcardController(ILogger<IdcardController> logger,IReadRepository readRepository)
        {
            this.logger = logger;
            this.readRepository = readRepository;
        }

        [HttpGet("read")]
        public ResultMessage<ReadInfo> Read(int timeout = 30)
        {
            try
            {
                ReadInfo readInfo = readRepository.GetReadInfo(timeout);
                return new ResultMessage<ReadInfo>() { State = 1, Message = "成功", Row = readInfo };
            }
            catch (Exception ex)
            {
                string exStr = "读取身份证，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<ReadInfo>() { State = 0, Message = exStr, Row = null };
            }
        }

        [HttpGet("stop")]
        public ResultMessage<string> Stop()
        {
            try
            {
                readRepository.CancelGetReadInfo();
                return new ResultMessage<string>() { State = 1, Message = "成功", Row = null };
            }
            catch (Exception ex)
            {
                string exStr = "停止读取身份证，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<string>() { State = 0, Message = exStr, Row = null };
            }
        }
    }
}
