using System;

namespace MyApp {
    internal class Program {

        [STAThread]
        static void Main(string[] args)
        {
            App application = new App();
            application.InitializeComponent();
            application.Run();
        }
    }
}
