using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class SettingService
    {
        //获取配置
        public async static Task<Settings> GetJavLibrarySetting()
        {
            Settings ret = new();
            string content = "";

            using (HttpClient client = new())
            {
                content = await client.GetStringAsync($"http://localhost:20001/api/config/GetConfig");
            }

            return JsonSerializer.Deserialize<Settings>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
