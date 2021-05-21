using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ScanDAL : BaseRepository
    {
        public ScanDAL() : base("Scan")
        {
        }

        public async Task<int> InsertFavi(WebScanUrlSite site, string type, string url)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM ScanFavi WHERE Url = @url)
                            BEGIN
                                INSERT INTO ScanFavi (Site, Type, Url) VALUES (@site, @type, @url);
                                SELECT @@IDENTITY;
                            END
                        ELSE
                            BEGIN
                                SELECT 0;
                            END";

            return await ExecuteAsync(sql, new { site, type, url });
        }

        public async Task<List<(WebScanUrlSite site, string type, string url)>> GetFaviByWhere(string where)
        {
            var sql = @"SELECT * FROM ScanFavi WHERE 1 = 1 " + where;

            return await QueryAsync<(WebScanUrlSite, string, string)>(sql);
        }
    }
}
