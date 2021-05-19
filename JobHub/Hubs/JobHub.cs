using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Utils;

namespace JobHub.Hubs
{
    public class JobHubs : Hub
    {
        public Task SendMessageToAll(string message)
        {
            LogHelper.Info("JobHUb -> " + message);
            return Clients.All.SendAsync("ReceivedMessage", message);
        }
    }
}
