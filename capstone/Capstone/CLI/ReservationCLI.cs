using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone.CLI
{
    public class ReservationCLI
    {
        private string connectionString;

        public ReservationCLI(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void RunReservationCLI(Park park, List<Campground> campgrounds)
        {
            Console.Clear();

            CampgroundCLI campgroundCLI = new CampgroundCLI(connectionString);

            int campgroundChoice = 0;

            while (campgroundChoice < 1 || campgroundChoice > campgrounds.Count)
            {
                campgroundCLI.DisplayCampgroundInfo(campgrounds);
                Console.WriteLine("Which campground? Enter 0 to cancel");
                Console.WriteLine();

                string input = Console.ReadLine();

                try
                {
                    campgroundChoice = int.Parse(input);
                }

                catch (Exception ex)
                {
                }

                if (campgroundChoice == 0)
                {
                    Console.Clear();
                    return;
                }

                if (campgroundChoice < 0 || campgroundChoice > campgrounds.Count)
                {
                    Console.Clear();
                    Console.WriteLine("Your selection was invalid. Please try again.");
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine("What is the arrival date? MM/DD/YYYY");
            Console.WriteLine();

            DateTime arrivalDate = Convert.ToDateTime(Console.ReadLine().Replace('/', '-'));

            Console.WriteLine();
            Console.WriteLine("What is the departure date? MM/DD/YYYY");
            Console.WriteLine();

            DateTime departureDate = Convert.ToDateTime(Console.ReadLine().Replace('/', '-'));

            Reservation reservation = new Reservation();
            reservation.FromDate = arrivalDate;
            reservation.ToDate = departureDate;

            List<Site> sites = CheckSiteAvailability(campgrounds[campgroundChoice - 1].CampgroundId, reservation);

            DisplayAvailableSites(sites, campgrounds[campgroundChoice - 1].DailyFee, reservation);

            MakeReservation(sites, reservation);

            Console.WriteLine(AddReservation(reservation));
            Console.WriteLine();
            Console.WriteLine("Press enter to return to the previous menu.");
            Console.ReadLine();
        }

        public List<Site> CheckSiteAvailability(int campgroundId, Reservation reservation)
        {
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);
            List<Site> sites = reservationSqlDAL.CheckSiteAvailability(campgroundId, reservation);

            return sites;
        }

        public void DisplayAvailableSites(List<Site> sites, decimal cost, Reservation reservation)
        {
            Console.WriteLine();
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine();

            Console.WriteLine($"{"Site No.",-10}{"Max Occup.",-10}{"Accessible",-10}" +
                $"{"RV Length",-10}{"Utilities",-10}{"Cost"}");

            System.TimeSpan reservationLength = reservation.ToDate - reservation.FromDate;

            int menuOption = 1;

            while (menuOption < 5)
            {
                foreach (Site site in sites)
                {
                    Console.WriteLine($"{menuOption})   {site.ToString()}{(cost * reservationLength.Days).ToString("C2")}");
                    menuOption++;
                }
            }

            Console.WriteLine();
        }

        public void MakeReservation(List<Site> sites, Reservation reservation)
        {
            int siteChoice = 0;

            while (siteChoice < 1 || siteChoice > sites.Count)
            {
                Console.WriteLine("Which site should be reserved? Enter 0 to cancel");
                Console.WriteLine();

                string input = Console.ReadLine();

                try
                {
                    siteChoice = int.Parse(input);
                }

                catch (Exception ex)
                {
                }

                if (siteChoice == 0)
                {
                    return;
                }

                if (siteChoice < 0 || siteChoice > sites.Count)
                {
                    Console.WriteLine("Your selection was invalid. Please try again.");
                    Console.WriteLine();
                }
            }

            reservation.SiteId = sites[siteChoice - 1].SiteId;

            Console.WriteLine();
            Console.WriteLine("What name should the reservation be made under?");
            Console.WriteLine();

            reservation.Name = Console.ReadLine();

            reservation.CreateDate = DateTime.UtcNow;
        }

        public string AddReservation(Reservation reservation)
        {
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);

            return $"Your reservation has been made and the confirmation id is {reservationSqlDAL.AddReservation(reservation)}";
        }
    }
}
