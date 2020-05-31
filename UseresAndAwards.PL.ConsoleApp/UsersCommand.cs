using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.BLL.Contracts;
using UseresAndAwards.Entities;

namespace UseresAndAwards.PL.ConsoleApp
{
    internal static class UsersCommand
    {
        private static IEntityLogic<User> Logic => BLL.Core.UserLogic.GetInstance;

        public static void Run()
        {
            do
            {
                Console.WriteLine("\n--- Users ---");
                Console.WriteLine("   a[dd]   - Add user");
                Console.WriteLine("   l[ist]  - Show all users");
                Console.WriteLine("   d[del]  - Delete user");
                Console.WriteLine("   e[exit] - Exit ");
                Console.Write("   Command: ");
            }
            while (WorkWithUsers(Console.ReadLine()));

            Console.WriteLine("\nfinished (Users)");
        }

        private static bool WorkWithUsers(string choice)
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
                    ExecuteUsersAddCommand();
                    return true;

                case "l":
                case "list":
                    ExecuteUsersListCommand();
                    return true;

                case "d":
                case "del":
                    ExecuteUsersDelCommand();
                    return true;

                case "e":
                case "exit":
                    return false;

                default:
                    Console.WriteLine($"Unknown command '{choice}'");
                    return true;
            }
        }

        private static void ExecuteUsersAddCommand()
        {
            Console.WriteLine("\n--- Users > Add user ---");

            string name;
            do
            {
                Console.Write("   User name: ");
                name = Console.ReadLine().Trim();
            }
            while (name == string.Empty);

            DateTime date;
            string input;
            do
            {
                Console.Write("   Date of birth: ");
                input = Console.ReadLine().Trim();
            }
            while (!DateTime.TryParse(input, out date));

            User user = new User()
            {
                Name = name,
                DateOfBirth = date.Date
            };

            string errorMessage;
            if (Logic.Add(user, out errorMessage))
            {
                Console.WriteLine("User added");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }

        private static void ExecuteUsersListCommand()
        {
            Console.WriteLine("\n--- Users > Add user > List users ---");

            IEnumerable<KeyValuePair<Guid, User>> collection;
            string errorMessage;
            if (Logic.GetAll(out collection, out errorMessage))
            {
                foreach (var item in collection)
                {
                    var user = item.Value;
                    Console.WriteLine($"ID:{item.Key}, Name:{user.Name}, Birth:{user.DateOfBirth.ToShortDateString()}, Age:{user.Age}");
                }

                Console.WriteLine("--- end of list ---");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }

        private static void ExecuteUsersDelCommand()
        {
            Console.WriteLine("\n--- Users > Delete user ---");

            Guid id;
            string input;
            do
            {
                Console.Write("   User ID: ");
                input = Console.ReadLine().Trim();
            }
            while (!Guid.TryParse(input, out id));

            string errorMessage;
            if (Logic.Delete(id, out errorMessage))
            {
                Console.WriteLine("User deleted");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }
    }
}
