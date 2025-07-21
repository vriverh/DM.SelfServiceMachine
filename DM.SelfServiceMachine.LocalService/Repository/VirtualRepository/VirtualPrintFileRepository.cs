using DM.SelfServiceMachine.LocalService.Models;
using DM.SelfServiceMachine.LocalService.Repository.IRepository;
using DM.SelfServiceMachine.LocalService.Repository.LocalRepository;
using DM.SelfServiceMachine.LocalService.Tools;
using Microsoft.Extensions.Logging;
using Spire.Pdf;
using Spire.Pdf.General.Find;
using Spire.Pdf.Widget;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Vanara.PInvoke.Gdi32;
using static Vanara.PInvoke.WinSpool;

namespace DM.SelfServiceMachine.LocalService.Repository.VirtualRepository
{
    public class VirtualPrintFileRepository : IPrintFileRepository
    {

        public VirtualPrintFileRepository()
        {
        }

        public void Print(PrintFileInfo printFileInfo)
        {
            byte[] fileBytes;
            if (printFileInfo.TranType == 1)
            {
                fileBytes = Convert.FromBase64String(printFileInfo.FileContent);
            }
            else
            {
                fileBytes = File.ReadAllBytes(printFileInfo.FilePath);
            }

            //if (printFileInfo.FileExtend.Trim().ToLower() == "pdfd")
            //{
            //    fileBytes = PdfTool.RemoveSignature(fileBytes);
            //}

            File.WriteAllBytes("D:\\test-1.pdf", fileBytes);
        }

        
        public PrinterStatus GetPrinterStatus()
        {
            return new PrinterStatus() { Code = 0, Desc = "测试内容正常" };
        }
    }
}
