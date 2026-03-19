using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;

namespace Client
{
    internal class Session
    {

        public ApiFactory Api = new ApiFactory("http://ticket.zmro.net/");
        public UserController User;
        public Commands.CommandsController Commands;
        public string Key;

        public Session(){}

        public void ConnectCommandController(Session session)
        {
            Commands = new Commands.CommandsController(session);
        }
        public void ConnectUserController(Session session)
        {
            User = new UserController(session);
        }

        public async Task<dynamic> Auth(string email, string pass)
        { 
            dynamic authResult = await Api.Request(RequestType.Post, "auth", new { Email = email, Password = pass });
            if (authResult.status == 200)
            {
                Key = authResult.content; // Safe api key to this->Key
                Api.Key = Key; // Safe api key to object api.Key
                Console.Clear();
            } else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Email or Password incorrect!");
            }
            return authResult;
        }

    }
}
