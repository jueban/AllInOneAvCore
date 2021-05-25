﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SeedMagnetSearchModel
    {
        public string SerachContent { get; set; }
        public string SearchUrl { get; set; }
        public string Title { get; set; }
        public string MagUrl { get; set; }
        public double MagSize { get; set; }
        public DateTime Date { get; set; }
        public int CompleteCount { get; set; }
        public SearchSeedSiteEnum Source { get; set; }
    }

    public enum SearchSeedSiteEnum
    {
        Btsow = 1,
        SukebeiSi = 2,
        SukebeiPro = 3,
        JavBus = 4
    }
}
