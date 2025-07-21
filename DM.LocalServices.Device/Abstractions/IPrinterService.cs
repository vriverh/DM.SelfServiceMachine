namespace DM.LocalServices.Device.Abstractions
{
    /// <summary>
    /// Printer service abstraction
    /// </summary>
    public interface IPrinterService
    {
        /// <summary>
        /// Send raw data to printer
        /// </summary>
        bool SendToPrinter(string printerName, string dataString);

        /// <summary>
        /// Send bytes to printer
        /// </summary>
        bool SendBytesToPrinter(string printerName, byte[] bytes);
    }
}