using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {
        private string connectionString;

        public ReservationSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
