using DM.SelfServiceMachine.LocalService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Controllers.Api.Hardware
{
    [ApiController]
    [Route("api/hardware/[controller]")]
    public class FprController : ControllerBase
    {
        [HttpPost("read")]
        public ResultMessage<string> Read()
        {
            try
            {
                return new ResultMessage<string>() { State = 0, Message = "当前设备无法采集指纹。", Row = null };
            }
            catch (Exception ex)
            {
                return new ResultMessage<string>() { State = 0, Message = ex.Message, Row = null };
            }
        }

        [HttpGet("cancel")]
        public ResultMessage<string> Cancel()
        {
            try
            {
                return new ResultMessage<string>() { State = 0, Message = "当前设备无法采集指纹。", Row = null };
            }
            catch (Exception ex)
            {
                return new ResultMessage<string>() { State = 0, Message = ex.Message, Row = null };
            }
        }
    }
}
