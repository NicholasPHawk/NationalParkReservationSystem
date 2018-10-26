using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {
        private string connectionString;

        private string SQL_checkSiteAvailability = "Select TOP 5 * FROM site s " +
                                                   "WHERE s.campground_id = @campground_id " +
                                                   "AND s.site_id NOT IN " +
                                                            "(SELECT site_id from reservation " +
                                                            "WHERE @from_date < to_date " +
                                                            "AND @to_date > from_date) " +
                                                   "ORDER BY s.site_id;";

        private string SQL_addReservation = "INSERT INTO reservation (site_id, name, from_date, to_date, create_date) " +
                                            "VALUES (@site_id, @name, @from_date, @to_date, @create_date);" +
                                            "SELECT CAST(SCOPE_IDENTITY() as int);";

        public ReservationSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Site> CheckSiteAvailability(int campgroundChoice, Reservation reservation)
        {
            List<Site> availableSites = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_checkSiteAvailability, conn);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundChoice);
                    cmd.Parameters.AddWithValue("@from_date", reservation.FromDate);
                    cmd.Parameters.AddWithValue("@to_date", reservation.ToDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.SiteId = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);

                        availableSites.Add(site);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return availableSites;
        }

        public int AddReservation(Reservation reservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_addReservation, conn);
                    cmd.Parameters.AddWithValue("@site_id", reservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", reservation.Name);
                    cmd.Parameters.AddWithValue("@from_date", reservation.FromDate);
                    cmd.Parameters.AddWithValue("@to_date", reservation.ToDate);
                    cmd.Parameters.AddWithValue("@create_date", DateTime.UtcNow);

                    reservation.ReservationId = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {

            }

            return reservation.ReservationId;            
        }
    }
}
