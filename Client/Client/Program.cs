using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Client
{
    internal class Program
    {
        private static string GetHiddenConsoleInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }

        static async Task Main(string[] args)
        {
            Session session = new Session();
            // Test credentials for auto login while debugging 
            //await session.Auth("demo@zmro.dev", "123");
            session.ConnectCommandController(session);
            session.ConnectUserController(session);
            while (true)
            {
                // If session key (api key) not set -> prompt user to login
                while (string.IsNullOrEmpty(session.Key))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("1. Login / 2. Signup");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("]\n~ ");
                    string userInput = Console.ReadLine();
                    if (userInput == "1")
                    {
                        Console.Clear();
                        while (string.IsNullOrEmpty(session.Key))
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Email");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("] ~ ");
                            string inputEmail = Console.ReadLine();

                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("[");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("Password");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("] ~ ");
                            string inputPassword = GetHiddenConsoleInput();
                            await session.Auth(inputEmail, inputPassword);
                        }
                    } else if (userInput == "2")
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Email");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("] ~ ");
                        string inputEmail = Console.ReadLine();

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("First name");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("] ~ ");
                        string inputFirstName = Console.ReadLine();


                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Last name");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("] ~ ");
                        string inputLastName = Console.ReadLine();

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Password");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("] ~ ");
                        string inputPassword = GetHiddenConsoleInput();
                        await session.Signup(inputEmail, inputFirstName, inputLastName, inputPassword);

                    } else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option (1 or 2)");
                    }
                    
                }
                // While session key is not null or empty -> allow user to use commands
                while (!string.IsNullOrEmpty(session.Key))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(session.Commands.Current.GetType().Name);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("] ~ ");
                    string input = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    await session.Commands.Command(input);
                }
            }

        }
    }
}
