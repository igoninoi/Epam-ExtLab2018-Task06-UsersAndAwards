using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UseresAndAwards.BLL.Contracts;
using UseresAndAwards.Entities;

namespace UseresAndAwards.PL.ConsoleApp
{
    internal static class JoinUserAwardCommand
    {
        private static IJoinLogic<User, Award> JoinLogic => BLL.Core.LinkUserAwardLogic.GetJounInstance;

        public static void Run()
        {
            do
            {
                Console.WriteLine("\n--- Users joined with Awards ---");
                Console.WriteLine("   l[ist]  - Show joined list");
                Console.WriteLine("   e[exit] - Exit ");
                Console.Write("   Command: ");
            }
            while (WorkWithJoinUserAward(Console.ReadLine()));

            Console.WriteLine("\nfinished (Link User and Award)");
        }

        private static bool WorkWithJoinUserAward(string choice)
        {
            if (choice == null)
            {
                throw new NullReferenceException();
            }

            choice = choice.Trim().ToLower();
            switch (choice)
            {
                case "l":
                case "list":
                    ExecuteJoinUserAwardListCommand();
                    return true;

                case "e":
                case "exit":
                    return false;

                default:
                    Console.WriteLine($"Unknown command '{choice}'");
                    return true;
            }
        }

        private static void ExecuteJoinUserAwardListCommand()
        {
            Console.WriteLine("\n--- Users joined with Awards List > List  ---");

            IEnumerable<KeyValuePair<User, Award>> collection;
            string errorMessage;
            if (JoinLogic.GetLeftJoin(out collection, out errorMessage))
            {
                string prevUserName = string.Empty;
                string output;
                StringBuilder stub = new StringBuilder();
                foreach (var item in collection)
                {
                    var user = item.Key;
                    var award = item.Value;
                    if (user.Name != prevUserName)
                    {
                        output = $"User name:{user.Name}, Birth:{user.DateOfBirth.ToShortDateString()}, Award: {award.Title}";
                        Console.WriteLine(output);
                        stub = stub.Clear().Append(' ', output.IndexOf(", Award:"));
                    }
                    else
                    {
                        Console.WriteLine($"{stub.ToString()}, Award: {award.Title}");
                    }

                    prevUserName = user.Name;
                }

                Console.WriteLine("--- end of list ---");
            }
            else
            {
                Console.WriteLine($"Error: {errorMessage}");
            }
        }
    }
}
