using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Androtomist.Models.Database
{
    public class DatabaseProcedures
    {
        private readonly NpgsqlConnection conn;
        private NpgsqlCommand pgCommand;

        public DatabaseProcedures(ref NpgsqlConnection conn)
        {
            this.conn = conn;
        }
    }
}
