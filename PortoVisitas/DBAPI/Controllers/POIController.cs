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

namespace DBAPI.Controllers
{
    public class POIController : ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: api/POI
        [Route("api/POI")]
        [ResponseType(typeof(List<POIDTO>))]
        public async Task<IHttpActionResult> GetPOI()
        {
            List<POI> listPoi = await unitOfWork.POIRepository.FindPOIs();

            List<POIDTO> list = unitOfWork.POIRepository.ConvertModelListToDTO(listPoi);

            return Ok(list);
        }

        // GET: api/POI
        [ResponseType(typeof(List<POIDTO>))]
        [Route("api/POIToApprove")]
        public async Task<IHttpActionResult> GetPOIToApprove()
        {
            List<POI> listPoi = await unitOfWork.POIRepository.FindPOIsToApprove();

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
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                string problems = "";
                foreach (string error in allErrors)
                {
                    problems += " _____ " + error;
                }
                return BadRequest("Invalid model object: "+problems);
            }

            if (id != pOI.POIID)
            {
                return BadRequest("ID received is not equal to POI ID");
            }

            try
            {
                await unitOfWork.POIRepository.UpdatePOI(pOI);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception ocurred: " + ex.Message);
            }
            //try
            //{
            //    await unitOfWork.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    if (!unitOfWork.POIRepository.POIExists(id))
            //    {
            //        return NotFound();
            //    }
            //    return BadRequest("Exception ocurred: "+ex.Message);
            //}

            return StatusCode(HttpStatusCode.OK);
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