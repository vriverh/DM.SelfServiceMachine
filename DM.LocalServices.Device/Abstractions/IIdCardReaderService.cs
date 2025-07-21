using DM.LocalServices.Models;

namespace DM.LocalServices.Device.Abstractions
{
    /// <summary>
    /// ID card reader service abstraction
    /// </summary>
    public interface IIdCardReaderService
    {
        /// <summary>
        /// Read ID card information
        /// </summary>
        ReadInfo ReadCard();

        /// <summary>
        /// Initialize the reader
        /// </summary>
        bool Initialize();

        /// <summary>
        /// Close the reader
        /// </summary>
        void Close();
    }
}