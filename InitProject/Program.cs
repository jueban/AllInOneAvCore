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
                AvatorImageFolder = "E:\\AvatorPic\\",
                AvNameFilter = "国产大片,苍老师强力推荐,有趣的小视频,美女荷官,台湾uu祼聊室,社区最新情报,精彩直播,澳门威尼斯人,澳门银河赌场,在精彩表演,AV在线观看,真人线上百家乐,有趣台妹小视频,裸聊直播,有趣的台湾妹妹直播,美女直播,美女教你搏一搏，单车变摩托,可以指揮表演,美女裸聊,激情裸聊视频,奔驰宝马娱乐城,免费手机看片,乐播传媒,博彩场一,注册免费送,牛逼,超高清手機影城APP快來下載喔,線上影片每天火熱更新中,辣妹裸聊,免费试看",
                ExcludeFolder = "FIN,TEMPFIN,MovieFiles"
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
