using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Session session = new Session();
            session.Auth("demo@zmro.dev", "123");

            // Todo command pattern for user commands

            // Get Users via Get
            //Console.WriteLine(await api.Request(RequestType.Get, "users", new {}));

            // Auth a user via Post
            //Console.WriteLine(await api.Request(RequestType.Post, "auth", new { Email = "demo@zmro.dev", Password = "123" }));
            
            Console.Read();
            
        }
    }
}
