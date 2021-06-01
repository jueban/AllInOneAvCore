using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UtilityDAL : BaseRepository
    {
        public UtilityDAL() : base("Utility")
        {
        }

        public int RecordCarPlate(string plate, string reason)
        {
            var sql = @"IF NOT EXISTS (SELECT * FROM RecordCarPlate WHERE Plate = @plate)
                                BEGIN                                
                                    INSERT INTO RecordCarPlate (Plate, Reason, CreateTime) VALUES (@plate, @reason, GETDATE());
                                    SELECT @@IDENTITY;
                                END
                            ELSE
                                BEGIN
                                    SELECT Id FROM RecordCarPlate WHERE Plate = @plate;
                                END";

            return QuerySingle<int>(sql, new { plate, reason });
        }

        public RecordCarPlate GetRecordCarPlate(int id)
        {
            var sql = "SELECT * FROM RecordCarPlate WHERE Id = @id";

            return QuerySingle<RecordCarPlate>(sql, new { id });
        }
    }
}
