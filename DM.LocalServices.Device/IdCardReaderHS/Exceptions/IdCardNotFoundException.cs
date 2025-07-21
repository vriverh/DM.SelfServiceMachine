using System;

namespace DM.LocalServices.Device.IdCardReaderHS
{
    public class IdCardNotFoundException : Exception
    {
        public IdCardNotFoundException()
            : base("未放卡或卡片放置不正确")
        {

        }
    }
}
