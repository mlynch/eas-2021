using System;
using System.Threading;
using System.Windows;
using System.IO;
using System.IO.Pipes;
using Capacitor;

namespace MyApp
{
    public static class WpfSingleInstance
    {
        private static bool AlreadyProcessedOnThisInstance;
        private const string PipeName = "MyAppPipe";

        internal static void Make(string appName, String[] args, bool uniquePerUser = true)
        {
            if(AlreadyProcessedOnThisInstance)
            {
                return;
            }
            AlreadyProcessedOnThisInstance = true;

            Application app = Application.Current;

            string eventName = uniquePerUser
                ? $"{appName}-{Environment.MachineName}-{Environment.UserDomainName}-{Environment.UserName}"
                : $"{appName}-{Environment.MachineName}";

            bool isSecondaryInstance = true;

            EventWaitHandle eventWaitHandle = null;
            try
            {
                eventWaitHandle = EventWaitHandle.OpenExisting(eventName);
            }
            catch
            {
                // This code only runs on the first instance.
                isSecondaryInstance = false;
            }

            if (isSecondaryInstance)
            {
                ActivateFirstInstanceWindow(eventWaitHandle);

                IPCClientSendArgs(args);

                // Let's produce a non-interceptable exit (2009 year approach).
                Environment.Exit(0);
            }

            StartIPCServer();

            RegisterFirstInstanceWindowActivation(app, eventName);
        }

        private static void ActivateFirstInstanceWindow(EventWaitHandle eventWaitHandle)
        {
            // Let's notify the first instance to activate its main window.
            _ = eventWaitHandle.Set();
        }

        private static void RegisterFirstInstanceWindowActivation(Application app, string eventName)
        {
            EventWaitHandle eventWaitHandle = new EventWaitHandle(
                false,
                EventResetMode.AutoReset,
                eventName);

            _ = ThreadPool.RegisterWaitForSingleObject(eventWaitHandle, WaitOrTimerCallback, app, Timeout.Infinite, false);

            eventWaitHandle.Close();
        }

        private static void WaitOrTimerCallback(object state, bool timedOut)
        {
            Application app = (Application)state;
            _ = app.Dispatcher.BeginInvoke(new Action(() =>
            {
                _ = Application.Current.MainWindow.Activate();
            }));
        }

        private static void IPCClientSendArgs(String[] args) {
            using (NamedPipeClientStream pipeClient =
                new NamedPipeClientStream(".", PipeName, PipeDirection.Out)) {

                try {
                    pipeClient.Connect(20000);
                    using (StreamWriter wr = new StreamWriter(pipeClient)) {
                        wr.WriteLine(String.Join(" ", args));
                    }
                    pipeClient.Flush();
                    pipeClient.Close();
                    pipeClient.Dispose();
                } catch (Exception e) {
                    Console.WriteLine("ERROR: {0}", e.Message);
                }
            }
        }

        private static void StartIPCServer() {
            var threadRef = new ThreadStart(() => {
                while (true) {
                    using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.In)) {
                        // Wait for a client to connect
                        pipeServer.WaitForConnection();

                        try {
                            using (StreamReader re = new StreamReader(pipeServer)) {
                                string args = re.ReadToEnd();
                                (App.Current as App).HandleArgs(args);
                            }

                        }
                        // Catch the IOException that is raised if the pipe is broken
                        // or disconnected.
                        catch (Exception e) {
                            Console.WriteLine("ERROR: {0}", e.Message);
                        }
                    }
                }
            });
            var thread = new Thread(threadRef);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
