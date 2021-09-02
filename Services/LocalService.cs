using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

                var moveSubFolder = @"\movefiles\";
                var moveFolder = folder + moveSubFolder;

                if (!Directory.Exists(moveFolder))
                {
                    Directory.CreateDirectory(moveFolder);
                }

                progress.Report($"初始化移动到的文件夹为 -> {moveFolder}");

                var setting = await SettingService.GetSetting();
                var exclude = setting.ExcludeFolder + "," + moveSubFolder;
                var filters = setting.AvNameFilter.Split(',').ToList();

                int limitSize = 200;
                List<FileInfo> fis = new List<FileInfo>();

                var status = FileUtility.GetFilesRecursive(folder, exclude, filters, FileUtility.VideoExtensions, fis, limitSize);

                if (string.IsNullOrEmpty(status))
                {
                    progress.Report($"一共获取了 {fis.Count} 个 大于等于 {limitSize}MB 的文件");
                }
                else
                {
                    progress.Report($"获取文件异常 {status}");
                }

                foreach (var fi in fis)
                {
                    var n = fi.Name.Replace(fi.Extension, "");
                    var e = fi.Extension;

                    progress.Report($"开始处理 {fi.FullName}, 文件名 {n} 扩展名 {e}");

                    if (fi.Name.Contains("-5" + fi.Extension) && fi.Length < 1 * 1024 * 1024 * 1024)
                    {
                        progress.Report("\t删除dummy文件");
                        continue;
                    }

                    if (moveRecord.ContainsKey(fi.Name))
                    {
                        moveRecord[fi.Name]++;
                        progress.Report($"\t存在移动记录,添加后缀 {moveRecord[fi.Name]}");
                    }
                    else
                    {
                        moveRecord.Add(fi.Name, 1);
                    }

                    if (File.Exists(moveFolder + n + e))
                    {
                        var oldN = n;

                        n += "_" + moveRecord[fi.Name];

                        progress.Report($"\t存在重名文件,修改文件名 {(n + "_" + moveRecord[fi.Name])}");

                        if (moveRecord[fi.Name] == 2)
                        {
                            File.Move(moveFolder + oldN + e, moveFolder + oldN + "_1" + e);
                        }
                    }

                    progress.Report($"\t移动文件 >= {fi.FullName} 到 => {moveFolder + n + e}");
                    File.Move(fi.FullName, moveFolder + n + e);
                }

                progress.Report("开始计算剩余各子文件夹大小");

                var subFolders = Directory.GetDirectories(folder);

                foreach (var sub in subFolders)
                {
                    if (!exclude.Contains(Path.GetFileName(sub), StringComparison.OrdinalIgnoreCase))
                    {
                        List<FileInfo> tempFi = new();

                        string tempStatus = FileUtility.GetFilesRecursive(sub, exclude, filters, FileUtility.VideoExtensions + ";!qB", tempFi);
                        double tempSize = 0D;

                        if (string.IsNullOrEmpty(tempStatus))
                        {
                            foreach (var fi in tempFi)
                            {
                                tempSize += fi.Length;
                            }

                            remainSize.Add(sub, FileUtility.GetAutoSizeString(tempSize, 1));

                            if (tempSize >= 500 * 1024 * 1024)
                            {
                                progress.Report($"\t =============={sub} 的大小为 {FileUtility.GetAutoSizeString(tempSize, 1)}==============");
                            }
                            else
                            {
                                progress.Report($"\t {sub} 的大小为 {FileUtility.GetAutoSizeString(tempSize, 1)}");
                            }
                        }
                    }
                }
            }
        }

        public static async Task Rename(string folder, IProgress<string> progress)
        {
            List<string> allPrefix = new List<string>();
            Dictionary<FileInfo, List<AvModel>> ret = new Dictionary<FileInfo, List<AvModel>>();
            Dictionary<string, int> moveReocrd = new Dictionary<string, int>();

            var moveSubFolder = @"\tempFin\";
            var moveFolder = folder + moveSubFolder;
            int found = 0;
            int notFound = 0;

            progress.Report($"初始化移动目录 {moveFolder}");

            if (!Directory.Exists(moveFolder))
            {
                Directory.CreateDirectory(moveFolder);
            }
            else
            {
                var alreadyFiles = new DirectoryInfo(moveFolder).GetFiles();

                foreach (var f in alreadyFiles)
                {
                    moveReocrd.Add(f.Name.ToUpper(), 1);
                }
            }

            progress.Report("开始加载缓存");

            var avs = await new JavLibraryDAL().GetAvModelByWhere("");

            progress.Report($"共加载 {avs.Count} 条缓存并开始处理前缀");

            allPrefix = new SettingsDAL().GetPrefix().Result.OrderByDescending(x => x.Length).ToList();

            progress.Report($"共加载 {allPrefix.Count} 条前缀");

            progress.Report($"开始获取目录 {folder} 下的文件");

            var files = Directory.GetFiles(folder);

            progress.Report($"共获取 {files.Length} 个文件");

            foreach (var f in files)
            {
                progress.Report($"开始处理文件 {f}");

                bool findMatch = false;
                string pi = "";
                FileInfo fi = new FileInfo(f);
                List<AvModel> fiMatchList = new List<AvModel>();

                var fileNameWithoutFormat = fi.Name.Replace(fi.Extension, "");

                ret.Add(fi, fiMatchList);

                foreach (var prefix in allPrefix)
                {
                    if (fileNameWithoutFormat.Contains(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        progress.Report($"\t找到匹配前缀 {prefix}");

                        var pattern = prefix + "{1}-?\\d{1,7}";
                        var possibleId = Regex.Match(fileNameWithoutFormat, pattern, RegexOptions.IgnoreCase).Value;

                        if (possibleId.Contains("-"))
                        {
                            pi = possibleId;
                        }
                        else
                        {
                            bool isFirst = true;
                            StringBuilder sb = new StringBuilder();

                            foreach (var c in possibleId)
                            {
                                if (c >= '0' && c <= '9')
                                {
                                    if (isFirst)
                                    {
                                        sb.Append("-");
                                        isFirst = false;
                                    }
                                }
                                sb.Append(c);
                            }

                            pi = sb.ToString();
                        }

                        if (!string.IsNullOrEmpty(pi))
                        {
                            progress.Report($"\t找到适配的番号 {pi}");

                            var possibleAv = avs.Where(x => x.AvId.Equals(pi, StringComparison.OrdinalIgnoreCase)).ToList();

                            if (possibleAv == null || possibleAv.Count <= 0)
                            {
                                var prefixPart = pi.Split('-')[0];
                                var numberPart = pi.Split('-')[1];

                                if (numberPart.StartsWith("00"))
                                {
                                    numberPart = numberPart.Substring(2);

                                    pi = prefixPart + "-" + numberPart;

                                    possibleAv = avs.Where(x => x.AvId.Equals(pi, StringComparison.OrdinalIgnoreCase)).ToList();
                                }
                            }

                            findMatch = true;
                            foreach (var av in possibleAv)
                            {
                                fiMatchList.AddRange(possibleAv);
                            }

                            progress.Report($"\t找到适配的AV {fiMatchList.Count} 条");

                            break;
                        }
                    }
                }

                if (findMatch)
                {
                    found++;
                }
                else
                {
                    notFound++;
                }
            }

            foreach (var item in ret)
            {
                if (item.Value.Count == 0)
                {
                    progress.Report($"文件 {item.Key.Name} 没有找到匹配");
                }
                else if (item.Value.Count > 1)
                {
                    var names = item.Value.Select(x => x.FileNameWithoutExtension).Distinct();
                    if (names.Count() != 1)
                    {
                        progress.Report($"文件 {item.Key.Name} 找到多条匹配,暂不处理");
                    }
                    else
                    {
                        progress.Report($"文件 {item.Key.Name} 找到1条匹配,开始处理");

                        var chinese = "";

                        if (item.Key.Name.Replace(item.Value.FirstOrDefault().AvId, "").Replace(item.Value.FirstOrDefault().Name, "").Contains("-C", StringComparison.OrdinalIgnoreCase) || item.Key.Name.Replace(item.Value.FirstOrDefault().AvId, "").Replace(item.Value.FirstOrDefault().Name, "").Contains("ch", StringComparison.OrdinalIgnoreCase))
                        {
                            chinese = "-C";
                        }

                        var tempFileName = item.Value.FirstOrDefault().AvId + "-" + item.Value.FirstOrDefault().Name + chinese + item.Key.Extension;
                        tempFileName = tempFileName.ToUpper();

                        if (moveReocrd.ContainsKey(tempFileName))
                        {
                            moveReocrd[tempFileName]++;

                            progress.Report($"\t存在移动记录,文件名后缀+1 -> {moveReocrd[tempFileName]}");

                            if (moveReocrd[tempFileName] == 2)
                            {
                                var oldFileMove = item.Value.FirstOrDefault().AvId + "-" + item.Value.FirstOrDefault().Name + "-1" + item.Key.Extension;
                                File.Move(moveFolder + tempFileName, moveFolder + oldFileMove.ToUpper());
                            }

                            tempFileName = item.Value.FirstOrDefault().AvId + "-" + item.Value.FirstOrDefault().Name + "-" + moveReocrd[tempFileName] + item.Key.Extension;
                        }
                        else
                        {
                            moveReocrd.Add(tempFileName, 1);
                        }

                        try
                        {
                            File.Move(item.Key.FullName, moveFolder + tempFileName);

                            progress.Report($"\t移动文件 -> {item.Key.FullName} 到 -> {moveFolder + tempFileName}");
                        }
                        catch (Exception ee)
                        {
                            progress.Report("异常 " + ee.ToString());
                        }
                    }
                }
                else if (item.Value.Count == 1)
                {
                    progress.Report($"文件 {item.Key.Name} 找到1条匹配,开始处理");

                    var chinese = "";

                    if (item.Key.Name.Replace(item.Value.FirstOrDefault().AvId, "").Replace(item.Value.FirstOrDefault().Name, "").Contains("-C", StringComparison.OrdinalIgnoreCase) || item.Key.Name.Replace(item.Value.FirstOrDefault().AvId, "").Replace(item.Value.FirstOrDefault().Name, "").Contains("ch", StringComparison.OrdinalIgnoreCase))
                    {
                        chinese = "-C";
                    }

                    var tempFileName = item.Value.FirstOrDefault().AvId + "-" + item.Value.FirstOrDefault().Name + chinese + item.Key.Extension;
                    tempFileName = tempFileName.ToUpper();

                    if (moveReocrd.ContainsKey(tempFileName))
                    {
                        moveReocrd[tempFileName]++;

                        progress.Report($"\t存在移动记录,文件名后缀+1 -> {moveReocrd[tempFileName]}");

                        if (moveReocrd[tempFileName] == 2)
                        {
                            var oldFileMove = item.Value.FirstOrDefault().AvId + "-" + item.Value.FirstOrDefault().Name + "-1" + item.Key.Extension;
                            File.Move(moveFolder + tempFileName, moveFolder + oldFileMove.ToUpper());
                        }

                        tempFileName = item.Value.FirstOrDefault().AvId + "-" + item.Value.FirstOrDefault().Name + "-" + moveReocrd[tempFileName] + item.Key.Extension;
                    }
                    else
                    {
                        moveReocrd.Add(tempFileName, 1);
                    }

                    try
                    {
                        File.Move(item.Key.FullName, moveFolder + tempFileName);

                        progress.Report($"\t移动文件 -> {item.Key.FullName} 到 -> {moveFolder + tempFileName}");
                    }
                    catch (Exception ee)
                    {
                        progress.Report("异常 " + ee.ToString());
                    }
                }
            }

            progress.Report($"找到匹配 --> {found} 未找到匹配 --> {notFound}");
        }

        public static string GetFolderInfo(string folder)
        {
            string ret = "";
            if (Directory.Exists(folder))
            {
                var di = new DirectoryInfo(folder);
                var files = di.GetFiles();

                ret = $"{folder} 共有 {di.GetDirectories().Length} 个文件夹，以及 {files.Length} 个独立文件，总计大小 {FileUtility.GetAutoSizeString(files.Sum(x => x.Length), 1)}";
            }

            return ret;
        }

        public static List<MyFileInfo> GetFolderFiles(string folder)
        {
            List<MyFileInfo> ret = new();

            if (Directory.Exists(folder))
            {
                new DirectoryInfo(folder).GetFiles().ToList().ForEach(x => ret.Add(new MyFileInfo() { 
                    Extension = x.Extension,
                    FullName = x.FullName,
                    Length = x.Length,
                    LengthStr = FileUtility.GetAutoSizeString(x.Length, 1),
                    Name = x.Name
                }));
            }

            return ret;
        }

        public static async Task<(string name, List<AvModel> av)> GetPossibleAvNameAndInfo(string fileName)
        {
            (string name, List<AvModel> av) ret = new();

            var imgFolder = SettingService.GetSetting().Result.JavLibraryImageFolder;
            string name = "";
            var fi = new FileInfo(fileName);

            var allPrefix = new SettingsDAL().GetPrefix().Result.OrderByDescending(x => x.Length).ToList();
            var fileNameWithoutFormat = fi.Name.Replace(fi.Extension, "");

            foreach (var prefix in allPrefix)
            {
                if (fileNameWithoutFormat.Contains(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var pattern = prefix + "{1}-?\\d{1,5}";
                    var possibleId = Regex.Match(fileNameWithoutFormat, pattern, RegexOptions.IgnoreCase).Value;

                    if (possibleId.Contains("-"))
                    {
                        name = possibleId;
                    }
                    else
                    {
                        bool isFirst = true;
                        StringBuilder sb = new StringBuilder();

                        foreach (var c in possibleId)
                        {
                            if (c >= '0' && c <= '9')
                            {
                                if (isFirst)
                                {
                                    sb.Append("-");
                                    isFirst = false;
                                }
                            }
                            sb.Append(c);
                        }

                        name = sb.ToString();
                    }

                    break;
                }
            }

            ret.name = name.ToUpper();

            if (!string.IsNullOrWhiteSpace(name))
            {
                var avs = await new JavLibraryDAL().GetAvModelByWhere($" AND AvId='{name}'");

                if (avs != null && avs.Any())
                {
                    //foreach (var av in avs)
                    //{
                    //    av.PicUrl = "file://" + (imgFolder + av.AvId + "-" + av.Name + ".jpg").Replace("\\", "/");
                    //}
                    ret.av = avs;
                }
                else
                {
                    ret.av = new List<AvModel>();
                }
            }
            else
            {
                ret.av = new List<AvModel>();
            }

            return ret;
        }

        public static async Task<List<AvModel>> GetPossibleAvMatch(string avId)
        {
            var ret = new List<AvModel>();

            ret = await new JavLibraryDAL().GetAvModelByWhere($" AND AvId = '{avId}'");

            return ret;
        }


        public static bool ManualRemove(ManualRenameModel model)
        {
            bool ret = false;

            var di = new FileInfo(model.moveFile);

            var targetFolder = CreateNeededFolder(model.rootFolder, model.location);
            targetFolder = targetFolder.EndsWith("\\") ? targetFolder : targetFolder + "\\";

            var av = new JavLibraryDAL().GetAvModelById(model.avDbId).Result;

            if (av != null)
            {
                var targetfile = GenerateTagetFileName(targetFolder, di.Extension, av, model);

                var res = FileUtility.RenameAndTransferUsingSystem(model.moveFile, targetfile, true, false);

                if (res == 0)
                {
                    ret = true;
                }
            }

            return ret;
        }

        public static string GenerateTagetFileName(string targetFolder, string extension, AvModel av, ManualRenameModel model)
        {
            var ret = targetFolder + av.AvId + "-" + av.Name;

            if (model.episode > 0)
            {
                ret += "-" + model.episode;
            }

            if (model.language == RenamneLanguage.Chinese)
            {
                ret += "-C";
            }

            return ret + extension;
        }

        public static string CreateNeededFolder(string currentFolder, RenameLocation location)
        {
            currentFolder = (currentFolder.EndsWith("\\") || currentFolder.EndsWith("/")) ? currentFolder : currentFolder + "\\";
            var ret = "";

            switch (location)
            {
                case RenameLocation.Fin:
                    ret = CreateFolder(currentFolder + @"fin\");
                    break;
                case RenameLocation.Notfound:
                    ret = CreateFolder(currentFolder + @"未找到\");
                    break;
                case RenameLocation.Uncensor:
                    ret = CreateFolder(currentFolder + @"无码\");
                    break;
                case RenameLocation.US:
                    ret = CreateFolder(currentFolder + @"欧美\");
                    break;
                case RenameLocation.VR:
                    ret = CreateFolder(currentFolder + @"VR\");
                    break;
            }

            return ret;
        }

        public static async Task<Dictionary<string, List<MyFileInfo>>> GetDuplicateAvFile()
        {
            var ignore = SettingService.GetSetting().Result.CannotMergeFileTag;
            Dictionary<string, List<MyFileInfo>> ret = new();

            var files = await GetAllLocalMyFile();

            foreach (var file in files)
            {
                var nameArray = file.Name.Split('-');

                if (nameArray.Length >= 3 && !file.Name.Contains(ignore))
                {
                    var key = nameArray[0] + "-" + nameArray[1];

                    if (ret.ContainsKey(key))
                    {
                        ret[key].Add(file);
                    }
                    else
                    {
                        ret.Add(key, new List<MyFileInfo> { file });
                    }
                }
            }

            return ret.Where(x => x.Value.Count > 1).ToDictionary(x => x.Key, x => x.Value);
        }

        public static async Task<List<MyFileInfo>> GetAllLocalMyFile()
        {
            List<MyFileInfo> ret = new();

            var oris = await GetAllLocalFile();

            foreach (var ori in oris)
            {
                ret.Add(new MyFileInfo()
                {
                    Extension = ori.Extension,
                    FullName = ori.FullName,
                    Length = ori.Length,
                    LengthStr = FileUtility.GetAutoSizeString(ori.Length, 1),
                    Name = ori.Name,
                    IsChinese = ori.Name.Contains("-C" + ori.Extension, StringComparison.OrdinalIgnoreCase)
                });
            }

            return ret;
        }

        public async static Task<List<FileInfo>> GetAllLocalFile(bool onlyFin = false)
        {
            List<FileInfo> ret = new();
            var searchFolder = await SettingService.GetSetting();
            var drives = Environment.GetLogicalDrives();

            if (onlyFin)
            {
                searchFolder.LocalSearchFolder = "fin";
            }

            foreach (var drive in drives)
            {
                foreach (var folder in searchFolder.LocalSearchFolder.Split(','))
                {
                    if (Directory.Exists(drive + folder))
                    {
                        ret.AddRange(new DirectoryInfo(drive + folder).GetFiles().ToList());
                    }
                }
            }

            return ret;
        }

        public static int DeleteFiles(List<string> files)
        {
            int count = 0;

            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                    count++;
                }
            }

            return count;
        }

        public async static Task<int> KeepAndDelete(KeepModel model)
        {
            foreach (var d in model.avs)
            {
                if (d.Keep == false)
                {
                    File.Delete(d.Fid);
                }
                else
                {
                    var fi = new FileInfo(d.Fid);
                    var keepLocation = fi.DirectoryName + "\\keep\\";
                    var keepFile = keepLocation + fi.Name;

                    if (!Directory.Exists(keepLocation))
                    {
                        Directory.CreateDirectory(keepLocation);
                        await Task.Delay(15);
                    }

                    fi.MoveTo(keepFile);
                }
            }

            return 1;
        }

        public static List<MyFileInfo> GetLocalAvs(string folder)
        {
            List<MyFileInfo> ret = new();

            if (Directory.Exists(folder))
            {
                var files = new DirectoryInfo(folder).GetFiles("*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    if (EverythingService.Extensions.Contains(file.Extension.Replace(".", "").ToLower()) && file.Length > 1024 * 1024 * 200) 
                    {
                        var playHistory = SettingService.GetPlayHistory(file.Name);

                        ret.Add(new MyFileInfo()
                        {
                            CreateDate = file.LastWriteTime,
                            Extension = file.Extension,
                            Folder = file.DirectoryName,
                            FullName = file.FullName.Replace("\\", "\\\\"),
                            Length = file.Length,
                            LengthStr = FileUtility.GetAutoSizeString(file.Length, 1),
                            Name = file.Name,
                            PlayTimes = playHistory == null ? 0 : playHistory.PlayTimes,
                            SetNotPlayed = playHistory == null ? true : playHistory.SetNotPlayed
                        });
                    }
                }
            }

            return ret;
        }

        private static string CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return Directory.CreateDirectory(folder).FullName;
            }
            else
            {
                return folder;
            }
        }
    }
}
