using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Models
{
    /// <summary>
    /// 设备的基本信息
    /// </summary>
    public class DevInfo
    {
        public const string Position = "DevInfo";

        /// <summary>
        /// 一体机编码
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 一体机名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 一体机所在登记所编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// MAC地址，格式为：F8-FF-C2-2B-CE-C3
        /// </summary>
        public string Mac { get; set; }

        /// <summary>
        /// IP地址，格式为：203.175.130.200
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 当前安装包版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 状态（0停用，1正常）
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 默认地址
        /// </summary>
        public string DefaultUrl { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 中心端URL
        /// </summary>
        public string QueueCenterURL { get;set; }

        /// <summary>
        /// 叫号中心移动端地址
        /// </summary>
        public string QueueWebURL { get;  set; }
    }
}
