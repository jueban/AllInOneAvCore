using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SystemTreeView
    {
        public string Dir { get; set; }
        public long TotalSize { get; set; }
        public long AvailableSize { get; set; }
        public string DirName { get; set; }
        public string TotalSizeStr { get; set; }
        public string AvailableSizeStr { get; set; }
    }

    public class FileFolderBreadCrumb
    { 
        public List<MyFileInfo> Files { get; set; }
        public List<MyDirectoryInfo> Dirs { get; set; }
        public Dictionary<string, string> BreadCrumb { get; set; }
        public string CurrentInfo { get; set; }
    }

    public class MyFileInfo
    {
        public string FullName { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
        public string Name { get; set; }
        public string LengthStr { get; set; }
        public bool IsChinese { get; set; }
        public DateTime CreateDate { get; set; }
        public string Folder { get; set; }
        public int PlayTimes { get; set; }
        public bool SetNotPlayed { get; set; }
        public List<string> Thumnails { get; set; }
        public string NewFullName { get; set; }
        public bool ChangeName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRemote { get; set; }
    }

    public class MyDirectoryInfo
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public MyDirectoryInfo Parent { get; set; }
    }
}
