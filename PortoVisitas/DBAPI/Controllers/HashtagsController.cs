using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClassLibrary.Models;
using DBAPI.DAL;
using ClassLibrary.DTO;
using System.Diagnostics;
using System;
using System.Linq;
using System.Web.Http.ModelBinding;
using ClassLibrary.Helpers;

namespace DBAPI.Controllers
{
    public class HashtagsController : ApiController
    {

        private UnitOfWork unitOfWork = new UnitOfWork();

        // PUT: api/Hashtags/5
        [HttpPut]
        [Route("api/Hashtags/{id}")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHashtags(int id, POI pOI)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                string problems = "";
                foreach (string error in allErrors)
                {
                    problems += " _____ " + error;
                }
                return BadRequest("Invalid model object: " + problems);
            }

            if (id != pOI.POIID)
            {
                return BadRequest("ID received is not equal to POI ID");
            }

            try
            {
                await unitOfWork.POIRepository.UpdateHashtags(pOI);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception ocurred: " + ex.Message);
            }

            return StatusCode(HttpStatusCode.OK);
        }
    }
}
