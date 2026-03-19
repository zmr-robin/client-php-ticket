using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
using Tickets;
using static System.Collections.Specialized.BitVector32;

namespace Commands
{
    internal interface ICommand
    {
        Task Execute(string command);
        void Help();
    }

    internal class Ticket : ICommand
    {
        static public Dictionary<string, string> CommandList = new Dictionary<string, string>
        {
            ["Message"] = "Send a message",
            ["Email {id}"] = "Get email adress of email id",
            ["Archive"] = "Archive current ticket",
            ["Load"] = "Refresh ticket messages",
            ["Back"] = "Back to tickets"
        };
        public Client.Session Session;
        public TicketsController Controller;
        public string TicketID;
        public Ticket(Client.Session session, string ticketID)
        {
            Session = session;
            Controller = new TicketsController(Session);
            TicketID = ticketID;
        }

        public async Task Execute(string command)
        {
            switch (command)
            {
                case "Help":
                    {
                        Help();
                        break;
                    }
                case "Clear":
                    {
                        Console.Clear();
                        break;
                    }
                case "Back":
                    {
                        Session.Commands.ChangeCurrent(new Tickets(Session));
                        break;
                    }
                case var prefix when command.StartsWith("Message"):
                    {
                        string content = command.Replace("Message ", "");
                        await Controller.Message(TicketID, content);
                        Console.Clear();
                        await Controller.View(TicketID);
                        break;
                    }
                case var prefix when command.StartsWith("Archive"):
                    {
                        await Controller.Archive(TicketID);
                        Console.Clear();
                        Session.Commands.ChangeCurrent(new Tickets(Session));
                        break;
                    }
                case "Load":
                    {
                        Console.Clear();
                        await Controller.View(TicketID);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Command not found, use 'Help' to learn about all available commands!");
                        break;

                    }
            }
        }

        public void Help()
        {

        }
    }

    internal class Tickets : ICommand
    {
        static public Dictionary<string, string> CommandList = new Dictionary<string, string>
        {
            ["Load"] = "View all open tickets",
            ["View {id}"] = "Inspect ticket with id",
            ["Back"] = "Back to menu"
        };
        public Client.Session Session;
        public TicketsController Controller;
        public Tickets(Client.Session session)
        {
            Session = session;
            Controller = new TicketsController(Session);
        }

        public async Task Execute(string command)
        {
            switch (command)
            {
                case "Help":
                    {
                        Help();
                        break;
                    }
                case "Clear":
                    {
                        Console.Clear();
                        break;
                    }
                case "Back":
                    {
                        Session.Commands.ChangeCurrent(new Menu(Session));
                        break;
                    }
                case "Load":
                    {
                        Console.Clear();
                        await Controller.Get();
                        break;
                    }
                case var prefix when command.StartsWith("View"):
                    {
                        Console.Clear();
                        string ticketID = command.Replace("View ", "");
                        await Controller.View(ticketID);
                        Session.Commands.ChangeCurrent(new Ticket(Session, ticketID));
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Command not found, use 'Help' to learn about all available commands!");
                        break;
                    }
            }
        }

        public void Help()
        {
            Console.WriteLine("Available commands for tickets");
            Console.WriteLine("---------------------------");
            for (int i = 0; i < CommandList.Count; i++)
            {
                Console.WriteLine(CommandList.ElementAt(i).Key.PadRight(20) + CommandList.ElementAt(i).Value);
            }
            Console.WriteLine("---------------------------");
        }

    }

    internal class Menu : ICommand
    {
        static public Dictionary<string, string> CommandList = new Dictionary<string, string>
        {
            ["Tickets"] = "Switch to ticket dashboard",
            ["Users"] = "Switch to user dashboard",
            ["Logout"] = "Kill session"
        };
        public Client.Session Session;
        public Menu(Client.Session session) {
            Session = session;
        }

        public async Task Execute(string command)
        {
            switch (command)
            {
                case "Help":
                    {
                        Help();
                        break;
                    }
                case "Clear":
                    {
                        Console.Clear();
                        break;
                    }
                case "Tickets":
                    {
                        Session.Commands.ChangeCurrent(new Tickets(Session));
                        break;
                    }
                case "Users":
                    {
                        break;
                    }
                case "Logout":
                    {
                        Console.Clear();
                        Session.Key = null;
                        break;
                    }
                case "Version":
                    {
                        dynamic serverVersion = await Session.Api.Request(RequestType.Get, "", new { });
                        Console.WriteLine(serverVersion["version"]);
                        break;
                    }
                case "Server":
                    {
                        Console.WriteLine("Current server: " + Session.Api.Url);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Command not found, use 'Help' to learn about all available commands!");
                        break;
                    }
            }
        }

        public void Help()
        {
            Console.WriteLine("Available commands for menu");
            Console.WriteLine("---------------------------");
            for (int i = 0; i < CommandList.Count; i++)
            {
                Console.WriteLine(CommandList.ElementAt(i).Key.PadRight(10) + CommandList.ElementAt(i).Value);
            }
            Console.WriteLine("---------------------------");
        }

    }

    internal class CommandsController
    {
        public ICommand Current;

        public CommandsController(Client.Session session) {
            Current = new Menu(session);
        }

        public void ChangeCurrent(ICommand newCurrent) {
            Current = newCurrent;
        }
        public async Task Command(string command) {
            await Current.Execute(command);
        }

    }
}
