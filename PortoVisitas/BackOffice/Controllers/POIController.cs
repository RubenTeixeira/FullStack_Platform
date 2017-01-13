using BackOffice.Helpers;
using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using ClassLibrary.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Controllers
{
    [AuthorizeUser(UserRole="Gestor")]
    public class POIController : Controller
    {
        static HttpClient client;

        // GET: POI
        public async Task<ActionResult> Index()
        {
            client = PVWebApiHttpClient.GetClient();

            IEnumerable<POIDTO> pois = null;
            var response = await client.GetAsync("api/POI/");

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

        // GET: POI/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: POI/Create
        public async Task<ActionResult> Create()
        {

            ViewBag.PoiList = await getPOIList(null);

            return View();
        }

        // POST: POI/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "POIID,Name,Description,OpenHour,CloseHour,VisitDuration,GPS_Lat,GPS_Long,Altitude,ConnectedPOIs")] POI pOI)
        {

            ViewBag.PoiList = await getPOIList(null);

            var connectedForm = Request.Form["ConnectedPOIs"];
            parseConnectedPOIs(pOI, connectedForm);

            string openHour = Request.Form["OpenHour"];
            pOI.OpenHour = Convert.ToDateTime(openHour);

            string closeHour = Request.Form["CloseHour"];
            pOI.CloseHour = Convert.ToDateTime(closeHour);

            pOI.Creator = PVWebApiHttpClient.getUsername();
            pOI.Approved = PVWebApiHttpClient.getUsername();

            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.PostAsJsonAsync("api/POI", pOI);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();

        }

        // GET: POI/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

            POIViewModel poiModel = new POIViewModel();
            List<POI> poiList = await getPOIList(pOI.POIID);
            buildPOIViewModel(poiModel,pOI,poiList);

            return View(poiModel);
        }

        // POST: POI/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, [Bind(Include = "POIID,Name,Description,OpenHour,CloseHour,VisitDuration,GPS_Lat,GPS_Long,Altitude,Creator,Approved,ConnectedPOIs,Hashtags")] POI pOI)
        {

            POIViewModel poiModel = new POIViewModel();
            List<POI> poiList = await getPOIList(null);
            POI poiAux = new POI();

            foreach(POI p in poiList)
            {
                if(p.POIID == pOI.POIID)
                {
                    poiAux = p;
                }
            }

            poiList.Remove(poiAux);

            var connectedForm = Request.Form["connectedPoi.SelectedItemIds"];
            parseConnectedPOIs(pOI, connectedForm);

            string openHour = Request.Form["poi.OpenHour"];
            pOI.OpenHour = Convert.ToDateTime(openHour);

            string closeHour = Request.Form["poi.CloseHour"];
            pOI.CloseHour = Convert.ToDateTime(closeHour);

            buildPOIViewModel(poiModel, pOI, poiList);

            pOI.Hashtags = poiAux.Hashtags;

            var responseHttp = await client.PutAsJsonAsync("api/POI/" + id, pOI);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(poiModel);
        }

        // GET: POI/Delete/5
        public async Task<ActionResult> Delete(int? id)
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

        // POST: POI/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {

            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.DeleteAsync("api/POI/" + id);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            POI pOI = await getPOIByID(id);

            if (pOI == null)
            {
                return HttpNotFound();
            }

            return View(pOI);
        }

        #region Helpers
        public async Task<List<POI>> getPOIList(int? id)
        {
            client = PVWebApiHttpClient.GetClient();

            IEnumerable<POIDTO> pois = null;
            var response = await client.GetAsync("api/POI/");

            if (response.IsSuccessStatusCode)
            {
                pois = await response.Content.ReadAsAsync<IEnumerable<POIDTO>>();
            }

            List<POI> poiList = new List<POI>();
            foreach (POIDTO poiDTO in pois)
            {
                POI poi = DTOConverters.ConvertDTOToModel(poiDTO);
                if( id != null && poi.POIID != id)
                {
                    poiList.Add(poi);
                }

                if(id == null)
                {
                    poiList.Add(poi);
                }

            }
            return poiList;
        }

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

        public void parseConnectedPOIs(POI pOI, Object connectedForm)
        {
            if (connectedForm != null)
            {
                string[] connectedPOIs = connectedForm.ToString().Split(',');

                if (connectedPOIs.Count() != 0)
                {
                    foreach (string id in connectedPOIs)
                    {
                        POI connected = new POI();
                        connected.POIID = Int32.Parse(id);
                        connected.Name = "Dummy";
                        connected.GPS_Lat = 41.15M;
                        connected.GPS_Long = -8.65M;
                        connected.Altitude = 15;
                        connected.VisitDuration = 60;

                        pOI.ConnectedPOIs.Add(connected);
                    }
                }
            }
        }

        public void buildPOIViewModel(POIViewModel poiModel, POI pOI, List<POI> poiList)
        {
            poiModel.poi = pOI;

            ConnectedPOIViewModel poiSelectedModel = new ConnectedPOIViewModel();
            IEnumerable<int> selectedItemsArray = pOI.ConnectedPOIs.Select(x => x.POIID).ToArray();
            ViewBag.Selected = selectedItemsArray;

            poiSelectedModel.SelectedItemIds = selectedItemsArray;

            poiSelectedModel.Items = poiList;

            poiModel.connectedPoi = poiSelectedModel;
        }
    }
    #endregion
}
