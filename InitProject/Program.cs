using DAL;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;
using Models;
using Services;
using System;
using System.Diagnostics;
using System.IO;
using Utils;

namespace InitProject
{
    class Program
    {
        static void Main(string[] args)
        {
            InitSettings();
            InitScheduleTask();

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
                    ChromeLocation = Win32Helper.GetExeLocation("Chrome.exe")
                },
                JavLibrarySettings = new JavLibrarySettings()
                {
                    CookieMode = JavLibraryGetCookieMode.Easy
                },
                JavLibraryImageFolder = "E:\\JavLibraryAvPic\\",
                JavBusImageFolder = "E:\\JavBusAvPic\\",
                AvatorImageFolder = "E:\\AvatorPic\\"
            };

            if (!Directory.Exists(settings.JavLibraryImageFolder))
            {
                Console.WriteLine("创建JavLibrary封面文件夹");
                Directory.CreateDirectory(settings.JavLibraryImageFolder);
            }

            if (!Directory.Exists(settings.JavBusImageFolder))
            {
                Console.WriteLine("创建JavBus封面文件夹");
                Directory.CreateDirectory(settings.JavBusImageFolder);
            }

            if (!Directory.Exists(settings.AvatorImageFolder))
            {
                Console.WriteLine("创建女优封面文件夹");
                Directory.CreateDirectory(settings.AvatorImageFolder);
            }

            new SettingsDAL().InitSetting(JsonHelper.SerializeWithUtf8(settings)).Wait();
        }

        static void InitScheduleTask()
        {
            Console.WriteLine($"初始化定时任务");

            InitScheduleOpenBroswerTask();
        }

        static void InitScheduleOpenBroswerTask()
        {
            Console.WriteLine("创建用浏览器获取JavLibrary Cookie的定时任务");
            ScheduleService.CreateOneTimeScheduler("OpenJavLibraryToGetCookie", "Open JavLibrary To Get Cookie", Win32Helper.GetExeLocation("Chrome.exe"), "http://www.javlibrary.com/cn/");
        }
    }
}
