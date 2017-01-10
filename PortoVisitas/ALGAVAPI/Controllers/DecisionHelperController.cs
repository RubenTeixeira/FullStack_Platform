using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace ALGAVAPI.Controllers
{
    public class DecisionHelperController : ApiController
    {
        public Task<IHttpActionResult> pois;
        public HttpClient client;

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Test")]
        public void Test()
        {   
            // Get all pois
            //pois = GetPOIs();
            System.IO.File.WriteAllText(@"C:\Users\Gil\Documents\IISExpress\pois.txt", pois.ToString());

            // Get caminhos


            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;     Esconde janela cmd
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C C:\\Users\\Gil\\Documents\\IISExpress\\loader_1.pl";
            process.StartInfo = startInfo;
            process.Start();

            // Read result 1 and 2
        }

        //public async Task<List<POIDTO>> GetPOIs()
        //{
        //    client = DBWebApiHttpClient.GetClient();

        //    List<POIDTO> pois = null;
        //    var response = await client.GetAsync("api/POI/");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        pois = await response.Content.ReadAsAsync<List<POIDTO>>();
        //        return Ok(pois);
        //    }

        //    return null;
        //}
    }
}
