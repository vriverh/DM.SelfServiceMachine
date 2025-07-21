using DM.LocalServices.Device.Abstractions;
using DM.LocalServices.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.NetworkInformation;

namespace DM.LocalServices.Device.Linux
{
    /// <summary>
    /// Linux implementation of machine info service
    /// </summary>
    public class LinuxMachineInfoService : IMachineInfoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LinuxMachineInfoService> _logger;
        private string? _mac;
        private string? _ip;

        public LinuxMachineInfoService(IConfiguration configuration, ILogger<LinuxMachineInfoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public string GetMacAddress()
        {
            if (string.IsNullOrWhiteSpace(_mac))
            {
                _mac = _configuration["Mac"];
                if (string.IsNullOrWhiteSpace(_mac))
                {
                    try
                    {
                        var networkInterface = NetworkInterface.GetAllNetworkInterfaces()
                            .FirstOrDefault(ni => ni.OperationalStatus == OperationalStatus.Up 
                                && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback);
                        
                        if (networkInterface != null)
                        {
                            _mac = networkInterface.GetPhysicalAddress().ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get MAC address");
                        _mac = "Unknown";
                    }
                }
            }
            return _mac ?? "";
        }

        public string GetIpAddress()
        {
            if (string.IsNullOrWhiteSpace(_ip))
            {
                _ip = _configuration["Ip"];
                if (string.IsNullOrWhiteSpace(_ip))
                {
                    try
                    {
                        _ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                            .FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString() ?? "";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to get IP address");
                        _ip = "Unknown";
                    }
                }
            }
            return _ip ?? "";
        }

        public DevInfo GetDeviceInfo()
        {
            return new DevInfo
            {
                No = _configuration["No"] ?? "",
                Name = _configuration["Name"] ?? "",
                Code = _configuration["Code"] ?? "",
                Mac = GetMacAddress(),
                Ip = GetIpAddress(),
                Version = _configuration["Version"] ?? "1.0.0",
                State = _configuration.GetValue<int>("State"),
                DefaultUrl = _configuration["DefaultUrl"] ?? "",
                DeviceName = _configuration["DeviceName"] ?? "",
                QueueCenterURL = _configuration["QueueCenterURL"] ?? "",
                QueueWebURL = _configuration["QueueWebURL"] ?? ""
            };
        }
    }
}