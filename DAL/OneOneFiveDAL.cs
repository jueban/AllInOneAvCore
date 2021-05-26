using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class OneOneFiveDAL : BaseRepository
    {
        public OneOneFiveDAL() : base("OneOneFive")
        {
        }

        public async Task<OneOneFiveCookie> GetOneOneFiveCookie()
        {
            var sql = @"SELECT TOP 1 * FROM OneOneFiveCookie WITH(NOLOCK) ORDER BY CreateTime DESC";

            return await QuerySingleOrDefaultAsync<OneOneFiveCookie>(sql);
        }

        public async Task<int> DeleteOneOneFiveCookie()
        {
            var sql = @"DELETE FROM OneOneFiveCookie";

            return await ExecuteAsync(sql);
        }

        public async Task<int> InsertOneOneFiveCookie(OneOneFiveCookie entity)
        {
            var sql = @"INSERT INTO OneOneFiveCookie (CookieJson, UserAgent, CreateTime) VALUES (@CookieJson, @UserAgent, GETDATE())";

            return await ExecuteAsync(sql, entity);
        }
    }
}
