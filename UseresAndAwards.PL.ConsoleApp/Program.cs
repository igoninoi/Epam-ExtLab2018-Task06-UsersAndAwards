using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseresAndAwards.PL.ConsoleApp
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            do
            {
                Console.WriteLine("\n--- Main ---");
                Console.WriteLine("   u[ser]  - Work with users");
                Console.WriteLine("   a[ward] - Work with awards");
                Console.WriteLine("   l[ink]  - Link users and awards");
                Console.WriteLine("   j[oin]  - Show users and joined awards");
                Console.WriteLine("   e[exit] - Exit ");
                Console.Write("   Command: ");
            }
            while (WorkWithEntities(Console.ReadLine()));

            Console.WriteLine("\nwork finished");
        }

        private static bool WorkWithEntities(string choice)
        {
            if (choice == null)
            {
                throw new NullReferenceException();
            }

            choice = choice.Trim().ToLower();
            switch (choice)
            {
                case "u":
                case "user":
                    UsersCommand.Run();
                    return true;

                case "a":
                case "award":
                    AwardsCommand.Run();
                    return true;

                case "l":
                case "link":
                    LinkUserAwardCommand.Run();
                    return true;

                case "j":
                case "join":
                    JoinUserAwardCommand.Run();
                    return true;

                case "e":
                case "exit":
                    return false;

                default:
                    Console.WriteLine($"Unknown command '{choice}'");
                    return true;
            }
        }
    }
}
