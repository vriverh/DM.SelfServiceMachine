using DM.SelfServiceMachine.LocalService.Models;
using DM.SelfServiceMachine.LocalService.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IDevInfoRepository devInfoRepository;

        public ServiceController(IDevInfoRepository devInfoRepository)
        {
            this.devInfoRepository = devInfoRepository;
        }

        [HttpGet("devInfo")]
        public ResultMessage<DevInfo> DevInfo()
        {
            try
            {
                DevInfo devInfo= devInfoRepository.GetDevInfo();
                return new ResultMessage<DevInfo>() { Message = "成功", State = 1, Row = devInfo };
            }
            catch (Exception ex)
            {
                return new ResultMessage<DevInfo>() { Message = ex.Message, State = 0, Row = null };
            }

        }
        [HttpGet("closeWindow")]
        public void CloseWindow()
        {
            App.Current.Dispatcher.Invoke(() => App.Current.Shutdown());
        }
    }
}
