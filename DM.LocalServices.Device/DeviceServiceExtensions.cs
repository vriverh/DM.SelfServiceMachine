using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace DM.LocalServices.Device
{
    /// <summary>
    /// Extension methods for device service registration
    /// </summary>
    public static class DeviceServiceExtensions
    {
        /// <summary>
        /// Register device services with dependency injection
        /// </summary>
        public static IServiceCollection AddDeviceServices(this IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddSingleton<IMachineInfoService, Windows.WindowsMachineInfoService>();
                services.AddSingleton<IPrinterService, Windows.WindowsPrinterService>();
                services.AddSingleton<IIdCardReaderService, Windows.WindowsIdCardReaderService>();
                services.AddSingleton<IFaceCameraService, Windows.WindowsFaceCameraService>();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                services.AddSingleton<IMachineInfoService, Linux.LinuxMachineInfoService>();
                services.AddSingleton<IPrinterService, Linux.LinuxPrinterService>();
                services.AddSingleton<IIdCardReaderService, Linux.LinuxIdCardReaderService>();
                services.AddSingleton<IFaceCameraService, Linux.LinuxFaceCameraService>();
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported platform");
            }

            return services;
        }
    }
}