using Microsoft.AspNetCore.SignalR;
using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Threading.Tasks;
using Utils;

namespace JobHub.Hubs
{
    public class JobHubs : Hub
    {
        private static readonly bool IsDebug = false;

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
                Progress<string> progress = new();
                progress.ProgressChanged += ReportScanProgress;

                NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始扫描JavLibrary");

                var startTime = DateTime.Now;

                str = RedisService.GetHash("scan", str);

                ScanParam param = JsonConvert.DeserializeObject<ScanParam>(str);

                await MagnetUrlService.SearchJavLibrary(param.Url, param.Page, param.Name, param.Order, progress);

                NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"扫描JavLibrary完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
            }
            catch (Exception ee)
            {
                LogHelper.Info(ee.ToString());
                await Clients.Caller.SendAsync($"异常 {ee}");
            }
            finally
            {
                RedisService.DeleteHash("scan", str);
            }

            return "success";
        }

        public async Task<string> ScanJavBus(string str)
        {
            try
            {
                NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"开始扫描JavBus");

                var startTime = DateTime.Now;

                str = RedisService.GetHash("scan", str);

                ScanParam param = JsonHelper.Deserialize<ScanParam>(str);
                Progress<string> progress = new();
                progress.ProgressChanged += ReportScanProgress;

                await MagnetUrlService.SearchJavBus(param.Url, param.Page, param.Name, progress);

                NoticeService.SendBarkNotice(SettingService.GetSetting().Result.BarkId, $"扫描JavBus完成，耗时 {(DateTime.Now - startTime).TotalSeconds} 秒");
            }
            catch (Exception ee)
            {
                await Clients.Caller.SendAsync($"异常 {ee}");
            }
            finally
            {
                RedisService.DeleteHash("scan", str);
            }

            return "success";
        }

        private void ReportScanProgress(object sender, string e)
        {
            if (!IsDebug)
            {
                Clients.Caller.SendAsync("ScanResult", e);
            }
            else
            {
                Console.WriteLine(e);
            }
        }
    }
}
