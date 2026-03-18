using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client;
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
            Console.WriteLine(ticket["Subject"]);
        }

    }
}
