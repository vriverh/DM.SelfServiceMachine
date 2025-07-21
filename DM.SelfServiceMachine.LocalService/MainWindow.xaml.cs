using Microsoft.Extensions.Hosting;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string defaultUrl = "";

        private bool isDisabled = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 界面事件

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if(Browser.CanGoBack)
                Browser.GoBack();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (Browser.CanGoForward)
                Browser.GoForward();
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            Browser.Reload();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if (Browser != null && Browser.CoreWebView2 != null && !string.IsNullOrWhiteSpace(txtBoxAddress.Text))
            {
                if (!txtBoxAddress.Text.StartsWith("http") && !txtBoxAddress.Text.StartsWith("edge"))
                {
                    txtBoxAddress.Text = "http://" + txtBoxAddress.Text;
                }
                Browser.CoreWebView2.Navigate(txtBoxAddress.Text);
            }
        }

        private void txtBoxAddress_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Enter)
            {
                GoButton_Click(null, null);
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            TestWindow testWindow = new TestWindow();
            testWindow.Owner = this;
            testWindow.ShowDialog();
        }

        private DateTime exitDateTime = DateTime.Now;
        private int exitNumber;
        private TimeSpan outTimeSpan = TimeSpan.FromSeconds(5);

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateTime.Now - exitDateTime > outTimeSpan)
            {
                exitDateTime = DateTime.Now;
                exitNumber = 0;
            }
            exitNumber++;
            if (exitNumber == 10)
            {
                ExitWindow exitWindow = new ExitWindow();
                exitWindow.Owner = this;
                exitWindow.ShowDialog();
            }
        }

        private void Browser_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            Browser.CoreWebView2.Settings.IsZoomControlEnabled = false;
            Browser.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            Browser.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
            Browser.CoreWebView2.Settings.IsPinchZoomEnabled = false;

            Browser.CoreWebView2.ServerCertificateErrorDetected += (s, args) =>
            {
                args.Action = CoreWebView2ServerCertificateErrorAction.AlwaysAllow;
            };
            SetState(isDisabled);
        }
        #endregion

        public void SetDebugModel()
        {
            this.TitleGrid.Visibility = Visibility.Visible;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            SetState(false);
            SetUrl("http://127.0.0.1:9180/swagger/index.html");
            this.exitPopup.IsOpen = false;
            this.Topmost = false;
        }

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state">true是停用，false是正常</param>
        public void SetState(bool state)
        {
            this.isDisabled = state;
            if (state)
            {
                ErrorGrid.Visibility = Visibility.Visible;
                Browser.Visibility = Visibility.Hidden;
            }
            else
            {
                ErrorGrid.Visibility = Visibility.Hidden;
                Browser.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 设置url
        /// </summary>
        /// <param name="url"></param>
        public void SetUrl(string url)
        {
            if (defaultUrl != url)
            {
                defaultUrl = url;
                this.Browser.Source = new Uri(url);
            }
        }
        
    }
}
