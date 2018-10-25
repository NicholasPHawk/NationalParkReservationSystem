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

            do
            {
                int inputInt = 0;

                while (inputInt < 1 || inputInt > parks.Count)
                {
                    DisplayMainMenu();
                    string input = Console.ReadLine();
                    if (input.ToLower() == "q")
                    {
                        return;
                    }

                    try
                    {
                        inputInt = int.Parse(input);
                    }
                    catch
                    {

                    }

                    if (inputInt < 1 || inputInt > parks.Count)
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
                        if (inputInt == i + 1)
                        {
                            parkIndex = i;
                        }
                    }
                }
                catch(Exception ex)
                {

                }

                //DisplayParkInfo(parks[parkIndex]);
                ParkMenu(parks[parkIndex]);


            }
            while (!isDone);
        }

        public void DisplayMainMenu()
        {
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL(connectionString);
            List<Park> parks = parkSqlDAL.GetParks();

            for (int i = 0; i < parks.Count; i++)
            {
                Console.WriteLine($"{i + 1})  {parks[i].Name}");
            }
            Console.WriteLine("Q)  to QUIT");
            Console.WriteLine();
            Console.WriteLine("Please choose a park");
        }

        public void DisplayParkInfo(Park park)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(park.Name);
            Console.WriteLine($"Location:           {park.Location}");
            Console.WriteLine($"Established:        {park.EstablishDate}");
            Console.WriteLine($"Area:               {park.Area} sq km");
            Console.WriteLine($"Annual Visitors:   {park.Visitors.ToString("N")}");
            Console.WriteLine();
            Console.WriteLine(park.Description);
            Console.WriteLine();
            Console.WriteLine();
        }

        public void ParkMenu(Park park)
        {
            bool isDone = false;

            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL(connectionString);

            do
            {
                DisplayParkInfo(park);
                DisplayParkMenu();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        List<Campground> campgrounds = campgroundSqlDAL.GetCampgrounds(park);
                        DisplayCampgroundInfo(campgrounds);
                        CampgroundsMenu(park);
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

            } while (!isDone);
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
            Console.WriteLine($"     {"Name", -40}{ "Open", -10}{ "Close", -10}{"Daily Fee"}");
            int menuOption = 1;
            foreach (Campground campground in campgrounds)
            {         
                Console.WriteLine($"{menuOption})   {campground.ToString()}");
                menuOption++;
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public void CampgroundsMenu(Park park)
        {
            bool isDone = false;

            CampgroundSqlDAL campgroundSqlDAL = new CampgroundSqlDAL(connectionString);

            do
            {
                DisplayCampgroundsMenu();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        List<Campground> campgrounds = campgroundSqlDAL.GetCampgrounds(park);
                        DisplayCampgroundInfo(campgrounds);
                        CampgroundsMenu(park);
                        break;

                    case "2":
                        isDone = true;
                        break;

                    default:
                        Console.WriteLine("Input a valid selection.");
                        break;
                }

            } while (!isDone);
        }

        public void DisplayCampgroundsMenu()
        {
            Console.WriteLine("1)  Search for Available Reservation");
            Console.WriteLine("2)  Return to previous screen");
            Console.WriteLine();
        }
    }
}
