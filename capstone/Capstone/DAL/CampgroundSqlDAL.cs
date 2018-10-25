using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string connectionString;

        private string SQL_GetCampgrounds = "SELECT * FROM campground WHERE park_id = @park_id;";

        public CampgroundSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Campground> GetCampgrounds(Park park)
        {
            List<Campground> campgrounds = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCampgrounds, conn);

                    cmd.Parameters.AddWithValue("@park_id", park.ParkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground campground = new Campground();
                        campground.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        campground.ParkId = Convert.ToInt32(reader["park_id"]);
                        campground.Name = Convert.ToString(reader["name"]);
                        campground.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
                        campground.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
                        campground.DailyFee = Convert.ToDecimal(reader["daily_fee"]);

                        campgrounds.Add(campground);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return campgrounds;
        }
    }
}

