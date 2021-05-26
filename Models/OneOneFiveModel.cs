using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OneOneFiveFolder
    {
        public static string All = "0";
        public static string Temp = "2091444771508569858";
        public static string Download = "2068937774368408801";
        public static string Upload = "2089198611951633887";
        public static string AV = "1810938661372282371";
        public static string Fin = "1834397846621504875";
        public static string NotFound = "2091514445650838660";
        public static string MoveBackToLocal = "2092826214403000069";
    }

    public class OneOneFiveFileListModel
    {
        public int count { get; set; }
        public int cur { get; set; }
        public int offset { get; set; }
        public int page_size { get; set; }
        public List<OneOneFiveFileItemModel> data { get; set; }
    }

    public class OneOneFiveFileItemModel
    {
        public string cid { get; set; }
        public string aid { get; set; }
        public string uid { get; set; }
        public string @class { get; set; }
        public string fid { get; set; }
        public string ico { get; set; }
        public string n { get; set; }
        public string pc { get; set; }
        public long play_long { get; set; }
        public long s { get; set; }
        public string sha { get; set; }
        public string t { get; set; }
        public DateTime time
        {
            get
            {
                DateTime ret = new DateTime(1970, 1, 1);
                if (DateTime.TryParse(t, out ret))
                {
                    return ret;
                }
                else
                {
                    return ret.AddMilliseconds(double.Parse(t));
                }
            }
        }
        public string AvId { get; set; }
        public string AvName { get; set; }
    }

    public enum OneOneFiveSearchType
    {
        All = 0,
        Video = 4,
    }

    public class OneOneFiveResult
    {
        public bool state { get; set; }
        public string error { get; set; }
        public string errorno { get; set; }
    }

    public class OneOneFiveCookie
    {
        public string CookieJson { get; set; }
        public string UserAgent { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public enum OneOneFiveUrlType
    { 
        Search = 1,
        List = 2,
        AddTask = 3,
        Move = 4,
        Delete = 5,
        Copy = 6,
    }
}
