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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleChatWpfClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            webView2.CoreWebView2InitializationCompleted += WebView2_CoreWebView2InitializationCompleted;
        }

        private async void webView2_Loaded(object sender, RoutedEventArgs e)
        {
            await webView2.EnsureCoreWebView2Async();

        }

        private void WebView2_CoreWebView2InitializationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            webView2.CoreWebView2.SetVirtualHostNameToFolderMapping("localhost.app", "wwwroot", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            //var receiver = webView2.CoreWebView2.GetDevToolsProtocolEventReceiver();

            webView2.Source = new Uri("http://localhost.app/index.html");
        }
    }
}
