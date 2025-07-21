using DM.LocalServices.Models;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Device.Abstractions;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace DM.LocalServices.API.WebApi
{
    public class DMFilterAttribute : ApiFilterAttribute
    {
        public DMFilterAttribute()
        {
        }

        public override Task OnRequestAsync(ApiRequestContext context)
        {
            string timestamp = Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds).ToString();
            context.HttpContext.RequestMessage.Headers.Add("x-timestamp", timestamp);

            // Get machine info service from DI container
            var machineInfoService = context.HttpContext.ServiceProvider.GetService<IMachineInfoService>();
            string mac = machineInfoService?.GetMacAddress() ?? "Unknown";
            context.HttpContext.RequestMessage.Headers.Add("x-mac", mac);

            string dataStr = timestamp + mac + timestamp;
            byte[] databy = Encoding.UTF8.GetBytes(dataStr);
            byte[] sha256by = SHA256.Create().ComputeHash(databy);
            string sha256Str = BitConverter.ToString(sha256by).Replace("-", "").ToUpper();
            context.HttpContext.RequestMessage.Headers.Add("x-code", sha256Str);

            return Task.CompletedTask;
        }

        public override Task OnResponseAsync(ApiResponseContext context)
        {
            return Task.CompletedTask;
        }
    }
}
