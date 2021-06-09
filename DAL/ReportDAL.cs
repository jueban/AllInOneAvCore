using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ReportDAL : BaseRepository
    {
        public ReportDAL() : base("Report")
        {
        }

        public int InsertReport(Report entity)
        {
            var sql = @"INSERT INTO Report (ReportDate,TotalCount,TotalExist,TotalExistSize,LessThenOneGiga,OneGigaToTwo,TwoGigaToFour,FourGigaToSix,GreaterThenSixGiga,Extension,H265Count,ChineseCount,IsFinish,EndDate) 
                            VALUES (GETDATE(), @TotalCount, @TotalExist, @TotalExistSize, @LessThenOneGiga, @OneGigaToTwo, @TwoGigaToFour, @FourGigaToSix, @GreaterThenSixGiga, @ExtensionJson, @H265Count, @ChineseCount, @IsFinish, GETDATE()) SELECT @@IDENTITY";

            return QuerySingle<int>(sql, entity);
        }

        public int UpdateReport(Report entity)
        {
            var sql = "UPDATE Report SET TotalExist = @TotalExist, TotalExistSize = @TotalExistSize, LessThenOneGiga = @LessThenOneGiga, OneGigaToTwo = @OneGigaToTwo, TwoGigaToFour = @TwoGigaToFour, FourGigaToSix = @FourGigaToSix, GreaterThenSixGiga = @GreaterThenSixGiga, Extension = @ExtensionJson, H265Count = @H265Count, ChineseCount = @ChineseCount WHERE ReportId = @ReportId";

            return Execute(sql, entity);
        }

        public int UpdateReportFinish(int id)
        {
            var sql = "UPDATE Report SET IsFinish = 1, EndDate = GETDATE() WHERE ReportID = @id";

            return Execute(sql, new { id });
        }

        public List<Report> GetReports()
        {
            var sql = "SELECT * FROM Report WHERE IsFinish = 1 ORDER BY EndDate DESC";

            return Query<Report>(sql);
        }

        public Report GetReport(int id)
        {
            var sql = "SELECT * FROM Report WHERE ReportId = @id";

            return QuerySingle<Report>(sql, new { id });
        }

        public List<ReportItem> ReportItem(int reportId)
        {
            var sql = "SELECT * FROM ReportItem WHERE ReportId = @reportId";

            return Query<ReportItem>(sql, new { reportId });
        }

        public int BatchInserReportItem(List<ReportItem> items)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ReportItemId", typeof(int));
            dt.Columns.Add("ReportType", typeof(int));
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("ExistCount", typeof(int));
            dt.Columns.Add("TotalCount", typeof(int));
            dt.Columns.Add("TotalSize", typeof(double));
            dt.Columns.Add("ReportId", typeof(int));

            foreach (var item in items)
            {
                dt.Rows.Add(null, (int)item.ReportType, item.ItemName, item.ExistCount, item.TotalCount, item.TotalSize, item.ReportId);
            }

            BatchInsert(dt);

            return items.Count;
        }
    }
}
