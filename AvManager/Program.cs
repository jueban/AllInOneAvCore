using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AvManager;
using Microsoft.Extensions.Hosting;
using AvManager.SignalR;
using AvManager.IdentityServer;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace AvManager
{
    static class Program
    {
        /// <summary>
        /// 是否退出应用程序
        /// </summary>
        static bool glExitApp = false;
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != current.Id)
                {
                    if (process.MainModule.FileName == current.MainModule.FileName)
                    {
                        ShowWindow(process.MainWindowHandle, 1);
                        return;
                    }
                }
            }

            var mode = "-WithNoServer";

            if (args.Length > 0)
            {
                if (args.Contains("-withserver"))
                {
                    CreateApiHostBuilder(args).Build().RunAsync();
                    CreateHangfireHostBuilder(args).Build().RunAsync();
                    CreateSignalRHostBuilder(args).Build().RunAsync();
                    CreateIdentityServerHostBuilder(args).Build().RunAsync();

                    mode = "-WithServer";
                }
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main(mode));
        }

        public static IHostBuilder CreateApiHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<ApiStartup>().UseUrls("http://*:21001");
                });

        public static IHostBuilder CreateHangfireHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<HangfireStartup>().UseUrls("http://*:21002");
                });

        public static IHostBuilder CreateSignalRHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<SignalRStartup>().UseUrls("http://*:21004");
                });

        public static IHostBuilder CreateIdentityServerHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<IdentityServerStartup>().UseUrls("http://*:21020");
                });
    }
}
