using DAL;
using Models;
using System;
using Utils;

namespace InitProject
{
    class Program
    {
        static void Main(string[] args)
        {
            InitSettings();

            Console.WriteLine("按任意键退出");
            Console.ReadKey();
        }

        static void InitSettings()
        {
            Console.WriteLine($"初始化配置");

            Settings settings = new Settings()
            {
                BarkId = "4z4uANLXpe8BXT3wAZVe9F",
                CommonSettings = new CommonSettings()
                {
                    ChromeLocation = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe"
                },
                JavLibrarySettings = new JavLibrarySettings()
                {
                    CookieMode = JavLibraryGetCookieMode.Easy
                },
                ImageFolder = "E:\\JavLibraryAvPic\\"
            };

            new SettingsDAL().InitSetting(JsonHelper.SerializeWithUtf8(settings)).Wait();
        }
    }
}
