using System;

namespace DM.LocalServices.Device.IdCardReaderHS
{
    public class IdCardReaderReadException : Exception
    {
        public IdCardReaderReadException()
            : base("身份证读卡失败")
        {

        }
    }
}
