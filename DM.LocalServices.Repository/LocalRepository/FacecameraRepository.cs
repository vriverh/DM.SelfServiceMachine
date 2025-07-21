using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DM.LocalServices.Repository.LocalRepository
{
    public class FacecameraRepository : IFacecameraRepository
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<FacecameraRepository> logger;

        /// <summary>
        /// 人脸相机服务
        /// </summary>
        private readonly IFaceCameraService faceCameraService;

        /// <summary>
        /// 是否取消
        /// </summary>
        private bool isClose = false;

        public FacecameraRepository(ILogger<FacecameraRepository> logger, IFaceCameraService faceCameraService)
        {
            this.logger = logger;
            this.faceCameraService = faceCameraService;
        }

        public void Open(int x, int y, int w, int h)
        {
            try
            {
                logger.LogInformation("打开人脸相机，位置：({X},{Y})，大小：{W}x{H}", x, y, w, h);
                isClose = false;
                
                if (!faceCameraService.Initialize())
                {
                    throw new ApplicationException("初始化人脸相机失败");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "打开人脸相机时发生错误");
                throw;
            }
        }

        public string GetFaceImage()
        {
            try
            {
                logger.LogInformation("开始获取人脸图像");
                
                if (isClose)
                {
                    throw new ApplicationException("相机已关闭");
                }

                var imageData = faceCameraService.CaptureImage();
                if (imageData == null || imageData.Length == 0)
                {
                    throw new ApplicationException("获取图像数据失败");
                }

                return Convert.ToBase64String(imageData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取人脸图像时发生错误");
                throw;
            }
        }

        public string GetCurrentImage()
        {
            try
            {
                logger.LogInformation("获取当前相机画面");
                
                if (isClose)
                {
                    throw new ApplicationException("相机已关闭");
                }

                var imageData = faceCameraService.CaptureImage();
                if (imageData == null || imageData.Length == 0)
                {
                    throw new ApplicationException("获取图像数据失败");
                }

                return Convert.ToBase64String(imageData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取当前画面时发生错误");
                throw;
            }
        }

        public void Close()
        {
            try
            {
                logger.LogInformation("关闭人脸相机");
                isClose = true;
                faceCameraService.Close();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "关闭人脸相机时发生错误");
            }
        }

        public int CompFace(string faceImage1, string faceImage2)
        {
            try
            {
                logger.LogInformation("开始人脸比较");
                // 简化实现，返回模拟相似度
                // 实际实现应该使用人脸识别算法
                return 85; // 返回85%相似度作为示例
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "人脸比较时发生错误");
                return 0;
            }
        }
    }
}