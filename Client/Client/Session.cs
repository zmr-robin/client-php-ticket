using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Newtonsoft.Json.Linq;

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

        public async Task Signup(string email, string firstName, string lastName, string password)
        {
            var signupData = new { Email = email, FirstName = firstName, LastName = lastName, Password = password };
            Console.WriteLine(signupData);
            Console.ReadLine();
            dynamic result = await Api.Request(RequestType.Post, "users/create", signupData);
            string status = null;

            if (result is JArray arr && arr.Count > 0)
                status = (string)arr[0]["status"];
            else if (result is JObject obj)
                status = (string)obj["status"];

            if (string.IsNullOrEmpty(status) || status != "401")
                {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("User created, you can now login");
            } else if (string.IsNullOrEmpty(status) || status != "409")
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Email already in use!");
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Your email is not whitelisted");
            }
        }

    }
}
