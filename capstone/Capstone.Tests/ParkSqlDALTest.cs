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
    public class ParkSqlDALTest
    {
        private TransactionScope tran;

        const string connectionString = @"Data Source =.\sqlexpress; Initial Catalog = NationalParkReservation; Integrated Security = True";

        private int parkId = 0;
        private int parkCount = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                connection.Open();

                cmd = new SqlCommand("INSERT INTO park (name, location, establish_date, area, visitors, description) " +
                                     "VALUES ('Test Park', 'The Place', '10/26/2018', '5000', '450000', 'This is a test park'); " +
                                     "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                parkId = (int)cmd.ExecuteScalar();

                cmd = new SqlCommand("SELECT COUNT(*) FROM park;", connection);
                parkCount = (int)cmd.ExecuteScalar();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetParksTest()
        {
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL(connectionString);
            List<Park> parks = parkSqlDAL.GetParks();

            Assert.IsNotNull(parks);
            Assert.AreEqual(parks.Count, parkCount);
        }
    }
}
