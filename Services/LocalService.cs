using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class LocalService
    {
        public static List<string> GetLocalDrives()
        {
            return Environment.GetLogicalDrives().ToList();
        }

        public static List<SystemTreeView> GetSystemTreeView()
        {
            List<SystemTreeView> ret = new List<SystemTreeView>();
            var roots = GetLocalDrives();

            foreach (var root in roots)
            {
                var di = new DriveInfo(root);

                SystemTreeView temp = new SystemTreeView();
                temp.Dir = root;
                temp.DirName = di.Name + " " + di.VolumeLabel;
                temp.AvailableSize = di.AvailableFreeSpace;
                temp.TotalSize = di.TotalSize;
                temp.AvailableSizeStr = FileUtility.GetAutoSizeString(di.AvailableFreeSpace, 1);
                temp.TotalSizeStr = FileUtility.GetAutoSizeString(di.TotalSize, 1);

                ret.Add(temp);
            }

            return ret;
        }

        public static FileFolderBreadCrumb GetFilesAndFolders(string root)
        {
            FileFolderBreadCrumb ret = new();
            ret.Dirs = new List<MyDirectoryInfo>();
            ret.Files = new List<MyFileInfo>();
            ret.BreadCrumb = new Dictionary<string, string>();

            if (Directory.Exists(root))
            {
                var di = new DirectoryInfo(root);

                di.GetFiles().Where(x => (x.Attributes & FileAttributes.Hidden) == 0).ToList().ForEach(x => ret.Files.Add(new MyFileInfo()
                {
                    FullName = x.FullName,
                    Extension = x.Extension,
                    Length = x.Length,
                    LengthStr = FileUtility.GetAutoSizeString(x.Length, 1),
                    Name = x.Name
                }));

                di.GetDirectories().Where(x => (x.Attributes & FileAttributes.Hidden) == 0).ToList().ForEach(x => ret.Dirs.Add(new MyDirectoryInfo() { 
                    FullName = x.FullName,
                    Name = x.Name,
                }));

                var bread = di;
                while (bread != null)
                {
                    ret.BreadCrumb.Add(bread.FullName, bread.Name.Replace("\\", ""));
                    bread = bread.Parent;
                }

                ret.BreadCrumb = ret.BreadCrumb.Reverse().ToDictionary(x => x.Key, x => x.Value);

                ret.CurrentInfo = $"共有 {ret.Dirs.Count} 个文件夹及 {ret.Files.Count} 个文件，总大小 {FileUtility.GetAutoSizeString(ret.Files.Sum(x => x.Length), 1)}";
            }

            return ret;
        }

        public static async Task RemoveFolder(string folder, IProgress<string> progress)
        {
            if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder))
            {
                Dictionary<string, string> remainSize = new Dictionary<string, string>();
                Dictionary<string, int> moveRecord = new Dictionary<string, int>();

                var moveFolder = folder + "/movefiles/";

                if (!Directory.Exists(moveFolder))
                {
                    Directory.CreateDirectory(moveFolder);
                }

                progress.Report("初始化移动到的文件夹为 -> " + moveFolder);

                progress.Report("把 " + moveFolder + " 添加到忽略列表");
                var setting = await SettingService.GetSetting();
                var exclude = setting.ExcludeFolder + "," + moveFolder;
                var filters = setting.AvNameFilter.Split(',').ToList();

                int limitSize = 200;
                List<FileInfo> fis = new List<FileInfo>();

                progress.Report("获取所有 >= " + limitSize + "mb 的在子文件夹内的文件");

                var status = FileUtility.GetFilesRecursive(folder, exclude, filters, FileUtility.VideoExtensions, fis, limitSize);

                if (string.IsNullOrEmpty(status))
                {
                    progress.Report("一共获取了 >= " + fis.Count + " 个文件");
                }
                else
                {
                    progress.Report("异常 >= " + status);
                }

                foreach (var fi in fis)
                {
                    progress.Report("开始移动 " + fi.FullName);

                    var n = fi.Name.Replace(fi.Extension, "");
                    var e = fi.Extension;

                    progress.Report("\t文件名 >= " + n + " 扩展名 => " + e);

                    if (fi.Name.Contains("-5" + fi.Extension) && fi.Length < 1 * 1024 * 1024 * 1024)
                    {
                        progress.Report("删除dummy文件");
                        continue;
                    }

                    if (moveRecord.ContainsKey(fi.Name))
                    {
                        moveRecord[fi.Name]++;
                        progress.Report("\t存在移动记录,添加后缀 >= " + moveRecord[fi.Name]);
                    }
                    else
                    {
                        moveRecord.Add(fi.Name, 1);
                    }

                    if (File.Exists(moveFolder + n + e))
                    {
                        var oldN = n;

                        n += "_" + moveRecord[fi.Name];

                        progress.Report("\t存在重名文件,修改文件名 >= " + (n + "_" + moveRecord[fi.Name]));

                        if (moveRecord[fi.Name] == 2)
                        {
                            File.Move(moveFolder + oldN + e, moveFolder + oldN + "_1" + e);
                        }
                    }

                    progress.Report("\t移动文件 >= " + fi.FullName + " 到 => " + moveFolder + n + e);
                    File.Move(fi.FullName, moveFolder + n + e);
                }

                progress.Report("开始计算剩余各子文件夹大小");

                var subFolders = Directory.GetDirectories(folder);

                foreach (var sub in subFolders)
                {
                    progress.Report("\t开始计算子文件夹 " + sub + " 的大小");

                    List<FileInfo> tempFi = new();

                    string tempStatus = FileUtility.GetFilesRecursive(sub, exclude, filters, FileUtility.VideoExtensions + ";!qB", tempFi);
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
                            progress.Report("\t" + sub + "的大小为 = > " + FileUtility.GetAutoSizeString(tempSize, 2));
                        }
                        else
                        {
                            progress.Report("\t" + sub + "的大小为 = > " + FileUtility.GetAutoSizeString(tempSize, 2));
                        }
                    }
                }
            }
        }
    }
}
