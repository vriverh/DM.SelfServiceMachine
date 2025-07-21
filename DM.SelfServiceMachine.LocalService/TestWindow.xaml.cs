using DM.SelfServiceMachine.LocalService.Repository.LocalRepository;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DM.SelfServiceMachine.LocalService
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        private FacecameraRepository facecameraRepository;
        private ReadRepositoryHS readRepositoryHS;

        public TestWindow()
        {
            InitializeComponent();
            facecameraRepository = new FacecameraRepository(new NullLogger<FacecameraRepository>());
            readRepositoryHS = new ReadRepositoryHS(new NullLogger<ReadRepositoryHS>());
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddLog("开始打开摄像头");
                facecameraRepository.Open(410,  110,  640,  480 );
                AddLog("打开摄像头成功");
            }
            catch (Exception ex)
            {
                AddLog("打开摄像头，报错：" + ex.Message);
            }
        }

        private void getCurrentImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddLog("开始获取当前图片");
                facecameraRepository.GetCurrentImage();
                AddLog("获取当前图片成功");
            }
            catch (Exception ex)
            {
                AddLog("获取当前图片，报错：" + ex.Message);
            }

        }

        private void getFaceImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddLog("开始获取人脸");
                string ss= facecameraRepository.GetFaceImage();
                AddLog("获取人脸成功");
            }
            catch (Exception ex)
            {
                AddLog("获取人脸，报错：" + ex.Message);
            }

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddLog("开始关闭摄像头");
                facecameraRepository.Close();
                AddLog("关闭摄像头成功");
            }
            catch (Exception ex)
            {
                AddLog("关闭摄像头，报错：" + ex.Message);
            }
        }

        private void compFace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddLog("开始人脸对比");
                string faceImage1 = Convert.ToBase64String(File.ReadAllBytes(@"C:\Users\vrive\Downloads\a.bmp"));
                string faceImage2 = Convert.ToBase64String(File.ReadAllBytes(@"C:\Users\vrive\Downloads\b.bmp"));
                int cf = facecameraRepository.CompFace( faceImage1,  faceImage2 );

                AddLog("人脸对比成功，对比值：" + cf);
            }
            catch (Exception ex)
            {
                AddLog("人脸对比报错：" + ex.Message);
            }
        }

        private void AddLog(string log)
        {
            this.tblog.Text = this.tblog.Text.Insert(0, "\r\n");
            this.tblog.Text = this.tblog.Text.Insert(0, log);
        }

        private void readIDCard_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
