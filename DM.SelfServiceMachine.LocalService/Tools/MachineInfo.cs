using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Threading.Tasks;

namespace DM.SelfServiceMachine.LocalService.Tools
{
    public static class MachineInfo
    {
        /// <summary>
        /// 配置参数
        /// </summary>
        public static IConfiguration configuration { get; set; }

        private static string mac;

        public static string Mac
        {
            get
            {
                if (string.IsNullOrWhiteSpace(mac))
                {
                    mac = configuration.GetValue<string>("Mac");
                    if (string.IsNullOrWhiteSpace(mac))
                    {
                        ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                        ManagementObjectCollection moc = mc.GetInstances();
                        foreach (ManagementObject mo in moc)
                        {
                            if (mo["IPEnabled"].ToString() == "True")
                            {
                                mac = mo["MacAddress"].ToString();
                            }
                        }
                    }
                }
                return mac;
            }
        }

        private static string ip;

        public static string Ip
        {
            get 
            { 
                if(string.IsNullOrWhiteSpace(ip))
                {
                    ip = configuration.GetValue<string>("Ip");
                    if (string.IsNullOrWhiteSpace(ip))
                    {
                        IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                        foreach (IPAddress iPAddress in addressList)
                        {
                            if (!iPAddress.ToString().Contains("::"))
                            {
                                ip = iPAddress.ToString();
                            }
                        }
                    }
                }
                return ip; 
            }
        }
    }
}
