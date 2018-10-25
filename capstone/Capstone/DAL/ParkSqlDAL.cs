using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;


namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private string connectionString;

        private string SQL_GetParks = "SELECT * FROM park;";

        public ParkSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Park> GetParks()
        {
            List<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetParks, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())                    
                    {
                        Park park = new Park();
                        park.ParkId = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);

                        parks.Add(park);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return parks;
        }
    }
}
