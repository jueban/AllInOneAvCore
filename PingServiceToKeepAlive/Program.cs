using System;
using System.Linq;
using System.Net.Http;

namespace PingServiceToKeepAlive
{
    class Program
    {
        static void Main(string[] args)
        {
            var sites = args[0].Split(',').ToList();

            foreach (var site in sites)
            {
                using (HttpClient hc = new HttpClient())
                {
                    var result = hc.GetStringAsync(site + "/ping/ping").Result;
                }
            }
        }
    }
}
