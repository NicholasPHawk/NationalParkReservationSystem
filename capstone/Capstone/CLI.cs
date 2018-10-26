using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone
{
    public class CLI
    {
        string connectionString = @"Data Source =.\sqlexpress; Initial Catalog = NationalParkReservation; Integrated Security = True";

        public void Run()
        {
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL(connectionString);
            List<Park> parks = parkSqlDAL.GetParks();

            bool isDone = false;

            while (!isDone)
            {
                int parkChoice = 0;

                while (parkChoice < 1 || parkChoice > parks.Count)
                {
                    DisplayMainMenu(parks);

                    string input = Console.ReadLine();

                    if (input.ToLower() == "q")
                    {
                        return;
                    }

                    try
                    {
                        parkChoice = int.Parse(input);
                    }

                    catch (Exception ex)
                    {
                    }

                    if (parkChoice < 1 || parkChoice > parks.Count)
                    {
                        Console.WriteLine("Input a valid selection.");
                        Console.WriteLine();
                    }
                }

                int parkIndex = 0;

                try
                {
                    for (int i = 0; i < parks.Count; i++)
                    {
                        if (parkChoice == i + 1)
                        {
                            parkIndex = i;
                        }
                    }
                }

                catch (Exception ex)
                {
                }

                ParkMenu(parks[parkIndex]);
            }
        }

        public void DisplayMainMenu(List<Park> parks)
        {
            for (int i = 0; i < parks.Count; i++)
            {
                Console.WriteLine($"{i + 1})  {parks[i].Name}");
            }
            Console.WriteLine("Q)  to QUIT");
            Console.WriteLine();
            Console.WriteLine("Please choose a park");
            Console.WriteLine();
        }

        public void ParkMenu(Park park)
        {
            bool isDone = false;

            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL(connectionString);

            while (!isDone)
            {
                DisplayParkInfo(park);
                DisplayParkMenu();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        List<Campground> campgrounds = campgroundSqlDAL.GetCampgrounds(park);
                        DisplayCampgroundInfo(campgrounds);
                        CampgroundMenu(park, campgrounds);
                        break;

                    case "2":

                        break;

                    case "3":
                        isDone = true;
                        break;

                    default:
                        Console.WriteLine("Input a valid selection.");
                        break;
                }
            }
        }

        public void DisplayParkInfo(Park park)
        {
            Console.WriteLine();
            Console.WriteLine(park.Name);
            Console.WriteLine($"Location:           {park.Location}");
            Console.WriteLine($"Established:        {park.EstablishDate}");
            Console.WriteLine($"Area:               {park.Area} sq km");
            Console.WriteLine($"Annual Visitors:    {park.Visitors.ToString("N")}");
            Console.WriteLine();
            Console.WriteLine(park.Description);
            Console.WriteLine();
            Console.WriteLine();
        }

        public void DisplayParkMenu()
        {
            Console.WriteLine("1)  View Campgrounds");
            Console.WriteLine("2)  Search for Reservation");
            Console.WriteLine("3)  Return to previous screen");
            Console.WriteLine();
        }

        public void DisplayCampgroundInfo(List<Campground> campgrounds)
        {
            Console.WriteLine();
            Console.WriteLine($"     {"Name",-40}{ "Open",-10}{ "Close",-10}{"Daily Fee"}");

            int menuOption = 1;
            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine($"{menuOption})   {campground.ToString()}");
                menuOption++;
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        public void CampgroundMenu(Park park, List<Campground> campgrounds)
        {
            bool isDone = false;

            while (!isDone)
            {
                DisplayCampgroundMenu();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Reservation reservation = SearchForReservation(park, campgrounds);
                        List<Site> sites = CheckAvailability(reservation);
                        Console.WriteLine(MakeReservation(sites));
                        break;

                    case "2":
                        isDone = true;
                        break;

                    default:
                        Console.WriteLine("Input a valid selection.");
                        break;
                }
            }
        }

        public void DisplayCampgroundMenu()
        {
            Console.WriteLine("1)  Search for Available Reservation");
            Console.WriteLine("2)  Return to previous screen");
            Console.WriteLine();
        }

        public Reservation SearchForReservation(Park park, List<Campground> campgrounds)
        {

            Console.WriteLine();
            Console.WriteLine("Which campground? Enter 0 to cancel");

            int campgroundChoice = 0;

            while (campgroundChoice < 1 || campgroundChoice > campgrounds.Count)
            {
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
                    return null;
                }

                if (campgroundChoice < 0 || campgroundChoice > campgrounds.Count)
                {
                    Console.WriteLine("Input a valid selection.");
                    DisplayCampgroundInfo(campgrounds);
                }
            }

            Console.WriteLine("What is the arrival date? MM/DD/YYYY");

            DateTime arrivalDate = Convert.ToDateTime(Console.ReadLine().Replace('/', '-'));

            Console.WriteLine("What is the departure date? MM/DD/YYYY");

            DateTime departureDate = Convert.ToDateTime(Console.ReadLine().Replace('/', '-'));

            Reservation reservation = new Reservation();
            reservation.FromDate = arrivalDate;
            reservation.ToDate = departureDate;
            return reservation;
        }

        public List<Site> CheckAvailability(Reservation reservation)
        {
            ReservationSqlDAL reservationSqlDAL = new ReservationSqlDAL(connectionString);
           // List<Site> sites = reservationSqlDAL.   (); // call method to search reservation table           

            return null;
        }

        public string MakeReservation(List<Site> sites)
        {
            Console.WriteLine("");

            return "";
            // ask for reservation
            // add info to reservation object
            // call method to add reservation to table
        }
    }
}