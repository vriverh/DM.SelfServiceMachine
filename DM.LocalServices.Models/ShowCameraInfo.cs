using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.Models
{
    public class ShowCameraInfo
    {
        /// <summary>
        /// 窗口X位置，摄像头画面显示X位置
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 窗口Y位置，摄像头画面显示X位置
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 窗口宽，摄像头画面显示宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 窗口高，摄像头画面显示高度
        /// </summary>
        public int Height { get; set; }
    }
}
