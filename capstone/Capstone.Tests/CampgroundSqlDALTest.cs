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
    public class CampgroundSqlDALTest
    {
        private TransactionScope tran;

        const string connectionString = @"Data Source =.\sqlexpress; Initial Catalog = NationalParkReservation; Integrated Security = True";

        private int campgroundId = 0;
        private int campgroundCount = 0;
        Park park = new Park();

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) " +
                                     "VALUES ('Test Park', 'The Place', '10/26/2018', '5000', '450000', 'This is a test park'); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                park.ParkId = (int)cmd.ExecuteScalar();


                cmd = new SqlCommand("INSERT INTO campground (park_id, name, open_from_mm, open_to_mm, daily_fee) " +
                                     "VALUES (@park_id, 'Test Campground', '05', '11', '20.00'); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                cmd.Parameters.AddWithValue("@park_id", park.ParkId);
                campgroundId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand(@"SELECT COUNT(*) FROM campground " +
                                     "WHERE park_id = @park_id;" , connection);
                cmd.Parameters.AddWithValue("@park_id", park.ParkId);
                campgroundCount = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetCampgroundsTest()
        {
            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL(connectionString);
            List<Campground> campgrounds = campgroundSqlDAL.GetCampgrounds(park);

            Assert.IsNotNull(campgrounds);
            Assert.AreEqual(campgrounds.Count, campgroundCount);
        }
    }
}
