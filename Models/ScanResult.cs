using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Models
{
    public class ScanResult
    {
        public int Id { get; set; }
        public WebScanUrlSite WebSite { get; set; }
        public DateTime StartTime { get; set; }
        public string Url { get; set; }
        public string MagUrl { get; set; }
        public string Name { get; set; }

        public List<SeedMagnetSearchModel> MagUrlObj
        {
            get
            {
                if (!string.IsNullOrEmpty(this.MagUrl))
                {
                    return JsonHelper.Deserialize<List<SeedMagnetSearchModel>>(this.MagUrl);
                }
                else
                {
                    return new List<SeedMagnetSearchModel>();
                }
            }
        }

        public string WebSiteStr
        {
            get 
            {
                return Enum.GetName(typeof(WebScanUrlSite), this.WebSite);
            }
        }

        public string DateStr
        {
            get
            {
                return this.StartTime.ToString("yyyy-MM-dd hh:mm:ss");
            }
        }
    }
}
