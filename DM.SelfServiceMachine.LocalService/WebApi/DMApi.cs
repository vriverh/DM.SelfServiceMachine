using AutoUpdaterDotNET;
using DM.SelfServiceMachine.LocalService.Models;
using DM.SelfServiceMachine.LocalService.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.WebApi
{
    /// <summary>
    /// 已弃用
    /// </summary>
    public class DMApi
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<DMApi> logger;

        private readonly IDMApi dmApi;

        private readonly IDevInfoRepository devInfoRepository;

        private readonly IConfiguration configuration;

        private bool state = false;

        private string url = "";

        public DMApi(ILogger<DMApi> logger,IDMApi dmApi, IDevInfoRepository devInfoRepository, IConfiguration configuration)
        {
            this.logger = logger;
            this.dmApi = dmApi;
            this.devInfoRepository = devInfoRepository;
            this.configuration = configuration;
        }

        public void Start()
        {
            Task.Run(Regist);
            Task.Run(UpdateApp);
        }

        public async void Regist()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(30);
            DevInfo devInfo = devInfoRepository.GetDevInfo();
            //if (devInfo.DebugModel)
            //{
            //    logger.LogDebug("开始进入dubug模式");
            //    App.Current.Dispatcher.Invoke(SetDebugModel);
            //}

            while (configuration.GetValue<bool>("Regist"))
            {
                try
                {
                    logger.LogDebug("开始注册");
                    DMResultMessage<DevInfo> resultMessage = await dmApi.RegistClient(devInfo);
                    if (resultMessage.Code == 200)
                    {
                        devInfo.No = resultMessage.Data.No;
                        devInfo.Name = resultMessage.Data.Name;
                        devInfo.State = resultMessage.Data.State;
                        devInfo.DefaultUrl = resultMessage.Data.DefaultUrl;
                        state = devInfo.State == 0;
                        url = devInfo.DefaultUrl;
                        logger.LogDebug("设置系统状态");
                        App.Current.Dispatcher.Invoke(SetWinInfo);
                    }
                    else
                    {
                        logger.LogDebug("注册心跳请求返回了一个错误，错误详情：", resultMessage.Message);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "注册心跳请求发生错误，错误详情：", ex.Message);
                }
                finally
                {
                    await Task.Delay(timeSpan);
                }
            }
        }

        private async void SetWinInfo()
        {
            while (App.Current.MainWindow?.IsLoaded != true)
            {
                await Task.Delay(10);
            }
            ((MainWindow)App.Current.MainWindow).SetUrl(url);
            ((MainWindow)App.Current.MainWindow).SetState(state);
        }

        public async void UpdateApp()
        {
            if (configuration.GetValue<bool>("Regist"))
            {
                logger.LogDebug("开始获取更新信息");
                try
                {
                    DMResultMessage<UpdateInfo> resultMessage = await dmApi.UpdateApp();
                    if (resultMessage.Code == 200)
                    {
                        string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoupdaterXML.txt");
                        string xmlTemplate = await File.ReadAllTextAsync(xmlPath);
                        string xmlStr = xmlTemplate.Replace("{version}", resultMessage.Data.Version)
                            .Replace("{url}", resultMessage.Data.Url)
                            .Replace("{mandatory}", resultMessage.Data.Mandatory.ToString().ToLower())
                            .Replace("{mode}", resultMessage.Data.Mode ? "2" : "1");
                        //string autoupdaterXML = @"C:\Users\vrive\AppData\Local\Temp\tmpF051.tmp";
                        string autoupdaterXML = Path.GetTempFileName();
                        await File.WriteAllTextAsync(autoupdaterXML, xmlStr);
                        logger.LogDebug("获取更新参数成功：版本号：{0}，URL：{1}", resultMessage.Data.Version, resultMessage.Data.Url);
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            AutoUpdater.Start(autoupdaterXML);
                        });
                    }
                    else
                    {
                        logger.LogDebug("更新程序接口返回错误，错误编号：{0}，错误详情：{1}", resultMessage.Code, resultMessage.Message);
                    }
                }
                catch (Exception ex)
                {

                    logger.LogWarning(ex, "更新程序发生错误，错误详情：", ex.Message);
                }
            }
        }
    }
}
