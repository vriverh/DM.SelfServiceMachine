using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DM.LocalServices.API
{

    public class ClientRegistService : BackgroundService
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<ClientRegistService> logger;

        private readonly IDMApi dmApi;

        private readonly IDevInfoRepository devInfoRepository;

        private IConfiguration configuration;

        private bool state = false;

        private string url = "";

        private Task executingTask;

        public ClientRegistService(ILogger<ClientRegistService> logger, IDMApi dmApi, IDevInfoRepository devInfoRepository, IConfiguration configuration)
        {
            this.logger = logger;
            this.dmApi = dmApi;
            this.devInfoRepository = devInfoRepository;
            this.configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            if (configuration.GetValue<bool>("Debug"))
            {
                // Removed WPF dependency - API service doesn't have debug UI mode
                SetDebugModel();
            }
            string defaultURL = configuration.GetValue<string>("DefaultURL");
            if (string.IsNullOrEmpty(defaultURL))
            {
                executingTask = ExecuteAsync(cancellationToken);
                if (executingTask.IsCompleted)
                {
                    return executingTask;
                }
            }
            else
            {
                url = defaultURL;
                state= false;
                SetWinInfo();
            }
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(30);
            DevInfo devInfo = devInfoRepository.GetDevInfo();
            while (!stoppingToken.IsCancellationRequested)
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

                        devInfo.DefaultUrl = GetUrl(resultMessage.Data.DefaultUrl);
                        state = devInfo.State == 0;
                        url = devInfo.DefaultUrl;
                        logger.LogDebug("设置系统状态");
                        SetWinInfo();
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

        private async void SetDebugModel()
        {
            // TODO: Implement debug mode configuration for API service
            logger.LogDebug("Debug mode enabled for API service");
            await Task.CompletedTask;
        }

        private async void SetWinInfo()
        {
            // TODO: Send configuration to WebView app through IPC or shared config
            logger.LogDebug("开始设置URL：{0}", url);
            logger.LogDebug("开始设置系统状态：{0}", state);
            logger.LogDebug("设置成功");
            await Task.CompletedTask;
        }

        private string GetUrl(string url)
        {
            var section = configuration.GetSection(nameof(IDMApi));
            Uri host = new Uri(section.GetValue<string>("HttpHost"));
            return (new Uri(host, url)).ToString();
        }
    }
}
