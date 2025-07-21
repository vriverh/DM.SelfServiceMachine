using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Models
{
    /// <summary>
    /// 打印文件信息
    /// </summary>
    public class PrintFileInfo
    {
        /// <summary>
        /// 传输类型，1：base64，2：本地文件（默认）
        /// </summary>
        public int TranType { get; set; }
        /// <summary>
        /// 文档绝对路径，TranType为2或空有效
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件扩展类型，TranType为1有效，如pdf，doc
        /// </summary>
        public string FileExtend { get; set; }
        /// <summary>
        /// 文件流Base64字符串，TranType为1有效
        /// </summary>
        public string FileContent { get; set; }

        public override string ToString()
        {
            return "传输类型=" + TranType + "，文档绝对路径=" + FilePath + "，文件扩展类型=" + FileExtend + "，文件流Base64字符串长度=" + FileContent.Length;
        }
    }
}
