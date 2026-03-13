using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using TUI;
using Session;
using System.Threading;

namespace API
{
    public class API
    {
        public string apiUrl = "http://ticket.zmro.net/";
        public HttpClient client = new HttpClient();
    }

    public class Request : API
    {
        public string result;
        public Request() {}

        public async Task<string> Get(string endpoint, string apiKey)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var cts = new CancellationTokenSource();
            var spinnerTask = Task.Run(() => TUI.TUI.Spinner("get...", cts.Token));

            HttpResponseMessage response = await client.GetAsync(apiUrl + endpoint);
            result = await response.Content.ReadAsStringAsync();

            cts.Cancel();
            await spinnerTask;

            return result;

        }

        public async Task<string> Post(string endpoint, object body, string apiKey = "")
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var cts = new CancellationTokenSource();
            var spinnerTask = Task.Run(() => TUI.TUI.Spinner("post...", cts.Token));

            HttpResponseMessage response = await client.PostAsync(apiUrl + endpoint, content);
            result = await response.Content.ReadAsStringAsync();

            cts.Cancel();
            await spinnerTask;
            
            return result;
        }

    }

}
