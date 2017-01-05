using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Controllers
{
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: POI/Create
        public async Task<ActionResult> Create()
        {

            ViewBag.PoiList = await getPOIList();

            return View();
        }

        // POST: POI/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "POIID,Name,Description,GPS_Lat,GPS_Long,ConnectedPOIs")] POI pOI)
        {

            ViewBag.PoiList = await getPOIList();

            var connectedForm = Request.Form["ConnectedPOIs"];
            parseConnectedPOIs(pOI, connectedForm);

            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.PostAsJsonAsync("api/POI", pOI);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();

        }

        // GET: POI/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: POI/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: POI/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: POI/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<List<POI>> getPOIList()
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
            return poiList;
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
                        connected.GPS_Lat = 1.0M;
                        connected.GPS_Long = 1.0M;

                        pOI.ConnectedPOIs.Add(connected);
                    }
                }
            }
        }
    }

}
