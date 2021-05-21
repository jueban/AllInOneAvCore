using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services
{
    public class MagnetUrlService
    {
        public static (WebScanUrlSite site, string type, string url) SaveFaviUrl(string url)
        {
            if (url.Contains("javbus.com", StringComparison.OrdinalIgnoreCase))
            {
                var info = PorcessJavBusUrl(url);
                return (WebScanUrlSite.JavBus, info.type, info.url);
            }

            if (url.Contains("javlibrary.com", StringComparison.OrdinalIgnoreCase))
            {
                var info = PorcessJavLibraryUrl(url);
                return (WebScanUrlSite.JavLibrary, info.type, info.url);
            }

            return (WebScanUrlSite.None, "", "");
        }

        private static (string type, string url) PorcessJavLibraryUrl(string url)
        {
            (string type, string url) ret = new();

            if (url.Contains("director"))
            {
                return DoJavLibraryUrl("director", url);
            }

            if (url.Contains("maker"))
            {
                return DoJavLibraryUrl("maker", url);
            }

            if (url.Contains("star"))
            {
                return DoJavLibraryUrl("star", url);
            }

            if (url.Contains("label"))
            {
                return DoJavLibraryUrl("label", url);
            }

            if (url.Contains("genre"))
            {
                return DoJavLibraryUrl("genre", url);
            }

            if (url.Contains("searchbyid"))
            {
                return DoJavLibraryUrl("searchbyid", url);
            }

            return ret;
        }

        private static (string type, string url) PorcessJavBusUrl(string url)
        {
            if (url.Contains("director"))
            {
                return DoJavBusUrl("director", url);
            }

            if (url.Contains("star"))
            {
                return DoJavBusUrl("star", url);
            }

            if (url.Contains("studio"))
            {
                return DoJavBusUrl("studio", url);
            }

            if (url.Contains("label"))
            {
                return DoJavBusUrl("label", url);
            }

            if (url.Contains("genre"))
            {
                return DoJavBusUrl("genre", url);
            }

            if (url.Contains("series"))
            {
                return DoJavBusUrl("series", url);
            }

            if (url.Contains("search"))
            {
                return DoJavBusUrl("search", url);
            }

            return ("", "");
        }

        private static (string type, string url) DoJavLibraryUrl(string type, string url)
        {
            (string type, string url) ret = new();

            Dictionary<string, string> dic = new() { { "director", "director" }, { "star", "actress" }, { "label", "publisher" }, { "maker", "company" }, { "genre", "category" }, { "searchbyid", "prefix" } };

            Regex reg = new($@"http://www.javlibrary.com/cn/vl_{type}(.+)");
            Match match = reg.Match(url);
            string value = match.Groups[1].Value;

            if (value.Contains("page"))
            {
                ret.url = url.Substring(0, url.LastIndexOf("&"));
            }
            else
            {
                ret.url = url;
            }

            ret.type = dic[type];

            return ret;
        }

        private static (string type, string url) DoJavBusUrl(string type, string url)
        {
            (string type, string url) ret = new();

            Dictionary<string, string> dic = new() { { "director", "director" }, { "star", "actress" }, { "label", "publisher" }, { "studio", "company" }, { "genre", "category" }, { "series", "series" }, { "search", "prefix" } };

            Regex reg = new($@"https://www.javbus.com/{type}/(.+)");
            Match match = reg.Match(url);
            string value = match.Groups[1].Value;

            if (value.Contains("/"))
            {
                ret.url = url.Substring(0, url.LastIndexOf("/"));
            }
            else
            {
                ret.url = url;
            }

            ret.type = dic[type];

            return ret;
        }
    }
}
