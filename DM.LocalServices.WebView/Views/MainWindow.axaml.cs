using Avalonia.Controls;
using Avalonia.ReactiveUI;
using DM.LocalServices.WebView.ViewModels;

namespace DM.LocalServices.WebView.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}