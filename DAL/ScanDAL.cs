using Models;
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

        public async Task<int> InsertFavi(WebScanUrlSite site, int type, string url, string name)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM ScanFavi WHERE Url = @url)
                            BEGIN
                                INSERT INTO ScanFavi (Site, Type, Url, name) VALUES (@site, @type, @url, @name);
                                SELECT @@IDENTITY;
                            END
                        ELSE
                            BEGIN
                                SELECT 0;
                            END";

            return await ExecuteAsync(sql, new { site, type, url, name });
        }

        public async Task<List<(WebScanUrlSite site, int type, string url, string name)>> GetFaviByWhere(string where)
        {
            var sql = @"SELECT * FROM ScanFavi WHERE 1 = 1 " + where;

            return await QueryAsync<(WebScanUrlSite, int, string, string)>(sql);
        }

        public async Task<int> SaveSeedMagnetSearchModel(ScanResult model)
        {
            var sql = @"INSERT INTO ScanResult (StartTime, WebSite, Url, Name, MagUrl) VALUES (@StartTime, @Site, @Url, @Name, @MagUrl);
                            SELECT @@IDENTITY;";

            return await QuerySingleOrDefaultAsync<int>(sql, model);
        }

        public async Task<int> UpdateSeedMagnetSearchModel(ScanResult model)
        {
            var sql = @"UPDATE ScanResult SET MagUrl = @MagUrl WHERE Id = @Id";

            return await ExecuteAsync(sql, model);
        }

        public async Task<List<ScanResult>> GetSeedMagnetSearchModelAll()
        {
            var sql = @"SELECT * FROM ScanResult";

            return await QueryAsync<ScanResult>(sql);
        }

        public async Task<ScanResult> GetSeedMagnetSearchModelById(int id)
        {
            var sql = @"SELECT * FROM ScanResult WHERE ID = @id";

            return await QuerySingleOrDefaultAsync<ScanResult>(sql, new { id });
        }

        public async Task<int> DeleteSeedMagnetSearchModelById(int id)
        {
            var sql = @"DELETE FROM ScanResult WHERE ID = @id";

            return await ExecuteAsync(sql, new { id });
        }
    }
}
