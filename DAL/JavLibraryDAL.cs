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
        public async Task<int> InsertCommonJavLibraryModel(CommonModel entity)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Name = @Name AND Url = @Url AND Type = @Type)
                            INSERT INTO CommonJavLibraryModel (Name, Url, Type) VALUES (@Name, @url, @Type)";

            return await ExecuteAsync(sql, entity);
        }

        public async Task<CommonModel> GetCommonJavLibraryModelById(int id)
        {
            var sql = @"SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Id = @id";

            return await QuerySingleOrDefaultAsync<CommonModel>(sql,  new { id });
        }

        public async Task<List<CommonModel>> GetCommonJavLibraryModelByType(CommonModelType type)
        {
            var sql = @"SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Type = @type";

            return await QueryAsync<CommonModel>(sql, new { type });
        }

        public async Task<List<CommonModel>> GetCommonJavLibraryModelByName(string name)
        {
            var sql = @"SELECT * FROM CommonJavLibraryModel WITH(NOLOCK) WHERE Name = @name";

            return await QueryAsync<CommonModel>(sql, new { name });
        }

        public async Task<int> InsertWebScanUrlModel(WebScanUrlModel entity)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM ScanUrl WITH(NOLOCK) WHERE Url = @Url)
                            BEGIN
                                INSERT INTO ScanUrl (AvId, Name, Url, IsDownload, CreateTime, UpdateTime) 
                                    VALUES (@AvId, @Name, @Url, @IsDownload, GETDATE(), GETDATE());
                                SELECT @@IDENTITY;
                            END
                        ELSE
                            BEGIN
                                SELECT Id From ScanUrl WITH(NOLOCK) WHERE Url = @Url;
                            END";

            return await QuerySingleOrDefaultAsync<int>(sql, entity);
        }

        public async Task<List<WebScanUrlModel>> GetWebScanUrlModel(bool onlyNotDownload)
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

        public async Task<int> InsertAvModel(AvModel entity)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM AvModel WHERE Url = @Url)
                            BEGIN
                                INSERT INTO AvModel (AvId, Name, Url, PicUrl, Infos, FileNameWithoutExtension, AvLength, ReleaseDate, CreateTime, UpdateTime)
                                    VALUES(@AvId, @Name, @Url, @PicUrl, @Infos, @FileNameWithoutExtension, @AvLength, @ReleaseDate, GETDATE(), GETDATE());
                                SELECT @@IDENTITY;
                            END
                        ELSE
                            BEGIN
                                SELECT 0;
                            END";

            return await QuerySingleOrDefaultAsync<int>(sql, entity);
        }

        public async Task<AvModel> GetAvModelById(int id)
        {
            var sql = @"SELECT * FROM AvModel WHERE Id = @id";

            return await QuerySingleAsync<AvModel>(sql, new { id });
        }

        public async Task<List<AvModel>> GetAvModelByWhere(string where)
        {
            var sql = @"SELECT * FROM AvModel WHERE 1 = 1 " + where;

            return await QueryAsync<AvModel>(sql);
        }

        public async Task<int> DeleteAvMapping(int avId)
        {
            var sql = @"DELETE FROM AvMapping WHERE AvId = @avId";

            return await ExecuteAsync(sql, new { avId });
        }

        public async Task<int> InsertAvMapping(int avId, int commonId, CommonModelType typeId)
        {
            var sql = @"INSERT INTO AvMapping (AvId, CommonId, CommonType) VALUES (@avId, @commonId, @typeId)";

            return await ExecuteAsync(sql, new { avId, commonId, typeId });
        }
        #endregion
    }
}