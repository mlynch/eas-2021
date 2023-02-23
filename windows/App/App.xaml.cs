using System;
using System.Diagnostics;
using System.Windows;

namespace MyApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        const string UriScheme = "ionic.cs.appsummit";
        const string SingleInstanceKey = "MyAppSingleInstance";

        public App() {}

        protected override void OnStartup(StartupEventArgs e) {
            WpfSingleInstance.Make(SingleInstanceKey, e.Args);

            base.OnStartup(e);
        }

        public void HandleArgs(string args) {
            var parts = args.Split(" ");
            if (parts.Length <= 0 ) {
                return;
            }

            Application.Current.Dispatcher.Invoke(new Action(() => {
                if (Uri.TryCreate(parts[0], UriKind.Absolute, out var uri) && string.Equals(uri.Scheme, UriScheme, StringComparison.OrdinalIgnoreCase)) {
                    var window = Application.Current.MainWindow as MainWindow;
                    if (window != null) {
                        window.HandleRedirect(uri);
                    }
                }
            }));
        }

    }
}
