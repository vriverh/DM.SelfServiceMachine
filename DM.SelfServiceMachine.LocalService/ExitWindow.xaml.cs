using DM.SelfServiceMachine.LocalService.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// ExitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ExitWindow : Window
    {
        public ExitWindow()
        {
            InitializeComponent();
            this.tbIp.Text = MachineInfo.Ip;
            this.tbMac.Text = MachineInfo.Mac;
            this.tbV.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonContent = button.Content.ToString();
            switch (buttonContent)
            {
                case "确定":
                    if(exitPB.Password== "223824")
                    {
                        Process[] processCheck = Process.GetProcessesByName("ProcessCheck");
                        if (processCheck.Length != 0)
                        {
                            foreach (var item in processCheck)
                            {
                                item.Kill();
                            }
                        }

                        App.Current.Shutdown();
                    }
                    exitPB.Password = "";
                    return;
                case "取消":
                    this.Close();
                    return;
                default:
                    exitPB.Password = exitPB.Password + buttonContent;
                    break;
            }
        }
    }
}
