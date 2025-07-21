using AutoUpdaterDotNET;
using DM.LocalServices.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DM.LocalServices.API.WebApi
{
    public class AutoUpdaterService : BackgroundService
    {
        private readonly ILogger<AutoUpdaterService> logger;
        private readonly IDMApi dmApi;
        private readonly IConfiguration configuration;
        private Task executingTask;

        public AutoUpdaterService(ILogger<AutoUpdaterService> logger, IDMApi dmApi, IConfiguration configuration)
        {
            this.logger = logger;
            this.dmApi = dmApi;
            this.configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            string defaultURL = configuration.GetValue<string>("DefaultURL");
            if (string.IsNullOrEmpty(defaultURL))
            {

                if (Process.GetProcessesByName("ProcessCheck").Length == 0)
                {
                    string processCheckPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProcessCheck.exe");
                    Process.Start(processCheckPath);
                }

                Task task = UpdateApp();

                executingTask = ExecuteAsync(cancellationToken);
                if (executingTask.IsCompleted)
                {
                    return executingTask;
                }
            }
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await UpdateApp();
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime updateTime = DateTime.Today.AddHours(18.5);
                TimeSpan delayTimeSpan;
                if (updateTime > DateTime.Now)
                {
                    delayTimeSpan = updateTime - DateTime.Now;
                }
                else
                {
                    delayTimeSpan = updateTime.AddDays(1) - DateTime.Now;
                }
                await Task.Delay(delayTimeSpan, stoppingToken);
                try
                {
                    await UpdateApp();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "更新服务发生了一个错误，错误信息：{0}", ex.Message);
                }
            }
        }


        public async Task UpdateApp()
        {
            logger.LogDebug("开始获取更新信息");
            try
            {
                DMResultMessage<UpdateInfo> resultMessage = await dmApi.UpdateApp();
                if (resultMessage.Code == 200)
                {
                    if (configuration.GetValue<bool>("Debug"))
                    {
                        resultMessage.Data.Mode = false;
                        resultMessage.Data.Mandatory = false;
                    }
                    
                    string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoupdaterXML.txt");
                    string xmlTemplate = await File.ReadAllTextAsync(xmlPath);
                    string updateUrl = GetUrl(resultMessage.Data.Url);
                    string xmlStr = xmlTemplate.Replace("{version}", resultMessage.Data.Version)
                        .Replace("{url}", updateUrl)
                        .Replace("{mandatory}", resultMessage.Data.Mandatory.ToString().ToLower())
                        .Replace("{mode}", resultMessage.Data.Mode ? "2" : "1");
                    string autoupdaterXML = Path.GetTempFileName();
                    await File.WriteAllTextAsync(autoupdaterXML, xmlStr);
                    logger.LogDebug("获取更新参数成功：版本号：{0}，URL：{1}", resultMessage.Data.Version, updateUrl);

                    AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
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

        private void AutoUpdater_ApplicationExitEvent()
        {
            Process[] processes = Process.GetProcessesByName("ProcessCheck");
            if (processes.Length != 0)
            {
                for (int i = 0; i < processes.Length; i++)
                {
                    Process process = processes[i];
                    process.Kill();
                }
            }
            App.Current.Shutdown();
        }

        private string GetUrl(string url)
        {
            //var section = configuration.GetSection(nameof(IDMApi));
            Uri host = new Uri(configuration.GetValue<string>("IDMApi:HttpHost"));
            return (new Uri(host, url)).ToString();
        }
    }
}
