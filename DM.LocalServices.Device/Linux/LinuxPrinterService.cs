using DM.LocalServices.Device.Abstractions;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace DM.LocalServices.Device.Linux
{
    /// <summary>
    /// Linux implementation of printer service
    /// </summary>
    public class LinuxPrinterService : IPrinterService
    {
        private readonly ILogger<LinuxPrinterService> _logger;

        public LinuxPrinterService(ILogger<LinuxPrinterService> logger)
        {
            _logger = logger;
        }

        public bool SendToPrinter(string printerName, string dataString)
        {
            try
            {
                // Use lp command on Linux
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "lp",
                        Arguments = $"-d {printerName}",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();
                process.StandardInput.Write(dataString);
                process.StandardInput.Close();
                process.WaitForExit();

                var success = process.ExitCode == 0;
                if (!success)
                {
                    var error = process.StandardError.ReadToEnd();
                    _logger.LogError("Failed to print to {PrinterName}: {Error}", printerName, error);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send string to printer {PrinterName}", printerName);
                return false;
            }
        }

        public bool SendBytesToPrinter(string printerName, byte[] bytes)
        {
            try
            {
                // Write bytes to temporary file and print
                var tempFile = Path.GetTempFileName();
                File.WriteAllBytes(tempFile, bytes);

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "lp",
                        Arguments = $"-d {printerName} {tempFile}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();
                process.WaitForExit();

                File.Delete(tempFile);

                var success = process.ExitCode == 0;
                if (!success)
                {
                    var error = process.StandardError.ReadToEnd();
                    _logger.LogError("Failed to print to {PrinterName}: {Error}", printerName, error);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send bytes to printer {PrinterName}", printerName);
                return false;
            }
        }
    }
}