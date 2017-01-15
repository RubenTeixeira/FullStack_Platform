using BackOffice.Helpers;
using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Controllers
{
    [AuthorizeUser(UserRole = "Gestor")]
    public class ValidatePOIController : Controller
    {

        static HttpClient client;

        // GET: ValidatePOI
        public async Task<ActionResult> Index()
        {
            client = PVWebApiHttpClient.GetClient();

            IEnumerable<POIDTO> pois = null;
            var response = await client.GetAsync("api/POIToApprove");

            if (response.IsSuccessStatusCode)
            {
                pois = await response.Content.ReadAsAsync<IEnumerable<POIDTO>>();
            }

            List<POI> poiList = new List<POI>();
            foreach (POIDTO poiDTO in pois)
            {
                POI poi = DTOConverters.ConvertDTOToModel(poiDTO);
                poiList.Add(poi);
            }

            return View(poiList);
        }

        // GET: ValidatePOI/Validate/1
        public async Task<ActionResult> Validate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            POI pOI = await getPOIByID(id);

            if (pOI == null)
            {
                return HttpNotFound();
            }

            return View(pOI);
        }

        // POST: ValidatePOI/Validate/1
        [HttpPost]
        public async Task<ActionResult> Validate(string submit, int id)
        {
            client = PVWebApiHttpClient.GetClient();

            POI pOI = await getPOIByID(id);

            foreach(Hashtag h in pOI.Hashtags)
            {
                h.ReferencedPOIs = new List<POI>();
            }

            foreach(POI connected in pOI.ConnectedPOIs)
            {
                connected.GPS_Lat = 41.15M;
                connected.GPS_Long = -8.65M;
                connected.Altitude = 15;
                connected.VisitDuration = 60;
                connected.ConnectedPOIs = new List<POI>();
            }

            if (submit == ClassLibrary.Resources.Global.Validate)
            {
                pOI.Approved = PVWebApiHttpClient.getUsername();
            }

            if (submit == ClassLibrary.Resources.Global.Reject)
            {
                pOI.Approved = "no";
            }

            var responseHttp = await client.PutAsJsonAsync("api/POI/" + id, pOI);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(pOI);
        }

        #region Helpers
        public static async Task<POI> getPOIByID(int? id)
        {
            client = PVWebApiHttpClient.GetClient();

            POIDTO poiDTO = null;
            var response = await client.GetAsync("api/POI/" + id);

            if (response.IsSuccessStatusCode)
            {
                poiDTO = await response.Content.ReadAsAsync<POIDTO>();
            }

            POI pOI = DTOConverters.ConvertDTOToModel(poiDTO);

            return pOI;
        }

    }
    #endregion
}