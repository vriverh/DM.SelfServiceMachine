using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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
        /// 窗口句柄
        /// </summary>
        private IntPtr windowPtr = IntPtr.Zero;

        /// <summary>
        /// 算法句柄
        /// </summary>
        private int hCtx = 0;

        /// <summary>
        /// 二进制特征的长度
        /// </summary>
        private int featureLength = 0;

        /// <summary>
        /// 是否取消
        /// </summary>
        private bool isClose = false;

        /// <summary>
        /// 回调函数
        /// </summary>
        private EcFaceCamSDK.CallbackDelegate callbackDelegate;

        /// <summary>
        /// 回调事件参数枚举
        /// </summary>
        private CallBackEvent callbackEvent = CallBackEvent.CALLBACK_EVENT_WAIT;

        public FacecameraRepository(ILogger<FacecameraRepository> logger)
        {
            this.logger = logger;
            //this.InitFace();
        }

        /// <summary>
        /// 打开摄像头
        /// </summary>
        public void Open(int x, int y, int w, int h)
        {
            isClose = false;
            logger.LogDebug("开始打开摄像头");
            int rv = 0;
            if (windowPtr == IntPtr.Zero)
            {
                windowPtr = EcFaceCamSDK.ECF_CreateWindow();
                logger.LogDebug("生产的Windows窗口句柄：{0}", windowPtr);
                rv = EcFaceCamSDK.ECF_SetDisplayWindow(0, windowPtr);
                logger.LogDebug("设置windows返回值：{0}", rv);


                callbackDelegate = new EcFaceCamSDK.CallbackDelegate(funCallBackEvent);
                EcFaceCamSDK.ECF_SetCallBack(callbackDelegate, IntPtr.Zero);
            }

            string xmlSamples = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xmlSamples.txt");
            string pszXmlPrameters = File.ReadAllText(xmlSamples);
            //rv = EcFaceCamSDK.ECF_Init();
            //logger.LogDebug("初始化返回值：{0}", rv);
            rv = EcFaceCamSDK.ECF_Open(pszXmlPrameters);
            logger.LogDebug("打开摄像头返回值：{0}", rv);
            if (rv != 0)
            {
                throw new ApplicationException("打开摄像头发生错误，设备错误返回值：" + rv);
            }
            if (x > 0 && y > 0)
            {
                EcFaceCamSDK.ECF_ShowWindow(windowPtr, x, y, w, h);
            }

        }

        /// <summary>
        /// 获取人像接口
        /// </summary>
        /// <returns></returns>
        public string GetFaceImage()
        {
            logger.LogDebug("开始获取人像");
            callbackEvent = CallBackEvent.CALLBACK_EVENT_WAIT;
            int rv = EcFaceCamSDK.ECF_StartDetectAsyn();
            if (rv == 0)
            {
                while (true)
                {
                    if (isClose)
                    {
                        throw new ApplicationException("活体检测被取消");
                    }
                    switch (callbackEvent)
                    {
                        case CallBackEvent.CALLBACK_EVENT_WAIT:
                            Task.Delay(100).Wait();
                            break;
                        case CallBackEvent.CALLBACK_EVENT_SUCC:
                            byte[] dataBuf = new byte[1024 * 1024];
                            EcFaceCamSDK.ECF_GetImageData(ImageType.IMAGE_TYPE_CROP_VIS, dataBuf, out int dataLen);
                            string faceBase64 = Convert.ToBase64String(dataBuf, 0, dataLen);
                            logger.LogDebug("获取到的人像内容成功");
                            return faceBase64;
                        case CallBackEvent.CALLBACK_EVENT_FAIL:
                            throw new ApplicationException("活体检测未通过");
                        case CallBackEvent.CALLBACK_EVENT_TIMEOUT:
                            throw new ApplicationException("活体检测超时");
                        case CallBackEvent.CALLBACK_EVENT_SNAP:
                            throw new ApplicationException("从设备获取图片失败");
                        case CallBackEvent.CALLBACK_EVENT_CANCEL:
                            throw new ApplicationException("活体检测被取消");
                        default:
                            throw new ApplicationException("获取人像发生了未知的错误，设备错误返回值：" + rv);
                    }
                }
            }
            else
            {
                throw new ApplicationException("获取人像发生了未知的错误，设备错误返回值：" + rv);
            }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="context"></param>
        public void funCallBackEvent(int eventId, IntPtr context)
        {
            if (eventId >= 100) // 100以上是结束事件，<100的是过程事件不做处理
            {
                switch (eventId)
                {
                    case (int)CallBackEvent.CALLBACK_EVENT_SUCC:
                    case (int)CallBackEvent.CALLBACK_EVENT_FAIL:
                    case (int)CallBackEvent.CALLBACK_EVENT_TIMEOUT:
                    case (int)CallBackEvent.CALLBACK_EVENT_SNAP:
                    case (int)CallBackEvent.CALLBACK_EVENT_CANCEL:
                        callbackEvent = (CallBackEvent)eventId;
                        break;
                    default:
                        callbackEvent = CallBackEvent.CALLBACK_EVENT_CANCEL;
                        break;
                }
            }
        }

        /// <summary>
        /// 获取当前摄像头画面
        /// </summary>
        /// <returns></returns>
        public string GetCurrentImage()
        {
            logger.LogDebug("开始获取当前摄像头画面");
            byte[] pImgJpg = new byte[1024 * 1024];
            int rv = EcFaceCamSDK.ECF_SnapFrame(0, pImgJpg, out int pnJpgLen);
            logger.LogDebug("获取当前摄像头画面，设备接口返回：{0}", rv);
            if (rv == 0)
            {
                string currentBase64 = Convert.ToBase64String(pImgJpg, 0, pnJpgLen);
                logger.LogDebug("获取当前摄像头画面成功");
                return currentBase64;
            }
            else
            {
                throw new ApplicationException("获取当前摄像头画面发生错误，设备错误返回值：" + rv);
            }
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        public void Close()
        {
            logger.LogDebug("开始关闭摄像头");
            EcFaceCamSDK.ECF_HideWindow(windowPtr);
            int rv = EcFaceCamSDK.ECF_Stop();
            logger.LogDebug("停止活体检测返回值：{0}", rv);
            rv = EcFaceCamSDK.ECF_Close();
            logger.LogDebug("关闭摄像头返回值：{0}", rv);
            isClose = true;
        }

        /// <summary>
        /// 人脸比较
        /// </summary>
        /// <param name="compFaceInfo"></param>
        /// <returns></returns>
        public int CompFace(string faceImage1,string faceImage2)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                logger.LogDebug("开始人脸比较");
                InitFace();
            });

            byte[] feature1 = new byte[featureLength * 2];
            byte[] feature2 = new byte[featureLength * 2];

            byte[] face1 = Convert.FromBase64String(faceImage1);
            byte[] face2 = Convert.FromBase64String(faceImage2);

            logger.LogDebug("开始对人脸图片计算出特征值");
            int nRet1 = TaiSDK.FaceGetFeatureFromImage(hCtx, face1, face1.Length, feature1);
            int nRet2 = TaiSDK.FaceGetFeatureFromImage(hCtx, face2, face2.Length, feature2);

            logger.LogDebug("开始对特征值进行比较");
            int nScore = TaiSDK.FaceCompFeature(hCtx, feature1, feature2);

            logger.LogDebug("这两个人脸的特征对比，相似度为：", nScore);
            return nScore;
        }

        private void InitFace()
        {
            if (hCtx == 0)
            {
                logger.LogDebug("开始初始化人脸比较");
                int[] hCtxs = { 0 };
                int nRetInit = TaiSDK.FaceInit(hCtxs);
                if (nRetInit > 0)
                {
                    hCtx = hCtxs[0];
                    featureLength = nRetInit;
                    logger.LogDebug("初始化成功，算法句柄：{0}，二进制特征长度：{1}", hCtx, featureLength);
                }
                else
                {
                    string errorStr = "初始化算法发生错误，算法错误返回值：" + nRetInit;
                    logger.LogDebug(errorStr);
                    throw new ApplicationException(errorStr);
                }
            }
        }
    }
}