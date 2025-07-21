using System.Runtime.InteropServices;
using System.Text;

namespace DM.LocalServices.Device
{
    public class ZKTecoId200SDK
    {

        /// <summary>
        /// 连接身份证阅读器 
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int InitComm(int port);

        /// <summary>
        /// 自动搜索身份证阅读器并连接身份证阅读器 
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int InitCommExt();

        /// <summary>
        /// 断开与身份证阅读器连接 
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int CloseComm();

        /// <summary>
        /// 判断是否有放卡，且是否身份证 
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int Authenticate();

        /// <summary>
        /// 读卡操作,信息文件存储在dll所在下
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int Read_Content(int index);

        /// <summary>
        /// 读卡操作,信息文件存储在dll所在下
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int ReadContent(int index);

        /// <summary>
        /// 获取SAM模块编号
        /// </summary>
        /// <param name="SAMID"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetSAMID(StringBuilder SAMID);

        /// <summary>
        /// 获取SAM模块编号（10位编号）
        /// </summary>
        /// <param name="SAMID"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetSAMIDEx(StringBuilder SAMID);

        /// <summary>
        /// 解析身份证照片
        /// </summary>
        /// <param name="PhotoPath"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetBmpPhoto(string PhotoPath);

        /// <summary>
        /// 解析身份证照片
        /// </summary>
        /// <param name="imageData"></param>
        /// <param name="cbImageData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetBmpPhotoToMem(byte[] imageData, int cbImageData);

        /// <summary>
        /// 解析身份证照片
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetBmpPhotoExt();

        /// <summary>
        /// 重置Sam模块
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int Reset_SAM();

        /// <summary>
        /// 获取SAM模块状态 
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetSAMStatus();

        /// <summary>
        /// 解析身份证信息 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetCardInfo(int index, StringBuilder value);

        /// <summary>
        /// 生成竖版身份证正反两面图片(输出目录：dll所在目录的cardv.jpg和SetCardJPGPathNameV指定路径)
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int ExportCardImageV();

        /// <summary>
        /// 生成横版身份证正反两面图片(输出目录：dll所在目录的cardh.jpg和SetCardJPGPathNameH指定路径) 
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int ExportCardImageH();

        /// <summary>
        /// 设置生成文件临时目录
        /// </summary>
        /// <param name="DirPath"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int SetTempDir(string DirPath);

        /// <summary>
        /// 获取文件生成临时目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cbPath"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetTempDir(StringBuilder path, int cbPath);

        /// <summary>
        /// 获取jpg头像全路径名 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cbPath"></param>
        [DllImport("termb.dll")]
        public static extern void GetPhotoJPGPathName(StringBuilder path, int cbPath);

        /// <summary>
        /// 设置jpg头像全路径名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int SetPhotoJPGPathName(string path);

        /// <summary>
        /// 设置竖版身份证正反两面图片全路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int SetCardJPGPathNameV(string path);

        /// <summary>
        /// 获取竖版身份证正反两面图片全路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cbPath"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetCardJPGPathNameV(StringBuilder path, int cbPath);

        /// <summary>
        /// 设置横版身份证正反两面图片全路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int SetCardJPGPathNameH(string path);

        /// <summary>
        /// 获取横版身份证正反两面图片全路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cbPath"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int GetCardJPGPathNameH(StringBuilder path, int cbPath);

        /// <summary>
        /// 获取姓名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getName(StringBuilder data, int cbData);

        /// <summary>
        /// 获取性别
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getSex(StringBuilder data, int cbData);

        /// <summary>
        /// 获取民族
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getNation(StringBuilder data, int cbData);

        /// <summary>
        /// 获取生日(YYYYMMDD)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getBirthdate(StringBuilder data, int cbData);

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getAddress(StringBuilder data, int cbData);

        /// <summary>
        /// 获取身份证号
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getIDNum(StringBuilder data, int cbData);

        /// <summary>
        /// 获取签发机关
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getIssue(StringBuilder data, int cbData);

        /// <summary>
        /// 获取有效期起始日期(YYYYMMDD)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getEffectedDate(StringBuilder data, int cbData);

        /// <summary>
        /// 获取有效期截止日期(YYYYMMDD) 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getExpiredDate(StringBuilder data, int cbData);

        /// <summary>
        /// 获取BMP头像Base64编码 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getBMPPhotoBase64(StringBuilder data, int cbData);

        /// <summary>
        /// 获取JPG头像Base64编码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getJPGPhotoBase64(StringBuilder data, int cbData);

        /// <summary>
        /// 获取竖版身份证正反两面JPG图像base64编码字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getJPGCardBase64V(StringBuilder data, int cbData);

        /// <summary>
        /// 获取横版身份证正反两面JPG图像base64编码字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbData"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getJPGCardBase64H(StringBuilder data, int cbData);

        /// <summary>
        /// 语音提示。。仅适用于与带HID语音设备的身份证阅读器（如ID200）
        /// </summary>
        /// <param name="nVoice"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int HIDVoice(int nVoice);

        /// <summary>
        /// 设置发卡器序列号
        /// </summary>
        /// <param name="iPort"></param>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int IC_SetDevNum(int iPort, StringBuilder data, int cbdata);

        /// <summary>
        /// 获取发卡器序列号
        /// </summary>
        /// <param name="iPort"></param>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int IC_GetDevNum(int iPort, StringBuilder data, int cbdata);

        /// <summary>
        /// 设置发卡器序列号 
        /// </summary>
        /// <param name="iPort"></param>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int IC_GetDevVersion(int iPort, StringBuilder data, int cbdata);

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="iPort"></param>
        /// <param name="keyMode"></param>
        /// <param name="sector"></param>
        /// <param name="idx"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <param name="snr"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int IC_WriteData(int iPort, int keyMode, int sector, int idx, StringBuilder key, StringBuilder data, int cbdata, ref uint snr);

        /// <summary>
        /// 读数据
        /// </summary>
        /// <param name="iPort"></param>
        /// <param name="keyMode"></param>
        /// <param name="sector"></param>
        /// <param name="idx"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <param name="snr"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int IC_ReadData(int iPort, int keyMode, int sector, int idx, StringBuilder key, StringBuilder data, int cbdata, ref uint snr);

        /// <summary>
        /// 读IC卡物理卡号 
        /// </summary>
        /// <param name="iPort"></param>
        /// <param name="snr"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int IC_GetICSnr(int iPort, ref uint snr);

        /// <summary>
        /// 读身份证物理卡号 
        /// </summary>
        /// <param name="iPort"></param>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int IC_GetIDSnr(int iPort, StringBuilder data, int cbdata);

        /// <summary>
        /// 获取英文名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getEnName(StringBuilder data, int cbdata);

        /// <summary>
        /// 获取中文名 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getCnName(StringBuilder data, int cbdata);

        /// <summary>
        /// 获取港澳台居通行证号码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cbdata"></param>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getPassNum(StringBuilder data, int cbdata);

        /// <summary>
        /// 获取签发次数
        /// </summary>
        /// <returns></returns>
        [DllImport("termb.dll")]
        public static extern int getVisaTimes();

        [DllImport("termb.dll")]
        public static extern int IC_ChangeSectorKey(int iPort, int keyMode, int nSector, StringBuilder oldKey, StringBuilder newKey);
    }
}
