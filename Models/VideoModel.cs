using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class VideoModel
    {
        public AvModel AvModel { get; set; }
        public List<MyFileInfo> FileInfo { get; set; }
    }
}
