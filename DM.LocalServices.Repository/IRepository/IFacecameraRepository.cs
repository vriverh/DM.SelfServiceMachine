using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.Repository.IRepository
{
    public interface IFacecameraRepository
    {
        /// <summary>
        /// 打开摄像头
        /// </summary>
        public void Open(int x, int y, int w, int h);

        /// <summary>
        /// 获取人像接口
        /// </summary>
        /// <returns></returns>
        public string GetFaceImage();

        /// <summary>
        /// 获取当前摄像头画面
        /// </summary>
        /// <returns></returns>
        public string GetCurrentImage();

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void Close();

        /// <summary>
        /// 人脸比较
        /// </summary>
        /// <param name="compFaceInfo"></param>
        /// <returns></returns>
        public int CompFace(string faceImage1, string faceImage2);

    }
}
