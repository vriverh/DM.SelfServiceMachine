using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.Models
{
    public class ReadInfo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别，中午：男，女
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 民族，中文：民族
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 出生日期，格式为：19840101
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Addr { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string Idcode { get; set; }
        /// <summary>
        /// 发证机关
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// 有效开始日期，格式为：20200101
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// 有效截止日期，格式为：20200101
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 扩展地址信息，可为空
        /// </summary>
        public string AppendMsg { get; set; }
        /// <summary>
        /// 身份证人像图片数据，Base64
        /// </summary>
        public string ImageStr { get; set; }
        /// <summary>
        /// 身份证指纹数据，Base64
        /// </summary>
        public string FprImageStr { get; set; }

    }
}
