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

        private int reservationId = 0;
        private int reservationCount = 0;

        [TestInitialize]
        public void Initialize()
        {
            tran = new TransactionScope();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd;

                    cmd = new SqlCommand("INSERT INTO reservation (site_id, name, from_date, to_date) " +
                                        "VALUES (29, 'Test Reservation', '10/26/2018', '10/27/2018'); " +
                                         "SELECT CAST(SCOPE_IDENTITY() as int);", connection);
                    reservationId = (int)cmd.ExecuteScalar();
                }
            }
            catch(Exception ex)
            {

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
            newReservation.ToDate = new DateTime(2018, 07, 09);

            List<Site> sites = reservationSqlDAL.CheckSiteAvailability(3, newReservation);

            Assert.IsTrue(sites.Count <= 5);
        }

        [TestMethod]
        public void AddReservationTest()
        {

                ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);
                Reservation newReservation = new Reservation();
                newReservation.SiteId = 30;
                newReservation.Name = "Jim Jones";
                newReservation.FromDate = new DateTime(2018, 07, 03);
                newReservation.ToDate = new DateTime(2018, 07, 10);
                int result = reservationSqlDAL.AddReservation(newReservation);
                bool didWork = false;
            try
            {
                if (result > 0)
                {
                    didWork = true;
                }
                else
                {
                    didWork = false;
                }
            }
            catch (Exception ex)
            {

            }
            Assert.IsTrue(didWork);
        }
    }
}
