using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace JobHub.Helper
{
    public class LocalHelper
    {
        public static async Task RemoveFolder(string folder)
        {
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                Dictionary<string, string> remainSize = new Dictionary<string, string>();
                Dictionary<string, int> moveRecord = new Dictionary<string, int>();

                var moveFolder = InitRemove(folder);

                var setting = await SettingService.GetSetting();
                var exclude = setting.ExcludeFolder + "," + moveFolder;
                var filter = setting.AvNameFilter.Split(',').ToList();

                var fis = GetMoveFiles(folder, exclude, filter);

                foreach (var fi in fis)
                {
                    //richTextBox1.AppendText("开始移动 " + fi.FullName, Color.Green, font, true);

                    var n = fi.Name.Replace(fi.Extension, "");
                    var e = fi.Extension;

                    //richTextBox1.AppendText("\t文件名 >= " + n + " 扩展名 => " + e, Color.Black, font, true);

                    if (fi.Name.Contains("-5" + fi.Extension) && fi.Length < 1 * 1024 * 1024 * 1024)
                    {
                        //richTextBox1.AppendText("删除dummy文件");
                        continue;
                    }

                    if (moveRecord.ContainsKey(fi.Name))
                    {
                        moveRecord[fi.Name]++;
                        //richTextBox1.AppendText("\t存在移动记录,添加后缀 >= " + moveRecord[fi.Name], Color.Red, font, true);
                    }
                    else
                    {
                        moveRecord.Add(fi.Name, 1);
                    }

                    if (File.Exists(moveFolder + n + e))
                    {
                        var oldN = n;

                        n += "_" + moveRecord[fi.Name];

                        //richTextBox1.AppendText("\t存在重名文件,修改文件名 >= " + (n + "_" + moveRecord[fi.Name]), Color.Red, font, true);

                        if (moveRecord[fi.Name] == 2)
                        {
                            File.Move(moveFolder + oldN + e, moveFolder + oldN + "_1" + e);
                        }
                    }

                    //richTextBox1.AppendText("\t移动文件 >= " + fi.FullName + " 到 => " + moveFolder + n + e, Color.Green, font, true);
                    File.Move(fi.FullName, moveFolder + n + e);
                }

                //richTextBox1.AppendText("开始计算剩余各子文件夹大小", Color.Green, font, true);

                var subFolders = Directory.GetDirectories(folder);

                foreach (var sub in subFolders)
                {
                    //richTextBox1.AppendText("\t开始计算子文件夹 " + sub + " 的大小", Color.Black, font, true);

                    List<FileInfo> tempFi = new();

                    string tempStatus = FileUtility.GetFilesRecursive(sub, exclude, filter, FileUtility.VideoExtensions + ";!qB", tempFi);
                    double tempSize = 0D;

                    if (string.IsNullOrEmpty(tempStatus))
                    {
                        foreach (var fi in tempFi)
                        {
                            tempSize += fi.Length;
                        }

                        remainSize.Add(sub, FileUtility.GetAutoSizeString(tempSize, 2));

                        if (tempSize >= 500 * 1024 * 1024)
                        {
                            //richTextBox1.AppendText("\t" + sub + "的大小为 = > " + FileSize.GetAutoSizeString(tempSize, 2), Color.Red, font, true);
                        }
                        else
                        {
                            //richTextBox1.AppendText("\t" + sub + "的大小为 = > " + FileSize.GetAutoSizeString(tempSize, 2), Color.Black, font, true);
                        }
                    }
                }
            }
        }

        private static string InitRemove(string folder)
        {
            var moveFolder = folder + "/movefiles/";

            if (!Directory.Exists(moveFolder))
            {
                Directory.CreateDirectory(moveFolder);
            }

            //richTextBox1.AppendText("初始化移动到的文件夹为 -> " + moveFolder, Color.Green, font, true);
            //richTextBox1.AppendText("把 " + moveFolder + " 添加到忽略列表", Color.Green, font, true);

            return moveFolder;
        }

        private static List<FileInfo> GetMoveFiles(string folder, string exclude, List<string> filters)
        {
            int limitSize = 200;
            List<FileInfo> fis = new List<FileInfo>();

            //richTextBox1.AppendText("获取所有 >= " + limitSize + "mb 的在子文件夹内的文件", Color.Green, font, true);

            var status = FileUtility.GetFilesRecursive(folder, exclude, filters, FileUtility.VideoExtensions, fis, limitSize);

            if (string.IsNullOrEmpty(status))
            {
                //richTextBox1.AppendText("一共获取了 >= " + fis.Count + " 个文件", Color.Green, font, true);
            }
            else
            {
                //richTextBox1.AppendText("异常 >= " + status, Color.Red, font, true);
            }

            return fis;
        }
    }
}
