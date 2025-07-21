using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Models
{
    /// <summary>
    /// 结果消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultMessage<T>
    {
        /// <summary>
        /// 状态，1：成功，0：失败
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 提示信息，成功活失败的信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Row { get; set; }
    }
}
