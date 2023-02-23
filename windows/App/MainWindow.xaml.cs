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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Capacitor;

namespace MyApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CapacitorApp CapacitorAppInstance { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763, 0))
            {
                //CapacitorAppInstance = new CapacitorApp(this, this.CapacitorWebView);
                CapacitorAppInstance = new CapacitorApp(this, this.CapacitorWebView);
                CapacitorAppInstance.LoadDefaultPlugins();
                CapacitorAppInstance.Load();
            }
        }

        public void HandleRedirect(Uri uri) {
            MessageBox.Show($"Handling redirect: {uri.ToString()}");
            var js = $@"
try {{
    var u = new URL('{uri.ToString()}');
    if (u.searchParams.get('code')) {{
        window.IonicAuth.handleLoginCallback('{uri.ToString()}');
    }} else {{
        window.IonicAuth.handleLogoutCallback();
    }}
}} catch (e) {{
    console.error('Error authenticating', e);
}}
            ";

            this.CapacitorWebView.Dispatcher.Invoke(() => {
                _ = this.CapacitorWebView.ExecuteScriptAsync(js);
            });
        }
    }
}
