using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PlayHistory
    {
        public string FileName { get; set; }
        public int PlayTimes { get; set; }
        public DateTime LastPlay { get; set; }
        public bool SetNotPlayed { get; set; }
    }
}