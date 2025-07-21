using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DM.LocalServices.Repository.VirtualRepository
{
    public class VirtualFacecameraRepository : IFacecameraRepository
    {
        /// <summary>
        /// 配置参数
        /// </summary>
        private readonly IConfiguration configuration;
        public VirtualFacecameraRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// 打开摄像头
        /// </summary>
        public void Open(int x, int y, int w, int h)
        {
        }

        /// <summary>
        /// 获取人像接口
        /// </summary>
        /// <returns></returns>
        public string GetFaceImage()
        {

            string image = configuration.GetValue<string>("FaceImage");
            if (File.Exists(image))
            {
                byte[] imageByte = File.ReadAllBytes(image);
                return Convert.ToBase64String(imageByte);
            }
            return "";
        }

        /// <summary>
        /// 获取当前摄像头画面
        /// </summary>
        /// <returns></returns>
        public string GetCurrentImage()
        {
            string image = configuration.GetValue<string>("CurrentImage");
            if (File.Exists(image))
            {
                byte[] imageByte = File.ReadAllBytes(image);
                return Convert.ToBase64String(imageByte);
            }
            return "";
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// 人脸比较
        /// </summary>
        /// <param name="faceImage1"></param>
        /// <param name="faceImage2"></param>
        /// <returns></returns>
        public int CompFace(string faceImage1, string faceImage2)
        {
            // 虚拟实现，返回配置中的值或默认值
            return configuration.GetValue<int>("CompFace", 85);
        }
    }
}