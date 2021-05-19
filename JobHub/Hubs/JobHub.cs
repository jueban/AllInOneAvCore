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
            progress.ProgressChanged += ReportProgress;

            await LocalService.RemoveFolder(folder, progress);

            return "success";
        }

        private void ReportProgress(object sender, string e)
        {
            Clients.Caller.SendAsync("RemoveFolder", e);
        }
    }
}
