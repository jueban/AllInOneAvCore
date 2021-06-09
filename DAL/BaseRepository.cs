using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;

namespace DAL
{
    public class BaseRepository
    {
        private string ConnectionString { get; set; }

        public BaseRepository(string db)
        {
            ConnectionString = @$"Server=localhost\SQLEXPRESS;Database={db};User=sa;password=pa$$w0rd;";
        }

        private IDbConnection NewConnection() => new SqlConnection(ConnectionString);

    #region async
        protected async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
            }
        }

        protected async Task<T> QuerySingleAsync<T>(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                return await conn.QuerySingleAsync<T>(sql, param);
            }
        }

        protected async Task<T> QueryFirstAsync<T>(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                return await conn.QueryFirstAsync<T>(sql, param);
            }
        }

        protected async Task<List<T>> QueryAsync<T>(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                var result = await conn.QueryAsync<T>(sql, param);

                return result.ToList();
            }
        }

        protected async Task<int> ExecuteAsync(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                return await conn.ExecuteAsync(sql, param);
            }
        }

        protected async Task QueryMultipleAsync(string sql, Action<SqlMapper.GridReader> map, object param = null)
        {
            using (var conn = NewConnection())
            {
                var result = await conn.QueryMultipleAsync(sql, param);

                map(result);
            }
        }
        #endregion

    #region sync
        protected T QuerySingleOrDefault<T>(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                return conn.QuerySingleOrDefault<T>(sql, param);
            }
        }

        protected T QuerySingle<T>(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                return conn.QuerySingle<T>(sql, param);
            }
        }

        protected List<T> Query<T>(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                var result = conn.Query<T>(sql, param);

                return result.ToList();
            }
        }

        protected int Execute(string sql, object param = null)
        {
            using (var conn = NewConnection())
            {
                return conn.Execute(sql, param);
            }
        }

        protected void QueryMultiple(string sql, Action<SqlMapper.GridReader> map, object param = null)
        {
            using (var conn = NewConnection())
            {
                var reader = conn.QueryMultiple(sql, param);
                map(reader);
            }
        }

        protected void BatchInsert(DataTable dt)
        {
            using (SqlConnection conn = (SqlConnection)NewConnection())
            {
                SqlBulkCopy bulkCopy = new(conn);
                bulkCopy.DestinationTableName = "ReportItem";
                bulkCopy.BatchSize = dt.Rows.Count;
                conn.Open();
                if (dt != null && dt.Rows.Count != 0)
                {
                    bulkCopy.WriteToServer(dt);
                }
            }
        }
        #endregion
    }
}
