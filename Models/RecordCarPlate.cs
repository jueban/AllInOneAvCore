using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RecordCarPlate
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Reason { get; set; }
        public DateTime CreateTime { get; set; }

        public override string ToString()
        {
            return $"车牌号{this.Plate}在{this.CreateTime.ToString("yyyy-MM-dd hh:mm:ss")}{this.Reason}";
        }
    }
}
