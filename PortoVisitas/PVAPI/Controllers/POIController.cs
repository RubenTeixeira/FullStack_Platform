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
using System.Web.Http.Cors;

namespace PVAPI.Controllers
{
    public class POIController : ApiController
    {
        static HttpClient client;

        // GET: api/POI
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> Get()
        {
            client = DBWebApiHttpClient.GetClient();

            IEnumerable<POIDTO> pois = null;
            var response = await client.GetAsync("api/POI/");

            if (response.IsSuccessStatusCode)
            {
               pois = await response.Content.ReadAsAsync<IEnumerable<POIDTO>>();
                return Ok(pois);
            }

            return BadRequest(response.ToString());
        }

        // GET: api/POI/5
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {

            client = DBWebApiHttpClient.GetClient();

            POIDTO poiDTO = null;
            var response = await client.GetAsync("api/POI/"+id);

            if (response.IsSuccessStatusCode)
            {
                poiDTO = await response.Content.ReadAsAsync<POIDTO>();
                return Ok(poiDTO);
            }

            return BadRequest();
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
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> PutPOI(int id, POI pOI)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.PutAsJsonAsync("api/POI/"+id, pOI);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<POIDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest();
        }

        // DELETE: api/POI/5
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.DeleteAsync("api/POI/" + id);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<POIDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest();
        }
    }
}
