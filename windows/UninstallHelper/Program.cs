using System;
using System.Diagnostics;
using System.IO;

namespace UninstallHelper
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the base directory where the executable is located.
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Construct the full path to the batch script.
            string scriptName = "uninstall.bat";
            string scriptPath = Path.Combine(baseDirectory, scriptName);

            // Start the batch script.
            ProcessStartInfo psi = new ProcessStartInfo(scriptPath)
            {
                RedirectStandardOutput = false,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            Process process = new Process { StartInfo = psi };
            process.Start();
            process.WaitForExit();
        }
    }
}
