using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Settings
    {
        public JavLibrarySettings JavLibrarySettings { get; set; }
        public CommonSettings CommonSettings { get; set; }
        public string BarkId { get; set; }
        public string JavLibraryImageFolder { get; set; }
        public string JavBusImageFolder { get; set; }
        public string AvatorImageFolder { get; set; }
        public string ExcludeFolder { get; set; }
        public string AvNameFilter { get; set; }
        public string Win10Duplicate { get; set; }
        public string LocalSearchFolder { get; set; }
        public string CannotMergeFileTag { get; set; }
    }

    public class JavLibrarySettings
    {
        public JavLibraryGetCookieMode CookieMode { get; set; }
    }

    public class CommonSettings
    {
        public string ChromeLocation { get; set; }
    }

    public enum SettingType
    { 
        WebConfig = 1,
    }
}
