using Microsoft.AspNetCore.SignalR;
using Services;
using System;
using System.Threading.Tasks;
using Utils;

namespace JobHub.Hubs
{
    public class JobHubs : Hub
    {
        public async Task<string> RemoveFolder(string folder)
        {
            LogHelper.Info("去文件夹");
            Progress<string> progress = new();
            progress.ProgressChanged += ReportRemoveFolderProgress;

            await LocalService.RemoveFolder(folder, progress);

            return "success";
        }

        private void ReportRemoveFolderProgress(object sender, string e)
        {
            Clients.Caller.SendAsync("RemoveFolder", e);
        }

        public async Task<string> Rename(string folder)
        {
            Progress<string> progress = new();
            progress.ProgressChanged += ReportRenameProgress;

            await LocalService.Rename(folder, progress);

            return "success";
        }

        private void ReportRenameProgress(object sender, string e)
        {
            Clients.Caller.SendAsync("Rename", e);
        }

        public async Task<string> ScanJavLibrary(string str)
        {
            try
            {
                await Clients.Caller.SendAsync($"接收到参数 {str}");

                ScanParam param = JsonHelper.Deserialize<ScanParam>(str);
                Progress<string> progress = new();
                progress.ProgressChanged += ReportScanProgress;

                await MagnetUrlService.SearchJavLibrary(param.Url, param.Page, param.Name, progress);
            }
            catch (Exception ee)
            {
                await Clients.Caller.SendAsync($"异常 {ee.ToString()}");
            }

            return "success";
        }

        public async Task<string> ScanJavBus(string url, string name, int page)
        {
            Progress<string> progress = new();
            progress.ProgressChanged += ReportScanProgress;

            await MagnetUrlService.SearchJavBus(url, page, name, progress);

            return "success";
        }

        private void ReportScanProgress(object sender, string e)
        {
            Clients.Caller.SendAsync("ScanResult", e);
        }
    }

    public class ScanParam
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public int Page { get; set; }
    }
}
