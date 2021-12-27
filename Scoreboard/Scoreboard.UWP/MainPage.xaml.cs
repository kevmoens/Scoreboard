using Microsoft.AspNetCore.SignalR.Client;
using Scoreboard.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Scoreboard.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //await ((MainViewModel)DataContext).Initialize("Endpoint=https://scoreboard.service.signalr.net;AccessKey=1wnnrH4YcO+CxyPqAEdkQ5wz0q2HetjIADpB8yYdRR8=;Version=1.0;", "https://scoreboard.service.signalr.net/hubs");
            HttpServer server = new HttpServer(1265);
            server.ViewModel = (MainViewModel)DataContext;
        }

    }
}
