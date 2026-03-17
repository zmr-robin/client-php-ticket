using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Session
    {

        public ApiFactory Api = new ApiFactory("http://ticket.zmro.net/");
        public string Key;

        public Session(){}

        public async void Auth(string email, string pass)
        { 
            dynamic authResult = await Api.Request(RequestType.Post, "auth", new { Email = email, Password = pass });
            Key = authResult.content; // Safe api key to this->Key
            Api.Key = Key; // Safe api key to object api.Key
        }

    }
}
