using DM.LocalServices.Device;
using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Repository.LocalRepository;
using DM.LocalServices.Repository.VirtualRepository;
using DM.LocalServices.API.WebApi;
using Serilog;
using System.Text;

namespace DM.LocalServices.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}