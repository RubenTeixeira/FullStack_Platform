using ClassLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ClassLibrary.Helpers;
using System.Net.Http.Formatting;
using System.Web.Http.Description;
using ClassLibrary.Models;
using Newtonsoft.Json;

namespace PVAPI.Controllers
{
    public class POIController : ApiController
    {
        static HttpClient client;

        // GET: api/POI
        public async Task<IEnumerable<POIDTO>> Get()
        {
            client = DBWebApiHttpClient.GetClient();

            IEnumerable<POIDTO> pois = null;
            var response = await client.GetAsync("api/POI/");

            if (response.IsSuccessStatusCode)
            {
                pois = await response.Content.ReadAsAsync<IEnumerable<POIDTO>>();
            }

            return pois;
        }

        // GET: api/POI/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/POI
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> PostPOI(POI pOI)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.PostAsJsonAsync("api/POI", pOI);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<POIDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest();
        }

        // PUT: api/POI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/POI/5
        public void Delete(int id)
        {
        }
    }
}
