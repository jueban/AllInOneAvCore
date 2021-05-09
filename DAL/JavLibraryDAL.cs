using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public class JavLibraryDAL : BaseRepository
    {
        public JavLibraryDAL() : base("JavLibrary")
        {
        }

        #region Cookie
        public async Task<JavLibraryCookieJson> GetJavLibraryCookie()
        {
            var sql = @"SELECT TOP 1 * FROM JavLibraryCookie WITH(NOLOCK) ORDER BY CreateTime DESC";

            return await QuerySingleOrDefaultAsync<JavLibraryCookieJson>(sql);
        }

        public async Task<int> DeleteJavLibraryCookie()
        {
            var sql = @"DELETE FROM JavLibraryCookie";

            return await ExecuteAsync(sql);
        }

        public async Task<int> InsertJavLibraryCookie(JavLibraryCookieJson entity)
        {
            var sql = @"INSERT INTO JavLibraryCookie (CookieJson, UserAgent, CreateTime) VALUES (@CookieJson, @UserAgent, GETDATE())";

            return await ExecuteAsync(sql, entity);
        }
        #endregion

        #region 网页
        public async Task<int> InsertCommonJavLibraryModel(CommonJavLibraryModel entity)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Name = @Name AND Url = @Url AND Type = @Type)
                            INSERT INTO CommonJavLibraryModel (Name, Url, Type) VALUES (@Name, @url, @Type)";

            return await ExecuteAsync(sql, entity);
        }

        public async Task<CommonJavLibraryModel> GetCommonJavLibraryModelById(int id)
        {
            var sql = @"SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Id = @id";

            return await QuerySingleOrDefaultAsync<CommonJavLibraryModel>(sql,  new { id });
        }

        public async Task<List<CommonJavLibraryModel>> GetCommonJavLibraryModelByType(CommonJavLibraryModelType type)
        {
            var sql = @"SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Type = @type";

            return await QueryAsync<CommonJavLibraryModel>(sql, new { type });
        }

        public async Task<List<CommonJavLibraryModel>> GetCommonJavLibraryModelByName(string name)
        {
            var sql = @"SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Name = @name";

            return await QueryAsync<CommonJavLibraryModel>(sql, new { name });
        }

        public async Task<int> InsertWebScanUrlModel(WebScanUrlModel entity)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM ScanUrl WITH(NOLOCK) WHERE Url = @Url)
                            INSERT INTO ScanUrl (Name, Url, IsDownload, CreateTime, UpdateTime) VALUES (@Name, @Url, @IsDownload, GETDATE(), GETDATE())";

            return await ExecuteAsync(sql, entity);
        }

        public async Task<List<WebScanUrlModel>> GetWebScanUrlModel(bool onlyNotDownload = true)
        {
            string where = "";

            if (onlyNotDownload)
            {
                where += " AND IsDownload = 0";
            }

            var sql = $"SELECT * FROM ScanUrl WITH(NOLOCK) WHERE 1 = 1 {where}";

            return await QueryAsync<WebScanUrlModel>(sql);
        }

        public async Task<int> UpdateWebScanUrlModel(int id, bool state)
        {
            var sql = @"UPDATE ScanUrl SET IsDownload = @state WHERE Id = @id";

            return await ExecuteAsync(sql, new { id, state });
        }
        #endregion
    }
}