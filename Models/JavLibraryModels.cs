using System;

namespace Models
{
    public class JavLibraryModels
    {
    }

    public class JavLibraryCookieJson
    {
        public string CookieJson{ get; set; }
        public string UserAgent{ get; set; }
        public DateTime CreateTime{ get; set; }
    }

    public enum JavLibraryEntryPointType
    {
        Category = 1,
        Actress = 2,
        Director = 3,
        Company = 4,
        Publisher = 5,
        BestRate = 6,
        MostWanted = 7,
        Update = 8,
        Rank = 9,
        Other = 10,
        Search = 11,
        Scan = 12,
    }

    public class JavLibrarySearchOrder
    {
        public static string Asc = "Asc";
        public static string Desc = "Desc";
    }
}
