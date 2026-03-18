using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;


namespace Client
{
    internal enum RequestType
    {
        Get, Post, Put, Delete
    }

    internal abstract class Request
    {
        public string Endpoint;
        public string Key;
        public object RequestData;
        public string Result;
        public HttpClient HttpClient = new HttpClient();

        public Request(string enpoint, string key, object body) {
            Endpoint = enpoint;
            Key = key;
            RequestData = body;
            HttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
        }
    }

    internal class Get : Request
    {
        public Get(string enpoint, string key, object body) : base (enpoint, key, body){}

        public async Task<object> Send()
        {
            HttpResponseMessage response = await HttpClient.GetAsync(Endpoint);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(result);
        }
    }

    internal class Post : Request
    {
        public Post(string enpoint, string key, object body) : base(enpoint, key, body) { }

        public async Task<object> Send()
        {
            var json = JsonConvert.SerializeObject(RequestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await HttpClient.PostAsync(Endpoint, content);
            Result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(Result);
        }
    }

    internal class ApiFactory
    {
        public string Url;
        public string Key;

        public ApiFactory(string url)
        {
            Url = url;
        }
        public async Task<object> Request(RequestType requestType, string endpoint, object requestData)
        {
            switch (requestType)
            {
                case RequestType.Get:
                    {
                        Get controller = new Get(Url + endpoint, Key, requestData);
                        return await controller.Send();
                    }
                case RequestType.Post:
                    {
                        Post controller = new Post(Url + endpoint, Key, requestData);
                        return await controller.Send();
                    }
                case RequestType.Put:
                    {
                        return null;
                    }
                case RequestType.Delete:
                    {
                        return null;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
