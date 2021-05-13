using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Win32Helper
    {
        public static string GetExeLocation(string exeFile)
        {
            try
            {
                string app = exeFile;
                RegistryKey regKey = Registry.LocalMachine;
                RegistryKey regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + app, false);
                object objResult = regSubKey.GetValue(string.Empty);
                RegistryValueKind regValueKind = regSubKey.GetValueKind(string.Empty);
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                    return objResult.ToString();
                }

                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}
