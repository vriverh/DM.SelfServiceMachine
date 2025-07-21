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
    public class FacecameraController : ControllerBase
    {

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<FacecameraController> logger;

        private readonly IFacecameraRepository facecameraRepository;

        public FacecameraController(ILogger<FacecameraController> logger,IFacecameraRepository  facecameraRepository)
        {
            this.logger = logger;
            this.facecameraRepository = facecameraRepository;
        }

        [HttpPost("open")]
        public ResultMessage<string> Open(ShowCameraInfo showCameraInfo)
        {
            try
            {
                facecameraRepository.Open(showCameraInfo.X, showCameraInfo.Y, showCameraInfo.Width, showCameraInfo.Height);
                return new ResultMessage<string>() { State = 1, Message = "成功", Row = "" };
            }
            catch (Exception ex)
            {
                string exStr = "打开摄像头，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<string>() { State = 0, Message = exStr, Row = null };
            }
        }

        [HttpGet("getFaceImage")]
        public ResultMessage<string> GetFaceImage()
        {
            try
            {
                string faceStr = facecameraRepository.GetFaceImage();
                return new ResultMessage<string>() { State = 1, Message = "获取人脸图像成功", Row = faceStr };
            }
            catch (Exception ex)
            {
                string exStr = "获取人脸图片，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<string>() { State = -1, Message = exStr, Row = null };
            }
        }

        [HttpGet("getCurrentImage")]
        public ResultMessage<string> GetCurrentImage()
        {
            try
            {
                string cureentStr = facecameraRepository.GetCurrentImage();
                return new ResultMessage<string>() { State = 1, Message = "获取当前图像成功", Row = cureentStr };
            }
            catch (Exception ex)
            {
                string exStr = "获取当前摄像机图片，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<string>() { State = 0, Message = exStr, Row = null };
            }
        }

        [HttpGet("close")]
        public ResultMessage<string> Close()
        {
            try
            {
                facecameraRepository.Close();
                return new ResultMessage<string>() { State = 1, Message = "关闭成功", Row = null };
            }
            catch (Exception ex)
            {
                string exStr = "关闭摄像头，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<string>() { State = 0, Message = exStr, Row = null };
            }
        }

        [HttpPost("compFace")]
        public ResultMessage<int?> CompFace(CompFaceInfo compFaceInfo)
        {
            try
            {
                int compValue = facecameraRepository.CompFace(compFaceInfo.FaceImage1, compFaceInfo.FaceImage2);
                return new ResultMessage<int?>() { State = 1, Message = "对比成功", Row = compValue };
            }
            catch (Exception ex)
            {
                string exStr = "人脸对比，发生了一个的错误，错误详情：" + ex.Message;
                logger.LogWarning(ex, exStr);
                return new ResultMessage<int?>() { State = 0, Message = ex.Message, Row = null };
            }
        }
    }
}
