using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PVAPI.Controllers
{
    public class VisitasController : ApiController
    {
        static HttpClient client;

        // GET: api/Visitas
        public async Task<IHttpActionResult> Get()
        {
            client = DBWebApiHttpClient.GetClient();

            IEnumerable<VisitaDTO> visita = null;
            var response = await client.GetAsync("api/Visitas/");

            if (response.IsSuccessStatusCode)
            {
                visita = await response.Content.ReadAsAsync<IEnumerable<VisitaDTO>>();
                return Ok(visita);
            }

            return BadRequest(response.ToString());
        }


        [HttpGet]
        [Route("api/UserVisitas")]
        public async Task<IHttpActionResult> GetUserVisitas(string email)
        {
            client = DBWebApiHttpClient.GetClient();

            IEnumerable<VisitaDTO> visitas = null;
            var response = await client.GetAsync("api/UserVisitas?email=" + email);

            if (response.IsSuccessStatusCode)
            {
                visitas = await response.Content.ReadAsAsync<IEnumerable<VisitaDTO>>();
                return Ok(visitas);
            }

            return BadRequest(response.ToString());

        }

        // GET: api/Visitas/5
        public async Task<IHttpActionResult> Get(int id)
        {
            client = DBWebApiHttpClient.GetClient();

            VisitaDTO visitaDTO = null;
            var response = await client.GetAsync("api/Visitas/" + id);

            if (response.IsSuccessStatusCode)
            {
                visitaDTO = await response.Content.ReadAsAsync<VisitaDTO>();
                return Ok(visitaDTO);
            }

            return BadRequest();
        }

        // POST: api/Visitas
        public async Task<IHttpActionResult> Post(VisitaDTO visitaDto)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.PostAsJsonAsync("api/Visitas", visitaDto);

            if (response.IsSuccessStatusCode)
            {
                VisitaDTO visitaDTO = await response.Content.ReadAsAsync<VisitaDTO>();
                return Ok(visitaDTO);
            }

            return BadRequest("Error " + response.ReasonPhrase);
        }

        // PUT: api/Visitas/5
        public async Task<IHttpActionResult> Put(int id, VisitaDTO visitaDto)
        {
            client = DBWebApiHttpClient.GetClient();

            var response = await client.PutAsJsonAsync("api/Visitas/" + id, visitaDto);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Ok(result);
            }

            var bad = await response.Content.ReadAsStringAsync();
            return BadRequest("PVAPI: Malformed request; " + bad);
        }

        // DELETE: api/Visitas/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.DeleteAsync("api/Visitas/" + id);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<VisitaDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest();
        }
    }
}
