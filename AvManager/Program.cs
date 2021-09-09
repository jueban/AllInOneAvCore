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

namespace AvManager
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            CreateApiHostBuilder(args).Build().RunAsync();
            CreateHangfireHostBuilder(args).Build().RunAsync();
            CreateSignalRHostBuilder(args).Build().RunAsync();
            CreateIdentityServerHostBuilder(args).Build().RunAsync();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
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
