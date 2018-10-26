using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone.CLI
{
    public class CampgroundCLI
    {
        private string connectionString;

        public CampgroundCLI(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void RunCampgroundCLI(Park park, List<Campground> campgrounds)
        {
            Console.Clear();

            bool isDone = false;

            while (!isDone)
            {
                DisplayCampgroundInfo(campgrounds);
                DisplayCampgroundMenu();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ReservationCLI reservationCLI = new ReservationCLI(connectionString);
                        reservationCLI.RunReservationCLI(park, campgrounds);
                        break;

                    case "2":
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

        public void DisplayCampgroundInfo(List<Campground> campgrounds)
        {
            Console.WriteLine($"     {"Name", -40}{"Open", -10}{"Close", -10}{"Daily Fee"}");

            int menuOption = 1;
            foreach (Campground campground in campgrounds)
            {
                Console.WriteLine($"{menuOption})   {campground.ToString()}");
                menuOption++;
            }

            Console.WriteLine();
        }

        public void DisplayCampgroundMenu()
        {
            Console.WriteLine("1)  Search for Reservation");
            Console.WriteLine("2)  Return to Previous Screen");
            Console.WriteLine();
        }
    }
}
