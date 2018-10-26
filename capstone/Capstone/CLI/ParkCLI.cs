using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone.CLI
{
    public class ParkCLI
    {
        private string connectionString;

        public ParkCLI(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void RunParkCLI(Park park)
        {
            Console.Clear();

            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL(connectionString);
            List<Campground> campgrounds = campgroundSqlDAL.GetCampgrounds(park);

            bool isDone = false;

            while (!isDone)
            {
                Console.Clear();

                DisplayParkInfo(park);
                DisplayParkMenu();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CampgroundCLI campgroundCLI = new CampgroundCLI(connectionString);
                        campgroundCLI.RunCampgroundCLI(park, campgrounds);
                        break;

                    case "2":
                        ReservationCLI reservationCLI = new ReservationCLI(connectionString);
                        reservationCLI.RunReservationCLI(park, campgrounds);
                        break;

                    case "3":
                        Console.Clear();
                        isDone = true;
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Your selection was invalid. Please try again.");
                        Console.WriteLine();
                        break;
                }
            }
        }

        public void DisplayParkInfo(Park park)
        {
            Console.WriteLine(park.Name);
            Console.WriteLine();
            Console.WriteLine($"Location:           {park.Location}");
            Console.WriteLine($"Established:        {park.EstablishDate.ToString("MM/dd/yyyy")}");
            Console.WriteLine($"Area:               {park.Area.ToString("N0")} sq km");
            Console.WriteLine($"Annual Visitors:    {park.Visitors.ToString("N0")}");
            Console.WriteLine();
            Console.WriteLine(park.Description);
            Console.WriteLine();
        }

        public void DisplayParkMenu()
        {
            Console.WriteLine("1)  View Campgrounds");
            Console.WriteLine("2)  Search for Reservation");
            Console.WriteLine("3)  Return to Previous Screen");
            Console.WriteLine();
        }
    }
}
