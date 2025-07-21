using DM.LocalServices.Repository.IRepository;
using DM.LocalServices.Repository.LocalRepository;
using DM.LocalServices.Repository.VirtualRepository;
using DM.LocalServices.API.WebApi;
using DM.LocalServices.Device;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace DM.LocalServices.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    //builder.WithOrigins("http://www.szreorc.com:8090").AllowAnyMethod().AllowAnyHeader().AllowCredentials();

                    //Uri host = new Uri(Configuration.GetValue<string>("IDMApi:HttpHost"));
                    //string url = host.AbsoluteUri.TrimEnd('/');
                    //builder.WithOrigins(url).AllowAnyMethod().AllowAnyHeader();
                });
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DM.SelfServiceMachine.LocalService", Version = "v1" });
            });
            services.AddHttpApi<IDMApi>().ConfigureHttpApi(Configuration.GetSection(nameof(IDMApi)));

            RegisterDI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DM.SelfServiceMachine.LocalService v1"));
            }

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void RegisterDI(IServiceCollection services)
        {
            // Register device services
            services.AddDeviceServices();

            // Register repository services
            services.AddSingleton<IDevInfoRepository, DevInfoRepository>();

            if (Configuration.GetValue<bool>("VirtualData"))
            {
                services.AddSingleton<IReadRepository, VirtualReadRepository>();
                services.AddSingleton<IPrintFileRepository, VirtualPrintFileRepository>();
                services.AddSingleton<IFacecameraRepository, VirtualFacecameraRepository>();
            }
            else
            {
                services.AddSingleton<IReadRepository, ReadRepository>();
                services.AddSingleton<IPrintFileRepository, PrintFileRepository>();
                services.AddSingleton<IFacecameraRepository, FacecameraRepository>();
            }
            
            // Add hosted services (commented out due to WPF dependencies)
            // services.AddHostedService<ClientRegistService>();
            // services.AddHostedService<AutoUpdaterService>();
        }
    }
}
