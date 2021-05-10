using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire
{
    public class Jobs
    {
        public static void OpenBroswerJob(string location, string url)
        {
            var process = System.Diagnostics.Process.Start(location, url);
        }

        //没想好怎么搞processId
        public static void CloseBroswerJob(int processId)
        {
            var process = System.Diagnostics.Process.GetProcessById(processId);

            if (process != null && process.HasExited != true)
            {
                process.Kill();
                process.Dispose();
            }
        }
    }
}
