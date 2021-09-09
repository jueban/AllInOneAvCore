using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NoticeService
    {
        public async static void SendBarkNotice(string content)
        {
            var setting = await SettingService.GetSetting();
            using (HttpClient client = new())
            {
                await client.GetAsync($"{setting.BarkSite}/{setting.BarkId}/{content}");
            }
        }
    }
}
