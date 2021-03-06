﻿using BackOffice.Helpers;
using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using ClassLibrary.Models;
using ClassLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Controllers
{

    [AuthorizeUser(UserRole = "Gestor")]
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

            ViewBag.route = buildOriginalRouteString(percurso, percurso.PercursoPOIs.ToList());

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
        public async Task<ActionResult> Create([Bind(Include = "PercursoID,Name,Description,StartHour,PercursoPOIsOrder")] Percurso percurso)
        {

            List<POI> list = await getPOIList(null);
            ViewBag.PoiList = list;

            if(!parsePercursoPOIs(percurso, list))
            {
                return View(percurso);
            }

            var finishHour = Request.Form["FinishHour"];

            if (finishHour != null)
            {
                percurso.FinishHour = percurso.StartHour.AddMinutes(double.Parse(finishHour, CultureInfo.InvariantCulture));
            }

            percurso.Creator = PVWebApiHttpClient.getUsername();

            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.PostAsJsonAsync("api/Percurso", percurso);

            if (responseHttp.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(percurso);

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

            string route = buildOriginalRouteString(percurso, poiList);

            percursoModel.Percurso = percurso;
            percursoModel.PercursoPOIs = poiList;
            percursoModel.PercursoOriginal = route;

            return View(percursoModel);
        }

        // POST: Percurso/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, [Bind(Include = "PercursoID,Name,Description,Creator,FinishHour,StartHour,PercursoPOIsOrder")] Percurso percurso)
        {

            PercursoViewModel percursoModel = new PercursoViewModel();
            List<POI> poiList = await getPOIList(null);

            percursoModel.Percurso = percurso;
            percursoModel.PercursoPOIs = poiList;
            percursoModel.PercursoOriginal = Request.Form["PercursoOriginal"].ToString();

            if (!parsePercursoPOIs(percurso, poiList))
            {
                return View(percurso);
            }

            var finishHour = Request.Form["Percurso.FinishHour"];
            try { 
                percurso.FinishHour = percurso.StartHour.AddMinutes(double.Parse(finishHour, CultureInfo.InvariantCulture));
            }catch{
                //
            }


            client = PVWebApiHttpClient.GetClient();
            var responseHttp = await client.PutAsJsonAsync("api/Percurso/" + id, percurso);

            Console.Write(responseHttp.RequestMessage);

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

            ViewBag.route = buildOriginalRouteString(percurso, percurso.PercursoPOIs.ToList());

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

            ViewBag.route = buildOriginalRouteString(percurso, percurso.PercursoPOIs.ToList());

            return View(percurso);
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

        public bool parsePercursoPOIs(Percurso percurso, List<POI> listPOI)
        {
            List<int> visited = new List<int>();

            if (listPOI != null && percurso.PercursoPOIsOrder!=null)
            {
                string[] percursoPOIs = percurso.PercursoPOIsOrder.Split(',');

                if (percursoPOIs.Count() != 0)
                {
                    foreach (string id in percursoPOIs)
                    {
                        foreach(POI p in listPOI)
                        {
                            if(Int32.Parse(id) == p.POIID && !visited.Contains(p.POIID)) {
                                POI connected = new POI();
                                connected.POIID = Int32.Parse(id);
                                connected.Name = "Dummy";
                                connected.GPS_Lat = 41.15M;
                                connected.GPS_Long = -8.65M;
                                connected.Altitude = 15;
                                connected.VisitDuration = 60;

                                percurso.PercursoPOIs.Add(connected);
                                visited.Add(p.POIID);
                            }
                        }
                    }
                }
            }else
            {
                return false;
            }
            return true;
        }

        public string buildOriginalRouteString(Percurso percurso, List<POI> poiList)
        {
            int count = 0;
            List<int> visited = new List<int>();
            string route = "";
            string[] ids = percurso.PercursoPOIsOrder.Split(',');

            foreach (string poi_id in ids)
            {
                foreach (POI p in poiList)
                {
                    if (p.POIID == Int32.Parse(poi_id))
                    {
                        if (count == 0)
                        {
                            route += ClassLibrary.Resources.Percurso.Begin + ": " + p.Name + "\n";
                        }
                        else if (count == ids.Length - 1)
                        {
                            route += ClassLibrary.Resources.Percurso.End + ": " + p.Name + "\n\n";
                        }
                        else
                        {
                            if (visited.Contains(p.POIID))
                            {
                                route += ClassLibrary.Resources.Percurso.Passing + ": " + p.Name + "\n";
                            }
                            else
                            {
                                route += ClassLibrary.Resources.Percurso.Stop + " " + count + ": " + p.Name + "\n";
                                visited.Add(p.POIID);
                            }
                        }
                    }
                }
                count++;
            }
            string hour = (percurso.FinishHour.Hour - percurso.StartHour.Hour).ToString();
            string minutos = (percurso.FinishHour.Minute -percurso.StartHour.Minute).ToString();
            if(Int32.Parse(minutos) < 10)
            {
                minutos = "0" + minutos;
            }
            route += ClassLibrary.Resources.Percurso.Duration + ": " +  hour+":"+minutos;

            return route;
        }

        //public void buildPercursoViewModel(PercursoViewModel percursoModel, Percurso percurso, List<POI> poiList)
        //{
        //    percursoModel.percurso = percurso;

        //    ConnectedPOIViewModel poiSelectedModel = new ConnectedPOIViewModel();
        //    IEnumerable<int> selectedItemsArray = percurso.PercursoPOIs.Select(x => x.POIID).ToArray();
        //    ViewBag.Selected = selectedItemsArray;

        //    poiSelectedModel.SelectedItemIds = selectedItemsArray;

        //    poiSelectedModel.Items = poiList;

        //    percursoModel.percursoPoi = poiSelectedModel;
        //}
    }
    #endregion
}
