using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.API.WebApi
{
    /// <summary>
    /// 结果消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DMResultMessage<T>
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 提示信息，成功活失败的信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }
}
