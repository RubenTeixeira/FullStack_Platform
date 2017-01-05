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

namespace DBAPI.Controllers
{
    public class POIController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: api/POI
        [ResponseType(typeof(List<POIDTO>))]
        public async Task<IHttpActionResult> GetPOI()
        {
            List<POI> listPoi = await unitOfWork.POIRepository.FindPOIs();

            List<POIDTO> list = unitOfWork.POIRepository.ConvertModelListToDTO(listPoi);

            return Ok(list);
        }

        // GET: api/POI/5
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> GetPOI(int id)
        {
            POI poi = await unitOfWork.POIRepository.FindPOIByIDAsync(id);

            if (poi == null)
            {
                return NotFound();
            }

            POIDTO dto = unitOfWork.POIRepository.ConvertModelToDTO(poi);

            return Ok(dto);
        }

        // PUT: api/POI/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPOI(int id, POI pOI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pOI.POIID)
            {
                return BadRequest();
            }

            unitOfWork.POIRepository.UpdatePOI(pOI);

            try
            {
                await unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!unitOfWork.POIRepository.POIExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/POI
        [ResponseType(typeof(POIDTO))]
        public async Task<IHttpActionResult> PostPOI(POI pOI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await unitOfWork.POIRepository.CreatePOI(pOI);

            var dto = unitOfWork.POIRepository.ConvertModelToDTO(pOI);

            return CreatedAtRoute("DefaultApi", new { id = pOI.POIID }, dto);
        }

        // DELETE: api/POI/5
        [ResponseType(typeof(POI))]
        public async Task<IHttpActionResult> DeletePOI(int id)
        {
            POI pOI = await unitOfWork.POIRepository.FindPOIByIDAsync(id);

            if (pOI == null)
            {
                return NotFound();
            }

            await unitOfWork.POIRepository.DeletePOI(id);

            return Ok(pOI);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}