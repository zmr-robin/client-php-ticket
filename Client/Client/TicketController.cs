using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Client;
using Commands;
using Newtonsoft.Json;

namespace Tickets
{
    internal class TicketsController
    {
        private Client.Session Session;
        public TicketsController(Client.Session session) {
            Session = session;
        }

        // Get all tickets
        public async Task Get()
        {
            dynamic tickets = await Session.Api.Request(RequestType.Get, "tickets", new { });

            int subjectWidth = 40;
            int idWidth = 5;
            int dateWidth = 20;
            int totalWidth = idWidth + subjectWidth + dateWidth + 4;

            Console.BackgroundColor = ConsoleColor.Red;
            int width = " Open tickets: ".Length + tickets.Count.ToString().Length + 40;
            Console.WriteLine(new string('-', totalWidth + 1));
            Console.WriteLine($" Open tickets: {tickets.Count}".PadRight(totalWidth + 1));
            Console.WriteLine(new string('=', totalWidth + 1));
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            foreach (var ticket in tickets)
            {
                string id = $"[{ticket["ID"]}]".PadRight(idWidth);
                string subject = $"{ticket["Subject"]}".PadRight(subjectWidth);
                string date = $"[{ticket["Date"]}]".PadLeft(dateWidth);
                Console.WriteLine($"{id}  {subject}  {date}");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        // Get ticket with id
        public async Task View(string id)
        {
            dynamic ticket = await Session.Api.Request(RequestType.Get, "tickets/" + id, new { });

            int contentWidth = 40;
            int idWidth = 5;
            int dateWidth = 20;
            int totalWidth = idWidth + contentWidth + dateWidth + 4;

            dynamic messages = await Session.Api.Request(RequestType.Get, "tickets/" + id + "/messages", new { });
            dynamic userEmailID = await Session.User.GetCurrentEmailID();

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine(new string('-', totalWidth - 1));
            Console.WriteLine($" {ticket["Subject"]}".PadRight(totalWidth - 1));
            Console.WriteLine(new string('=', totalWidth - 1));
            foreach (var message in messages)
            {
                if (message["EmailID"] == userEmailID)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                } else
                {
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                }
                string emailID = $"[{message["EmailID"]}]".PadRight(idWidth);
                string content = $"{message["Content"]}".PadRight(contentWidth);
                string date = $"[{message["Date"]}]".PadLeft(dateWidth);
                Console.WriteLine($"{emailID} {content} {date}");
            }
            Console.BackgroundColor = ConsoleColor.Black;

        }

        // Send Message to ticket
        public async Task Message(string ticketID, string content)
        {
            string userEmailID = await Session.User.GetCurrentEmailID();
            dynamic messageData = new {
                TicketID = ticketID,
                Content = content,
                EmailID = userEmailID
            };
            await Session.Api.Request(RequestType.Post, "messages", messageData);
        }

        // Archive a ticket 
        public async Task Archive(string ticketID)
        {
            await Session.Api.Request(RequestType.Put, "tickets/" + ticketID + "/archive", new {});
        }

    }
}
