using System;

namespace DM.SelfServiceMachine.LocalService.Tools.IdCardReaderHS
{
    public class IdCardReaderInitException : Exception
    {
        public IdCardReaderInitException(string message)
            : base(message)
        {

        }
    }
}
