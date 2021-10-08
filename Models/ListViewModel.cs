using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class MatchNameListViewModel
    {
        public int Index { get; set; }
        public FileInfo OriFile { get; set; }
        public string DescFile { get; set; }
        public List<AvModel> PossibleFiles { get; set; }
    }
}
