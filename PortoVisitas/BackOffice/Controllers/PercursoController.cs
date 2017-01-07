using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using ClassLibrary.ViewModels;
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
    public class PercursoController : Controller
    {
        static HttpClient client;

        // GET: Percurso
        public async Task<ActionResult> Index()
        {
            client = PVWebApiHttpClient.GetClient();

            IEnumerable<PercursoDTO> percursos = null;
            var response = await client.GetAsync("api/Percurso/");

            if (response.IsSuccessStatusCode)
            {
                percursos = await response.Content.ReadAsAsync<IEnumerable<PercursoDTO>>();
            }

            List<Percurso> percursosList = new List<Percurso>();
            foreach (PercursoDTO percursoDTO in percursos)
            {
                Percurso percurso = DTOConverters.ConvertDTOToModel(percursoDTO);
                percursosList.Add(percurso);
            }

            return View(percursosList);
        }

        // GET: Percurso/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Percurso percurso = await getPercursoByID(id);

            if (percurso == null)
            {
                return HttpNotFound();
            }

            return View(percurso);
        }

        // GET: Percurso/Create
        public async Task<ActionResult> Create()
        {

            ViewBag.PoiList = await getPOIList(null);

            return View();
        }

        // POST: Percurso/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "PercursoID,Name,Description,PercursoPOIs")] Percurso percurso)
        {

            ViewBag.PoiList = await getPOIList(null);

            var connectedForm = Request.Form["PercursoPOIs"];
            parsePercursoPOIs(percurso, connectedForm);

            percurso.Creator = PVWebApiHttpClient.getUsername();

            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.PostAsJsonAsync("api/Percurso", percurso);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();

        }

        // GET: Percurso/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Percurso percurso = await getPercursoByID(id);

            if (percurso == null)
            {
                return HttpNotFound();
            }

            PercursoViewModel percursoModel = new PercursoViewModel();
            List<POI> poiList = await getPOIList(null);
            buildPercursoViewModel(percursoModel, percurso, poiList);

            return View(percursoModel);
        }

        // POST: Percurso/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, [Bind(Include = "PercursoID,Name,Description,PercursoPOIs")] Percurso percurso)
        {

            PercursoViewModel percursoModel = new PercursoViewModel();
            List<POI> poiList = await getPOIList(null);

            var connectedForm = Request.Form["percursoPoi.SelectedItemIds"];
            parsePercursoPOIs(percurso, connectedForm);

            buildPercursoViewModel(percursoModel, percurso, poiList);

            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.PutAsJsonAsync("api/Percurso/" + id, percurso);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(percursoModel);
        }

        // GET: Percurso/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Percurso percurso = await getPercursoByID(id);

            if (percurso == null)
            {
                return HttpNotFound();
            }

            return View(percurso);
        }

        // POST: Percurso/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {

            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.DeleteAsync("api/Percurso/" + id);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            Percurso percurso = await getPercursoByID(id);

            if (percurso == null)
            {
                return HttpNotFound();
            }

            return View(percurso);
        }


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
                if (id != null && poi.POIID != id)
                {
                    poiList.Add(poi);
                }

                if (id == null)
                {
                    poiList.Add(poi);
                }

            }
            return poiList;
        }

        public async Task<Percurso> getPercursoByID(int? id)
        {
            client = PVWebApiHttpClient.GetClient();

            PercursoDTO percursoDTO = null;
            var response = await client.GetAsync("api/Percurso/" + id);

            if (response.IsSuccessStatusCode)
            {
                percursoDTO = await response.Content.ReadAsAsync<PercursoDTO>();
            }

            Percurso percurso = DTOConverters.ConvertDTOToModel(percursoDTO);

            return percurso;
        }

        public void parsePercursoPOIs(Percurso percurso, Object connectedForm)
        {
            if (connectedForm != null)
            {
                string[] percursoPOIs = connectedForm.ToString().Split(',');

                if (percursoPOIs.Count() != 0)
                {
                    foreach (string id in percursoPOIs)
                    {
                        POI connected = new POI();
                        connected.POIID = Int32.Parse(id);
                        connected.Name = "Dummy";
                        connected.GPS_Lat = 1.0M;
                        connected.GPS_Long = 1.0M;

                        percurso.PercursoPOIs.Add(connected);
                    }
                }
            }
        }

        public void buildPercursoViewModel(PercursoViewModel percursoModel, Percurso percurso, List<POI> poiList)
        {
            percursoModel.percurso = percurso;

            ConnectedPOIViewModel poiSelectedModel = new ConnectedPOIViewModel();
            IEnumerable<int> selectedItemsArray = percurso.PercursoPOIs.Select(x => x.POIID).ToArray();
            ViewBag.Selected = selectedItemsArray;

            poiSelectedModel.SelectedItemIds = selectedItemsArray;

            poiSelectedModel.Items = poiList;

            percursoModel.percursoPoi = poiSelectedModel;
        }
    }
}
