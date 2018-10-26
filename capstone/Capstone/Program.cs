using System;
using Capstone.CLI;

namespace Capstone
{
    public class Program
    {
        public static void Main(string[] args)
        {
           MainCLI mainCLI = new MainCLI();
            mainCLI.RunMainCLI();
        }
    }
}
