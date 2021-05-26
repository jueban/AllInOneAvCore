using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EverythingResult
    {
        public string totalResults { get; set; }
        public List<EverythingFileResult> results { get; set; }
    }

    public class EverythingFileResult
    {
        public string type { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public string size { get; set; }
        public string sizeStr { get; set; }
        public string chinese
        {
            get
            {
                return this.name.Contains("-C.") ? "中文" : "";
            }
        }
        public string location { get; set; }
    }

    [Flags]
    public enum FileLocation
    { 
        None = 0,
        Local = 1,
        OneOneFive = 2,
        LocalAndOneOneFive = 4
    }
}
