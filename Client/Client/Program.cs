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
        static async Task Main(string[] args)
        {
            Session session = new Session();
            await session.Auth("demo@zmro.dev", "123");
            session.ConnectCommandController(session);
            while (true)
            {
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
                            string inputPassword = Console.ReadLine();
                            await session.Auth(inputEmail, inputPassword);
                        }
                    } else if (userInput == "2")
                    {

                    } else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option (1 or 2)");
                    }
                    

                    
                    
                }
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
