using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace DM.LocalServices.WebView.Views;

public partial class MainWindow : Window
{
    public string CurrentUrl { get; set; } = "http://www.baidu.com";

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        
        // Initialize WebView here when it becomes available
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // TODO: Initialize WebView component when Avalonia.WebView becomes available
        // For now, we'll show a placeholder
        await Task.Delay(1000); // Simulate loading
        
        // Hide loading text
        if (WebViewContainer.Children.Count > 0)
        {
            WebViewContainer.Children.Clear();
        }
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement WebView back navigation
    }

    private void ForwardButton_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement WebView forward navigation
    }

    private void ReloadButton_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement WebView reload
    }

    private void GoButton_Click(object? sender, RoutedEventArgs e)
    {
        if (AddressTextBox?.Text != null)
        {
            CurrentUrl = AddressTextBox.Text;
            // TODO: Navigate WebView to the new URL
        }
    }

    private void AddressTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            GoButton_Click(sender, new RoutedEventArgs());
        }
    }

    private void ExitButton_Click(object? sender, RoutedEventArgs e)
    {
        // Show exit confirmation or just close
        Close();
    }
}