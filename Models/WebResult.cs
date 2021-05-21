using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class WebResult
    {
        public string msg { get; set; }
        public Status status { get; set; }
    }

    public enum Status
    { 
        Ok = 0,
        Error = -2,
        Notfound = 1,
        Exception = -1
    }
}
