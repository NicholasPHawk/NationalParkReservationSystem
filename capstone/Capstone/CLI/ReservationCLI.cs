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

            int campgroundChoice = -1;

            while (campgroundChoice < 1 || campgroundChoice > campgrounds.Count)
            {
                campgroundCLI.DisplayCampgroundInfo(campgrounds);

                Console.WriteLine("Which campground? Enter 0 to cancel and return to the previous menu.");
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

            Reservation reservation = new Reservation();

            List<Site> sites = new List<Site>();

            bool sitesAvailable = false;

            while (!sitesAvailable)
            {
                Console.WriteLine();
                Console.WriteLine("What is the arrival date? MM/DD/YYYY");
                Console.WriteLine();

                reservation.FromDate = Convert.ToDateTime(Console.ReadLine().Replace('/', '-'));

                Console.WriteLine();
                Console.WriteLine("What is the departure date? MM/DD/YYYY");
                Console.WriteLine();

                reservation.ToDate = Convert.ToDateTime(Console.ReadLine().Replace('/', '-'));

                sites = CheckSiteAvailability(campgrounds[campgroundChoice - 1].CampgroundId, reservation);

                if (sites.Count > 0)
                {
                    sitesAvailable = true;
                }

                else
                {
                    Console.WriteLine();
                    Console.WriteLine("There are no available sites for those dates.");
                    Console.WriteLine("Would you like to enter alternate dates? Enter (Y)es or (N)o.");
                    Console.WriteLine();

                    bool checkAlternateDates = false;

                    while (!checkAlternateDates)
                    {
                        string input = Console.ReadLine();

                        switch (input.ToLower())
                        {
                            case "y":
                                checkAlternateDates = true;
                                break;

                            case "n":
                                Console.Clear();
                                return;

                            default:
                                Console.WriteLine();
                                Console.WriteLine("Your selection was invalid. Please try again.");
                                Console.WriteLine();
                                break;
                        }
                    }
                }
            }

            DisplayAvailableSites(sites, reservation, campgrounds[campgroundChoice - 1].DailyFee);

            if (!MakeReservation(sites, reservation))
            {
                Console.Clear();
                return;
            }

            DisplayReservationId(reservation);
        }


        public List<Site> CheckSiteAvailability(int campgroundId, Reservation reservation)
        {
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);
            List<Site> sites = reservationSqlDAL.CheckSiteAvailability(campgroundId, reservation);

            return sites;
        }

        public void DisplayAvailableSites(List<Site> sites, Reservation reservation, decimal cost)
        {
            Console.WriteLine();
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine();

            Console.WriteLine($"      {"Site No.",-10}{"Max Occup.",-15}{"Accessible",-15}" +
                $"{"Max RV Length",-20}{"Utilities",-15}{"Cost"}");

            System.TimeSpan reservationLength = reservation.ToDate - reservation.FromDate;

            int menuOption = 1;
            foreach (Site site in sites)
            {
                Console.WriteLine($"{menuOption})    {site.ToString()}{(cost * reservationLength.Days).ToString("C2")}");
                menuOption++;
            }

            Console.WriteLine();
        }

        public bool MakeReservation(List<Site> sites, Reservation reservation)
        {
            bool reservationIsSuccessful = false;

            int siteChoice = -1;

            while (siteChoice < 1 || siteChoice > sites.Count)
            {
                Console.WriteLine("Which site should be reserved? Enter 0 to cancel and return to the previous menu.");
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
                    return reservationIsSuccessful;
                }

                if (siteChoice < 0 || siteChoice > sites.Count)
                {
                    Console.WriteLine();
                    Console.WriteLine("Your selection was invalid. Please try again.");
                    Console.WriteLine();
                }
            }

            reservation.SiteId = sites[siteChoice - 1].SiteId;

            Console.WriteLine();
            Console.WriteLine("What name should the reservation be made under?");
            Console.WriteLine();

            reservation.Name = Console.ReadLine();

            reservationIsSuccessful = true;
            return reservationIsSuccessful;
        }

        public void DisplayReservationId(Reservation reservation)
        {
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);

            Console.WriteLine();
            Console.WriteLine($"Your reservation has been made and the confirmation id is {reservationSqlDAL.AddReservation(reservation)}.");
            Console.WriteLine("Press enter to return to the previous menu.");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
