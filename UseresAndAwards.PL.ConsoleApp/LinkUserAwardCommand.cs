using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.BLL.Contracts;
using UseresAndAwards.Entities;

namespace UseresAndAwards.PL.ConsoleApp
{
    internal static class LinkUserAwardCommand
    {
        private static IEntityLogic<LinkUserAward> Logic => BLL.Core.LinkUserAwardLogic.GetInstance;

        public static void Run()
        {
            do
            {
                Console.WriteLine("\n--- Link User and Award ---");
                Console.WriteLine("   a[dd]   - Add link");
                Console.WriteLine("   l[ist]  - Show all links");
                Console.WriteLine("   d[del]  - Delete link");
                Console.WriteLine("   e[exit] - Exit ");
                Console.Write("   Command: ");
            }
            while (WorkWithLinkUserAward(Console.ReadLine()));

            Console.WriteLine("\nfinished (Link User and Award)");
        }

        private static bool WorkWithLinkUserAward(string choice)
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
                    ExecuteLinkUserAwardAddCommand();
                    return true;

                case "l":
                case "list":
                    ExecuteLinkUserAwardListCommand();
                    return true;

                case "d":
                case "del":
                    ExecuteLinkUserAwardDelCommand();
                    return true;

                case "e":
                case "exit":
                    return false;

                default:
                    Console.WriteLine($"Unknown command '{choice}'");
                    return true;
            }
        }

        private static void ExecuteLinkUserAwardAddCommand()
        {
            Console.WriteLine("\n--- Link User and Award > Add link ---");

            string input;
            Guid userId;
            do
            {
                Console.Write("   User ID: ");
                input = Console.ReadLine().Trim();
            }
            while (!Guid.TryParse(input, out userId));

            Guid awardId;
            do
            {
                Console.Write("   Award ID: ");
                input = Console.ReadLine().Trim();
            }
            while (!Guid.TryParse(input, out awardId));

            LinkUserAward link = new LinkUserAward()
            {
                UserId = userId,
                AwardId = awardId
            };

            string errorMessage;
            if (Logic.Add(link, out errorMessage))
            {
                Console.WriteLine("Award added");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }

        private static void ExecuteLinkUserAwardListCommand()
        {
            Console.WriteLine("\n--- Link User and Award List > List link ---");

            IEnumerable<KeyValuePair<Guid, LinkUserAward>> collection;
            string errorMessage;
            if (Logic.GetAll(out collection, out errorMessage))
            {
                foreach (var item in collection)
                {
                    var link = item.Value;
                    Console.WriteLine($"Link ID:{item.Key}, User ID:{link.UserId}, Award ID {link.AwardId}");
                }

                Console.WriteLine("--- end of list ---");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }

        private static void ExecuteLinkUserAwardDelCommand()
        {
            Console.WriteLine("\n--- Link User and Award > Delete link ---");

            Guid id;
            string input;
            do
            {
                Console.Write("Link ID: ");
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
