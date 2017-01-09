using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PVAPI.Controllers
{
    public class PercursoController : ApiController
    {
        static HttpClient client;

        // GET: api/Percurso
        [ResponseType(typeof(PercursoDTO))]
        public async Task<IHttpActionResult> Get()
        {
            client = DBWebApiHttpClient.GetClient();

            IEnumerable<PercursoDTO> percurso = null;
            var response = await client.GetAsync("api/Percurso/");

            if (response.IsSuccessStatusCode)
            {
                percurso = await response.Content.ReadAsAsync<IEnumerable<PercursoDTO>>();
                return Ok(percurso);
            }

            return BadRequest(response.ToString());
        }

        // GET: api/Percurso/5
        [ResponseType(typeof(PercursoDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {

            client = DBWebApiHttpClient.GetClient();

            PercursoDTO percursoDTO = null;
            var response = await client.GetAsync("api/Percurso/" + id);

            if (response.IsSuccessStatusCode)
            {
                percursoDTO = await response.Content.ReadAsAsync<PercursoDTO>();
                return Ok(percursoDTO);
            }

            return BadRequest();
        }

        // POST: api/Percurso
        [ResponseType(typeof(PercursoDTO))]
        public async Task<IHttpActionResult> PostPercurso(Percurso percurso)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.PostAsJsonAsync("api/Percurso", percurso);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<PercursoDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest();
        }

        // PUT: api/Percurso/5
        [ResponseType(typeof(PercursoDTO))]
        public async Task<IHttpActionResult> PutPOI(int id, Percurso percurso)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.PutAsJsonAsync("api/Percurso/" + id, percurso);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<PercursoDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest();
        }

        // DELETE: api/Percurso/5
        [ResponseType(typeof(PercursoDTO))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            client = DBWebApiHttpClient.GetClient();
            var response = await client.DeleteAsync("api/Percurso/" + id);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var objResponse1 = JsonConvert.DeserializeObject<PercursoDTO>(result);
                return Ok(objResponse1);
            }

            return BadRequest();
        }
    }
}
