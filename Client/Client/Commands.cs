using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets;
using Session;
using TUI;
using System.Windows.Input;

namespace Commands
{
    public class Commands
    {
        static public TUI.TUI tui;
        static public string current = "menu";
        static public string currentID;

        static public Dictionary<string, string[]> commands = new Dictionary<string, string[]>
        {
            { "MenuCommand", new string[] { "Tickets", "User" } },
            { "TestCommand", new string[] { "all", "" } }
        };

        public Commands(TUI.TUI inputTui) {
            tui = inputTui;
        }

        static public async Task rout(string command, Auth session)
        {
            Console.Clear();
            switch (current)
            {
                case "menu":
                    switch (command)
                    {
                        case "Tickets":
                            {
                                // handle tickets
                                Tickets.Tickets controller = new Tickets.Tickets(session);
                                current = "tickets";
                                tui.Tickets(await controller.get());
                                break;
                            }

                        default:
                            Console.WriteLine("Command not found!");
                            break;
                            //return null;
                    }
                    break;
                case "tickets":
                    switch (command)
                    {
                        case var prefix when command.StartsWith("View"):
                            {
                                Tickets.Ticket controller = new Tickets.Ticket(session);
                                string ticketID = command.Replace("View ", "");
                                currentID = ticketID;
                                current = "ticket";
                                tui.Ticket(await controller.get(ticketID), ticketID);
                                break;
                            }
                        case "Back":
                            {
                                current = "menu";
                                tui.Main();
                                break;
                            }
                        default:
                            {
                                current = "tickets";
                                Console.WriteLine("Falsch!!!!!!!!!!!!!!!!");
                                break;
                            }
                    }
                    break;
                case "ticket":
                    switch (command)
                    {
                        case var prefix when command.StartsWith("Message"):
                            {
                                Tickets.Ticket controller = new Tickets.Ticket(session);
                                string message = command.Replace("Message ", "");
                                await controller.send(message, currentID);
                                tui.Ticket(await controller.get(currentID), currentID);
                                break;
                            }
                        case "Back":
                            {
                                current = "tickets";
                                Tickets.Tickets controller = new Tickets.Tickets(session);
                                tui.Tickets(await controller.get());
                                break;
                            }
                        default:
                            {
                                Tickets.Ticket controller = new Tickets.Ticket(session);
                                tui.Ticket(await controller.get(currentID), currentID);
                                break;
                            }
                    }
                    break;
                default:
                    current = "menu";
                    tui.Main();
                    break;
            }
        }

    }
}
