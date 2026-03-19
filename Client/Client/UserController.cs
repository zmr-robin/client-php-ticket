using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class UserController
    {
        private Client.Session Session;
        public UserController(Client.Session session)
        {
            Session = session;
        }

        public async Task<string> GetCurrentEmailID()
        {
            string apiKey = Session.Key;
            // Get by apikey supporter ID
            dynamic result = await Session.Api.Request(RequestType.Get, "auth/" + apiKey, new {});
            // Get supporter email
            dynamic resultSupporter = await Session.Api.Request(RequestType.Get, "users/" + result["SupporterID"], new { });
            return resultSupporter["EmailID"];
        }
    }
}
