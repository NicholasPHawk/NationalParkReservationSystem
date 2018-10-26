using System;
using System.Collections.Generic;
using System.Text;
using Capstone.Models;
using Capstone.DAL;

namespace Capstone.CLI
{
    public class MainCLI
    {
        public string connectionString = @"Data Source =.\sqlexpress; Initial Catalog = NationalParkReservation; Integrated Security = True";

        public void RunMainCLI()
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
                        isDone = true;
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
                        Console.Clear();
                        Console.WriteLine("Your selection was invalid. Please try again.");
                        Console.WriteLine();
                    }
                }

                ParkCLI parkCLI = new ParkCLI(connectionString);
                parkCLI.RunParkCLI(parks[parkChoice - 1]);
            }
        }

        public void DisplayMainMenu(List<Park> parks)
        {
            int menuOption = 1;
            foreach (Park park in parks)
            {
                Console.WriteLine($"{menuOption})  {park.Name}");
                menuOption++;
            }

            Console.WriteLine("Q)  QUIT");
            Console.WriteLine();
            Console.WriteLine("Please choose a park");
            Console.WriteLine();
        }
    }
}