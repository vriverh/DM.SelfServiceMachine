using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.Models
{
    public class PrinterStatus
    {
        /// <summary>
        /// 状态代码 0 打印机状态正常 非0 打印机状态异常
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string Desc { get; set; }
    }
}
