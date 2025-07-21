using DM.LocalServices.Models;

namespace DM.LocalServices.Device.Abstractions
{
    /// <summary>
    /// Machine information service abstraction
    /// </summary>
    public interface IMachineInfoService
    {
        /// <summary>
        /// Get MAC address
        /// </summary>
        string GetMacAddress();

        /// <summary>
        /// Get IP address
        /// </summary>
        string GetIpAddress();

        /// <summary>
        /// Get device information
        /// </summary>
        DevInfo GetDeviceInfo();
    }
}