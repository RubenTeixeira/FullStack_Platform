using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClassLibrary.Helpers
{
    public class ALGAVWebApiHttpClient
    {
        public const string WebApiBaseAddress = "http://10.8.11.86/ALGAVAPI/";
        //public static string WebApiBaseAddress = ConfigurationManager.AppSettings["PVAPIURL"];

        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(WebApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
