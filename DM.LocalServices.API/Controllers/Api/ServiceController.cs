using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.API.Controllers.Api
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
            // In a pure API application, we can't directly shutdown like in WPF
            // This would need to be handled differently, perhaps by triggering an application shutdown
            Environment.Exit(0);
        }
    }
}
