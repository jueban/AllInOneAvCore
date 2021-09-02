using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class SettingService
    {
        //从WebAPI获取配置
        public async static Task<Settings> GetSetting()
        {
            var content = await new SettingsDAL().GetAllSettings();

            return content;
        }

        public async static Task SaveSetting(Settings settings, IProgress<string> progress)
        {
            progress.Report($"初始化配置");

            if (!Directory.Exists(settings.JavLibraryImageFolder))
            {
                progress.Report("创建JavLibrary封面文件夹");
                Directory.CreateDirectory(settings.JavLibraryImageFolder);
            }

            if (!Directory.Exists(settings.JavBusImageFolder))
            {
                progress.Report("创建JavBus封面文件夹");
                Directory.CreateDirectory(settings.JavBusImageFolder);
            }

            if (!Directory.Exists(settings.AvatorImageFolder))
            {
                progress.Report("创建女优封面文件夹");
                Directory.CreateDirectory(settings.AvatorImageFolder);
            }

            var settringDAL = new SettingsDAL();
            settringDAL.InitSetting(JsonHelper.SerializeWithUtf8(settings)).Wait();

            settringDAL.TruncatePrefix().Wait();
            settringDAL.InsertPrefix(settings.Prefix).Wait();
        }

        public static int InsertPlayHistory(PlayHistory entity)
        {
            return new SettingsDAL().InsertPlayHistory(entity);
        }

        public static PlayHistory GetPlayHistory(string fileName)
        {
            return new SettingsDAL().GetPlayHistory(fileName);
        }

        public static int SetPlayHistoryNotPlayed(string fileName)
        {
            return new SettingsDAL().SetPlayHistoryNotPlayed(fileName);
        }
    }
}
