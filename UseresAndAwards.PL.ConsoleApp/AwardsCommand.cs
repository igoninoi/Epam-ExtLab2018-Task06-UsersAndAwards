using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.BLL.Contracts;
using UseresAndAwards.Entities;

namespace UseresAndAwards.PL.ConsoleApp
{
    internal static class AwardsCommand
    {
        private static IEntityLogic<Award> Logic => BLL.Core.AwardLogic.GetInstance;

        public static void Run()
        {
            do
            {
                Console.WriteLine("\n--- Awards ---");
                Console.WriteLine("   a[dd]   - Add award");
                Console.WriteLine("   l[ist]  - Show all awards");
                Console.WriteLine("   d[del]  - Delete award");
                Console.WriteLine("   e[exit] - Exit ");
                Console.Write("   Command: ");
            }
            while (WorkWithAwards(Console.ReadLine()));

            Console.WriteLine("\nfinished (Awards)");
        }

        private static bool WorkWithAwards(string choice)
        {
            if (choice == null)
            {
                throw new NullReferenceException();
            }

            choice = choice.Trim().ToLower();
            switch (choice)
            {
                case "a":
                case "add":
                    ExecuteAwardsAddCommand();
                    return true;

                case "l":
                case "list":
                    ExecuteAwardsListCommand();
                    return true;

                case "d":
                case "del":
                    ExecuteAwardsDelCommand();
                    return true;

                case "e":
                case "exit":
                    return false;

                default:
                    Console.WriteLine($"Unknown command '{choice}'");
                    return true;
            }
        }

        private static void ExecuteAwardsAddCommand()
        {
            Console.WriteLine("\n--- Awards > Add award ---");

            string title;
            do
            {
                Console.Write("   Award title: ");
                title = Console.ReadLine().Trim();
            }
            while (title == string.Empty);

            Award award = new Award() { Title = title };

            string errorMessage;
            if (Logic.Add(award, out errorMessage))
            {
                Console.WriteLine("Award added");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }

        private static void ExecuteAwardsListCommand()
        {
            Console.WriteLine("\n--- Awards > List awards ---");

            IEnumerable<KeyValuePair<Guid, Award>> collection;
            string errorMessage;
            if (Logic.GetAll(out collection, out errorMessage))
            {
                foreach (var item in collection)
                {
                    var award = item.Value;
                    Console.WriteLine($"ID:{item.Key}, Title:{award.Title}");
                }

                Console.WriteLine("--- end of list ---");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }

        private static void ExecuteAwardsDelCommand()
        {
            Console.WriteLine("\n--- Awards > Delete award ---");

            Guid id;
            string input;
            do
            {
                Console.Write("   Award ID: ");
                input = Console.ReadLine().Trim();
            }
            while (!Guid.TryParse(input, out id));

            string errorMessage;
            if (Logic.Delete(id, out errorMessage))
            {
                Console.WriteLine("Award deleted");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }
    }
}
