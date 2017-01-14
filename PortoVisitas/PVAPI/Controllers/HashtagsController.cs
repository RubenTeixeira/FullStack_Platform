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
    public class HashtagsController : ApiController
    {

        static HttpClient client;


        // PUT: api/Hashtags/5
        [HttpPut]
        [Route("api/Hashtags/{id}")]
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> PutHashtags(int id, POI pOI)
        {
            client = DBWebApiHttpClient.GetClient();

            var response = await client.PutAsJsonAsync("api/Hashtags/" + id, pOI);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<POIDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest("PVAPI: Malformed request; " + response.ReasonPhrase);
        }

    }
}
