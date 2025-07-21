using System.Runtime.InteropServices;

namespace DM.LocalServices.Device.IdCardReaderHS
{

    [StructLayout(LayoutKind.Sequential, Size = 16, CharSet = CharSet.Ansi)]
    public struct IdCardStructure
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public char name;     //姓名
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public char sex;      //性别
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public char people;    //民族，护照识别时此项为空
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public char birthday;   //出生日期
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 70)]
        public char address;  //地址，在识别护照时导出的是国籍简码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public char number;  //地址，在识别护照时导出的是国籍简码
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public char signdate;   //签发日期，在识别护照时导出的是有效期至 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public char validtermOfStart;  //有效起始日期，在识别护照时为空
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public char validtermOfEnd;  //有效截止日期，在识别护照时为空
    }
}
