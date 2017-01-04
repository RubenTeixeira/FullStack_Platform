using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClassLibrary.Models;
using DBAPI.Models;

namespace DBAPI.Controllers
{
    public class POIController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/POI
        public IQueryable<POI> GetPOIs()
        {
            return db.POIs;
        }

        // GET: api/POI/5
        [ResponseType(typeof(POI))]
        public async Task<IHttpActionResult> GetPOI(int id)
        {
            POI pOI = await db.POIs.FindAsync(id);
            if (pOI == null)
            {
                return NotFound();
            }

            return Ok(pOI);
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

            db.Entry(pOI).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!POIExists(id))
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
        [ResponseType(typeof(POI))]
        public async Task<IHttpActionResult> PostPOI(POI pOI)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.POIs.Add(pOI);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pOI.POIID }, pOI);
        }

        // DELETE: api/POI/5
        [ResponseType(typeof(POI))]
        public async Task<IHttpActionResult> DeletePOI(int id)
        {
            POI pOI = await db.POIs.FindAsync(id);
            if (pOI == null)
            {
                return NotFound();
            }

            db.POIs.Remove(pOI);
            await db.SaveChangesAsync();

            return Ok(pOI);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool POIExists(int id)
        {
            return db.POIs.Count(e => e.POIID == id) > 0;
        }
    }
}