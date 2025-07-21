using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Models
{
    public class UpdateInfo
    {
        /// <summary>
        /// 最新版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 安装包地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 是否强制升级:否false,是true
        /// </summary>
        public bool Mandatory { get; set; }

        /// <summary>
        /// 升级模式:普通false,静默升级true
        /// </summary>
        public bool Mode { get; set; }

        /// <summary>
        /// 上线时间戳
        /// </summary>
        public long Onlinetime { get; set; }

    }
}
