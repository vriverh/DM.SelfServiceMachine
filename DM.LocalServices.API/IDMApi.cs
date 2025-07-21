using DM.LocalServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace DM.LocalServices.API
{
    [DMFilter]
    public interface IDMApi
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="mac"></param>
        /// <param name="ip"></param>
        /// <param name="state"></param>
        /// <param name="version"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("/szbdc-web/aio/regist")]
        Task<DMResultMessage<DevInfo>> RegistClient([JsonContent] DevInfo devInfo);

        /// <summary>
        /// 升级
        /// </summary>
        /// <returns></returns>
        [HttpPost("szbdc-web/aio/app")]
        Task<DMResultMessage<UpdateInfo>> UpdateApp();
    }
}
