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
    }
}
