using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DM.LocalServices.Device
{
    public class TaiSDK
    {
        /// <summary>
        /// 算法初始化，在调用其他接口前调用。
        /// </summary>
        /// <param name="hCtx">初始化成功后的算法句柄。</param>
        /// <returns>大于 0：成功（返回值是二进制特征的长度）；其他值：失败（返回的是对应错误码）。</returns>
        [DllImport("TaiSDK.dll", EntryPoint = "face_init", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int FaceInit(int[] hCtx);

        /// <summary>
        /// 算法卸载，宿主程序退出前调用。
        /// </summary>
        /// <param name="hCtx">算法句柄</param>
        /// <returns>0：成功；其他值：失败（返回的是对应错误码）。</returns>
        [DllImport("TaiSDK.dll", EntryPoint = "face_exit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int FaceExit(int hCtx);

        /// <summary>
        /// 返回内存图像中人脸的特征
        /// </summary>
        /// <param name="hCtx">算法句柄</param>
        /// <param name="pic_bin">内存图像的地址</param>
        /// <param name="pic_len">内存图像的长度</param>
        /// <param name="feature">成功则返回人脸的特征。需要客户预先分配好内存，建议内存大小建议为函数 face_init()调用成功时返回值的 2 倍；</param>
        /// <returns></returns>
        [DllImport("TaiSDK.dll", EntryPoint = "face_get_feature_from_image", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int FaceGetFeatureFromImage(int hCtx, byte[] pic_bin, int pic_len, byte[] feature);

        /// <summary>
        /// 比对两个特征， 返回相似度得分（0-100）
        /// </summary>
        /// <param name="hCtx">算法句柄</param>
        /// <param name="feature1">人脸特征数据 1</param>
        /// <param name="feature2">人脸特征数据 2</param>
        /// <returns></returns>
        [DllImport("TaiSDK.dll", EntryPoint = "face_comp_feature", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern int FaceCompFeature(int hCtx, byte[] feature1, byte[] feature2);
    }
}
