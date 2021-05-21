using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LocalModel
    {
    }

    public class ManualRenameModel
    { 
        public RenameLocation location { get; set; }
        public RenamneLanguage language { get; set; }
        public int episode { get; set; }
        public int avDbId { get; set; }
        public string rootFolder { get; set; }
        public string moveFile { get; set; }
    }

    public class ManualRenameResultModel : WebResult
    { 
        public MyFileInfo from { get; set; }
        public MyFileInfo to { get; set; }
    }

    public enum RenameLocation
    { 
        Fin = 0,
        Notfound = 1,
        US = 2,
        VR = 3,
        Uncensor = 4
    }

    public enum RenamneLanguage
    { 
        Janpanese = 0,
        Chinese = 1
    }
}
