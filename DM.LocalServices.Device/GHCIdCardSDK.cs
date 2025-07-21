using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DM.LocalServices.Device
{
    public class IdCardDev
    {
        public const string Position = "IdCardDev";
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 扩展盒
        /// </summary>
        public string Extport { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public int Baud { get; set; }
    }

    public class GHCIdCardSDK
    {
        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_Dev_Connect(int Port, byte Extport, int Baud);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_Dev_Disconnect();

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GetModeID(ref int nIDLen, byte[] sIDData);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Read_SerialNum(ref int SerialNum_Len, byte[] SerialNum, int timeout_ms);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_ReadCard(int timeout_ms);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_ReadCard_Fp(int timeout_ms);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GetCardInfo(int index, byte[] infodata);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Type(byte[] type);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Name(byte[] cName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Sex(byte[] cSex);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Nation(byte[] cNation);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Birthday(byte[] cBirthday);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Address(byte[] cAddress);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_IDNumber(byte[] cIDNumber);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_IssueDepartment(byte[] cIssueDepartment);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_ValidFromDate(byte[] cValidFromDate);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_ValidExpiryDate(byte[] cValidExpiryDate);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_Reserve(byte[] cReserve);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnName(byte[] EnName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnSex(byte[] EnSex);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnNum(byte[] EnNum);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnState(byte[] EnState);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnCHName(byte[] EnCHName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnStarDate(byte[] EnStarDate);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnEndDate(byte[] EnEndDate);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnBirthday(byte[] EnBirthday);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnCardVer(byte[] EnCardVer);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnGovCode(byte[] EnGovCode);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_EnReserve(byte[] EnReserve);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaName(byte[] GaName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaSex(byte[] GaSex);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaBirthday(byte[] GaBirthday);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaAddress(byte[] GaAddress);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaCardID(byte[] GaCardID);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaGov(byte[] GaGov);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaStartDate(byte[] GaStartDate);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaEndData(byte[] GaEndData);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaPassCardID(byte[] GaPassCardID);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_GaSignTimes(byte[] GaSignTimes);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_FpInfo(byte[] FpInfo, ref int FpInfoLen);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_CreateHeadPhoto(int phototype, byte[] HeadPhotoName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_CreateFrontPhoto(int phototype, byte[] FrontPhotoName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_CreateBackPhoto(int phototype, byte[] BackPhotoName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_CreateBothPhoto(int phototype, byte[] FrontPhotoName, byte[] BackPhotoName);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_IDCard_ReadNewAppMsg(int timeout_ms, byte[] pucAppMsg, ref int puiAppMsgLen);

        [DllImport("GHC_GetIDCardInfo.dll")]
        public static extern int GHC_GetFileBase64Buffer(byte[] filename, byte[] base64data, int base64dataLen, ref int outbase64dataLen);

    }
}
