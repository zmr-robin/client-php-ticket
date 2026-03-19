using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace Client
{
    internal class UserController
    {
        private Client.Session Session;
        public UserController(Client.Session session)
        {
            Session = session;
        }

        // Get email ID of logged in user
        public async Task<string> GetCurrentEmailID()
        {
            string apiKey = Session.Key;
            // Get by apikey supporter ID
            dynamic result = await Session.Api.Request(RequestType.Get, "auth/" + apiKey, new {});
            // Get supporter email
            dynamic resultSupporter = await Session.Api.Request(RequestType.Get, "users/" + result["SupporterID"], new { });
            return resultSupporter["EmailID"];
        }

        public async Task Get()
        {
            dynamic result = await Session.Api.Request(RequestType.Get, "users", new {});
            var table = new ConsoleTable("UserID", "EmailID", "RoleID", "First Name", "Last Name");
            foreach(var user in result)
            {
                var userEmail = await Session.Api.Request(RequestType.Get, "emails/" + user["EmailID"], new { });
                //Console.WriteLine(userEmail);
                table.AddRow(user["UserID"], userEmail["Email"], user["RoleID"], user["FirstName"], user["LastName"]);
            }
            table.Write();
        }

        public async Task Whitelist(string email)
        {
            await Session.Api.Request(RequestType.Post, "users/invite", new { Email = email });
        }
    }
}
