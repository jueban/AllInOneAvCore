using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    public class SettingsDAL : BaseRepository
    {
        public SettingsDAL() : base("Settings")
        {
        }

        public async Task<Settings> GetAllSettings()
        {
            Settings ret = new Settings();
            var sql = @"SELECT TOP 1 * FROM Setting";

            var settings = await QuerySingleAsync<string>(sql);

            if (!string.IsNullOrEmpty(settings))
            {
                ret = JsonHelper.Deserialize<Settings>(settings);
            }

            return ret;
        }

        public async Task<int> UpdateSetting(string settings)
        {
            var sql = @"UPDATE Setting SET Settings = @settings, UpdateTime = GETDATE()";

            return await ExecuteAsync(sql, new { settings });
        }

        public async Task<int> InitSetting(string settings)
        {
            var sql = @"
                        TRUNCATE TABLE Setting;
                        INSERT INTO Setting (Settings, UpdateTime) VALUES (@settings, GETDATE())";

            return await ExecuteAsync(sql, new { settings });
        }

        public async Task<int> InsertPrefix(string prefix)
        {
            var sql = @"INSERT INTO Prefix (Prefix) VALUES (@prefix)";

            return await ExecuteAsync(sql, new { prefix });
        }

        public async Task<int> TruncatePrefix()
        {
            var sql = @"TRUNCATE TABLE Prefix ";

            return await ExecuteAsync(sql);
        }

        public async Task<List<string>> GetPrefix()
        {
            var sql = "SELECT * FROM Prefix";

            var str = await QuerySingleAsync<string>(sql);

            return str.Split(',').ToList();
        }

        public int InsertPlayHistory(PlayHistory entity)
        {
            var sql = @"
                        IF NOT EXISTS(SELECT TOP 1 * FROM PlayHistory WHERE FileName = @FileName)
                            BEGIN
                                INSERT INTO PlayHistory (FileName, PlayTimes, LastPlay, SetNotPlayed) VALUES (@FileName, @PlayTimes, GETDATE(), @SetNotPlayed)
                            END
                        ELSE
                            BEGIN
                                UPDATE PlayHistory SET PlayTimes = PlayTimes + 1, LastPlay = GETDATE(), SetNotPlayed = @SetNotPlayed WHERE FileName = @FileName
                            END;";

            return Execute(sql, entity);
        }

        public PlayHistory GetPlayHistory(string fileName)
        {
            var sql = "SELECT TOP 1 * FROM PlayHistory WHERE FileName = @fileName";

            return QuerySingleOrDefault<PlayHistory>(sql, new { fileName });
        }

        public int SetPlayHistoryNotPlayed(string fileName)
        {
            var sql = "UPDATE PlayHistory SET SetNotPlayed = 1 WHERE FileName = @fileName";

            return Execute(sql, new { fileName });
        }
    }
}
