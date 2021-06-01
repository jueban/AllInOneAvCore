using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Services
{
    public class UtilityService
    {
        public static string RecordCarPlate(string plate, string reason)
        {
            var pattern = @"([京津晋冀蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新][ABCDEFGHJKLMNPQRSTUVWXY][1-9DF][1-9ABCDEFGHJKLMNPQRSTUVWXYZ]\d{3}[1-9DF]|[京津晋冀蒙辽吉黑沪苏浙皖闽赣鲁豫鄂湘粤桂琼渝川贵云藏陕甘青宁新][ABCDEFGHJKLMNPQRSTUVWXY][\dABCDEFGHJKLNMxPQRSTUVWXYZ]{5})";
            var ret = "";

            LogHelper.Info($"Plate -> {plate} Reason -> {reason}");


            plate = plate.Replace(" ", "").Replace(".", "").Replace("。", "");

            if (Regex.IsMatch(plate, pattern))
            {
                var business = new UtilityDAL();
                var id = business.RecordCarPlate(plate, reason);
                var record = business.GetRecordCarPlate(id);

                ret = record.ToString();
            }
            else
            {
                ret = "无效的车牌号";
            }

            return ret;
        }
    }
}
