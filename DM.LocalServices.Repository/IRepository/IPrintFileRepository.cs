using DM.LocalServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DM.LocalServices.Repository.IRepository
{
    public interface IPrintFileRepository
    {
        void Print(PrintFileInfo printFileInfo);

        PrinterStatus GetPrinterStatus();
    }
}
