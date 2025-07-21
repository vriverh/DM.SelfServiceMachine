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
    public class SncameraController : ControllerBase
    {
        [HttpPost("open")]
        public ResultMessage<string> Open()
        {
            try
            {
                return new ResultMessage<string>() { State = 0, Message = "当前设备无高拍仪设备。", Row = null };
            }
            catch (Exception ex)
            {
                return new ResultMessage<string>() { State = 0, Message = ex.Message, Row = null };
            }
        }


        [HttpGet("capture")]
        public ResultMessage<string> Capture()
        {
            try
            {
                return new ResultMessage<string>() { State = 0, Message = "当前设备无高拍仪设备。", Row = null };
            }
            catch (Exception ex)
            {
                return new ResultMessage<string>() { State = 0, Message = ex.Message, Row = null };
            }
        }

        [HttpGet("close")]
        public ResultMessage<string> Close()
        {
            try
            {
                return new ResultMessage<string>() { State = 0, Message = "当前设备无高拍仪设备。", Row = null };
            }
            catch (Exception ex)
            {
                return new ResultMessage<string>() { State = 0, Message = ex.Message, Row = null };
            }
        }
    }
}
