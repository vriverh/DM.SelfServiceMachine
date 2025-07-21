using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Tools
{
    public static class EcFaceCamSDK
    {

        /// <summary>
        /// 算法回调函数
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="context"></param>
        public delegate void CallbackDelegate(int eventId, IntPtr context);

        /// <summary>
        /// 设置事件回调函数
        /// </summary>
        /// <param name="cbd"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_SetCallBack", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_SetCallBack(CallbackDelegate cbd, IntPtr context);

        /// <summary>
        /// 创建窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_CreateWindow", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static IntPtr ECF_CreateWindow();

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_ShowWindow", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void ECF_ShowWindow(IntPtr hWnd, int x, int y, int w, int h);

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_HideWindow", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void ECF_HideWindow(IntPtr hWnd);

        /// <summary>
        /// 销毁窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_DestroyWindow", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static void ECF_DestroyWindow(IntPtr hWnd);

        /// <summary>
        /// 设置视频窗口
        /// </summary>
        /// <param name="nWndType"></param>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_SetDisplayWindow", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_SetDisplayWindow(int nWndType, IntPtr hWnd);


        /// <summary>
        /// 打开摄像头
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_Init", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_Init();

        /// <summary>
        /// 打开摄像头
        /// </summary>
        /// <param name="pszXmlPrameters"></param>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_Open", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_Open(string pszXmlPrameters);

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_Close", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_Close();

        /// <summary>
        /// 启动检活异步
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_StartDetectAsyn", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_StartDetectAsyn();

        /// <summary>
        /// 启动检活同步
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_StartDetectSyn", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_StartDetectSyn();

        /// <summary>
        /// 停止检活
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_Stop", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_Stop();

        /// <summary>
        /// 获取检活结果数据
        /// </summary>
        /// <param name="nType"></param>
        /// <param name="dataBuf"></param>
        /// <param name="dataLen"></param>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_GetImageData", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_GetImageData(ImageType nType, byte[] dataBuf,out int dataLen);

        /// <summary>
        /// 获取检活结果数据
        /// </summary>
        /// <param name="nImageType"></param>
        /// <param name="pImgJpg"></param>
        /// <param name="pnJpgLen"></param>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_SnapFrame", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_SnapFrame(int nImageType,byte[] pImgJpg,out int pnJpgLen);

        /// <summary>
        /// 反初始化
        /// </summary>
        /// <returns></returns>
        [DllImport("EcFaceCamSDK.dll", EntryPoint = "ECF_Exit", ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public extern static int ECF_Exit();
    }

    // 事件定义
    public enum CallBackEvent
    {
        CALLBACK_EVENT_WAIT = -1,
        CALLBACK_EVENT_SUCC = 100,
        CALLBACK_EVENT_FAIL = 101,
        CALLBACK_EVENT_TIMEOUT = 102,
        CALLBACK_EVENT_SNAP = 103,
        CALLBACK_EVENT_CANCEL = 104
    }

    // 图像选项
    public enum ImageType
    {
        IMAGE_TYPE_VIS = 0,
        IMAGE_TYPE_NIR = 1,
        IMAGE_TYPE_VIS_RC = 2,
        IMAGE_TYPE_NIR_RC = 3,
        IMAGE_TYPE_CROP_VIS = 4,
        IMAGE_TYPE_CROP_NIR = 5
    }
}
