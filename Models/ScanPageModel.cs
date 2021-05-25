using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ScanPageModel
    {
        public int Page { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public List<ScanPageDrop> Drops { get; set; }
    }

    public class ScanPageDrop
    { 
        public int Type { get; set; }
        public string Title { get; set; }
        public List<ScanPageDropItem> Items { get; set; }
    }

    public class ScanPageDropItem
    { 
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
