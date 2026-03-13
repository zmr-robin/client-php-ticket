using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API;
using static System.Collections.Specialized.BitVector32;

namespace Session
{
    public class Session
    {
        public string apiKey;
        public bool auth = false;

        public void startSession(string apiKey)
        {

        }
    }

    public class Auth : Session
    {
        public Auth()
        {
            if (String.IsNullOrEmpty(apiKey))
            {
                auth = false;
            } else
            {
                auth= true; 
            }
        }

        public async Task Login(string UserEmail, string UserPassword)
        {
            var api = new API.Request();
            var loginData = new { Email = UserEmail, Password = UserPassword};
            string authResult = await api.Post("auth", loginData);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(authResult);

            if (response.status == 200){
                apiKey = (string)response.content;
                auth = true;
            } else
            {
                auth = false;
                Console.WriteLine("Failed to login!");
            }

            Console.WriteLine(authResult);
            //Console.ReadKey();
        }

        public async Task<string> getEmail()
        {
            var api = new API.Request();
            string apiResult = await api.Get("auth/" + apiKey, apiKey);
            var responseApi = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(apiResult);  // kein List<>
            string supporterResult = await api.Get("users/" + responseApi.SupporterID, apiKey);
            var responseSupporter = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(supporterResult);  // kein List<>
            return responseSupporter.EmailID;
        }

    }

}
