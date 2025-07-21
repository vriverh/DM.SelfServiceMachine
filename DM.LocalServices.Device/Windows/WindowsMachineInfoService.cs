using DM.LocalServices.Device.Abstractions;
using DM.LocalServices.Models;
using Microsoft.Extensions.Configuration;
using System.Management;
using System.Net;

namespace DM.LocalServices.Device.Windows
{
    /// <summary>
    /// Windows implementation of machine info service
    /// </summary>
    public class WindowsMachineInfoService : IMachineInfoService
    {
        private readonly IConfiguration _configuration;
        private string? _mac;
        private string? _ip;

        public WindowsMachineInfoService(IConfiguration configuration)
        {
            _configuration = configuration;
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
                        ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            if (mo["IPEnabled"]?.ToString() == "True")
                            {
                                _mac = mo["MacAddress"]?.ToString() ?? "";
                                break;
                            }
                        }
                    }
                    catch
                    {
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
                    catch
                    {
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