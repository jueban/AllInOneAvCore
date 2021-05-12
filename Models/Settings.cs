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
        public string ImageFolder { get; set; }
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
