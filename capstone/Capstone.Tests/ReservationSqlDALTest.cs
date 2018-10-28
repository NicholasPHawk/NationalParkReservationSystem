using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using System.Collections.Generic;
using System;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationSqlDALTest
    {
        private TransactionScope tran;

        const string connectionString = @"Data Source =.\sqlexpress; Initial Catalog = NationalParkReservation; Integrated Security = True";

        private int parkId = 0;
        private int campgroundId = 0;
        private int siteId = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) " +
                     "VALUES ('Test Park', 'The Place', '10/26/2018', '5000', '450000', 'This is a test park'); " +
                     "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                parkId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) " +
                     "VALUES (@park_id, 'Test Campground', '05', '11', '20.00'); " +
                     "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                cmd.Parameters.AddWithValue("@park_id", parkId);
                campgroundId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)" +
                    "VALUES (@campground_id, '1', '6', 1, '20', 1);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                siteId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date) " +
                                    "VALUES (@site_id, 'Test Reservation', '07/01/2018', '07/04/2018'); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                cmd.Parameters.AddWithValue("@site_id", siteId);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void CheckSiteAvailabilityTest()
        {
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);

            Reservation newReservation = new Reservation();
            newReservation.FromDate = new DateTime(2018, 07, 03);
            newReservation.ToDate = new DateTime(2018, 07, 10);

            List<Site> sites = reservationSqlDAL.CheckSiteAvailability(campgroundId, newReservation);

            Assert.AreEqual(0, sites.Count);

            newReservation.FromDate = new DateTime(2018, 07, 04);

            sites = reservationSqlDAL.CheckSiteAvailability(campgroundId, newReservation);

            Assert.AreEqual(1, sites.Count);
        }

        [TestMethod]
        public void AddReservationTest()
        {
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);

            Reservation newReservation = new Reservation();
            newReservation.SiteId = siteId;
            newReservation.Name = "Jim Jones";
            newReservation.FromDate = new DateTime(2018, 07, 04);
            newReservation.ToDate = new DateTime(2018, 07, 10);
            
            int result = reservationSqlDAL.AddReservation(newReservation);
            
            Assert.IsTrue(result > 0);
        }
    }
}
