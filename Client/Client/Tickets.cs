using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Session;

namespace Tickets
{
    public class Tickets
    {
        public Auth session;

        public Tickets(Auth userSession)
        {
            session = userSession;
        }

        public async Task<List<dynamic>> get()
        {
            var api = new API.Request();
            string ticketsResult = await api.Get("tickets", session.apiKey);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(ticketsResult);
        }
    }

    public class Ticket : Tickets
    {
        public Ticket(Auth userSession) : base(userSession) { }

        public async Task<List<dynamic>> get(string id)
        {
            var api = new API.Request();
            string ticketsResult = await api.Get("tickets\\" + id + "\\messages" , session.apiKey);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(ticketsResult);
        }

        public async Task send(string message, string ticketID)
        {
            var api = new API.Request();
            var messageData = new { TicketID = ticketID, Content = message, EmailID = await session.getEmail() };
            var result = await api.Post("messages" , messageData, session.apiKey);
        }

    }

}
