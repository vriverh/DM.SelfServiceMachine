using System;

namespace DM.LocalServices.Device.IdCardReaderHS
{
    public class IdCardReaderInitException : Exception
    {
        public IdCardReaderInitException(string message)
            : base(message)
        {

        }
    }
}
