using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace ALGAVAPI.Controllers
{
    public class DecisionHelperController : ApiController
    {
        public HttpClient client;

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/Test")]
        public async Task<IHttpActionResult> Test()
        {
            // Get all pois
            List<POIDTO> pois = await GetPOIs();
            // Get caminhos
            List<CaminhoDTO> caminhos = await GetCaminhos();


            // Escreve base de conhecimento
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl"))

            foreach (POIDTO poidto in pois)
                {
                    string id = poidto.ID.ToString();
                    string horaAberturaHora = poidto.OpenHour.Hour.ToString();
                    string horaAberturaMinutos = poidto.OpenHour.Minute.ToString();
                    string horaAberturaSegundos = "0";
                    string horaFechoHora = poidto.CloseHour.Hour.ToString();
                    string horaFechoMinutos = poidto.CloseHour.Minute.ToString();
                    string horaFechoSegundos = "0";
                    string tempoEstimadoVisita = "60";
                    string poiLatitude = poidto.GPS_Lat.ToString();
                    poiLatitude = poiLatitude.Replace(',', '.');
                    string poiLongitude = poidto.GPS_Long.ToString();
                    poiLongitude = poiLongitude.Replace(',', '.');

                    file.WriteLine("poi("+id+","+"time("+horaAberturaHora+","+horaAberturaMinutos+","+
                                    horaAberturaSegundos+")"+","+
                                    "time("+horaFechoHora+", "+horaFechoMinutos+", "+
                                    horaFechoSegundos + ")" + ","
                                    + tempoEstimadoVisita+","+poiLatitude+","+poiLongitude+").");
                }

            int idCaminho = 0;
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", true))
                foreach (CaminhoDTO caminhoDto in caminhos)
            {
                string idPoiInicio = caminhoDto.POIID.ToString();
                string idPoiFinal = caminhoDto.ConnectedPOIID.ToString();
                    int poiInicialID = caminhoDto.POIID;
                    int poiFinalID = caminhoDto.ConnectedPOIID;

                    POIDTO poiInicial = null;
                    POIDTO poiFinal = null;
                    foreach(POIDTO poi in pois)
                    {
                        if(poi.ID == poiInicialID) { poiInicial = poi; }
                        if(poi.ID == poiFinalID) { poiFinal = poi; }
                    }

                    Debug.Write(poiInicialID);
                    Debug.Write(poiInicial.ID);
                        double poiFinal_Lat = (double)poiFinal.GPS_Lat;
                        double poiFinal_Long = (double)poiFinal.GPS_Long;
                        double poiInicial_Lat = (double)poiInicial.GPS_Lat;
                        double poiInicial_Long = (double)poiInicial.GPS_Long;


                    double distanciaEntrePois = Math.Sqrt((poiFinal_Lat - poiInicial_Lat) * (poiFinal_Lat - poiInicial_Lat) + (poiFinal_Long - poiInicial_Long)*(poiFinal_Long - poiInicial_Long));

                    double inclinacao = ((poiFinal.Altitude - poiInicial.Altitude) * 100) / distanciaEntrePois;

                file.WriteLine("caminho("+idCaminho+","+idPoiInicio+","+idPoiFinal+","+inclinacao+").");
                idCaminho++;
            }

            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(pe, 50)."+ System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(carro, 300)."+ System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(autocarro, 200)."+ System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(tuk, 100).");


            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;     Esconde janela cmd
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C C:\\Users\\Gil\\Documents\\IISExpress\\loader_1.pl";
            process.StartInfo = startInfo;
            process.Start();

            // Read result 1 and 2

            return Ok(pois);
        }

        public async Task<List<POIDTO>> GetPOIs()
        {
            client = DBWebApiHttpClient.GetClient();

            List<POIDTO> pois = null;
            var response = await client.GetAsync("api/POI/");

            if (response.IsSuccessStatusCode)
            {
                pois = (List<POIDTO>) await response.Content.ReadAsAsync<IEnumerable<POIDTO>>();
                return pois;
            }

            return null;
        }

        public async Task<List<CaminhoDTO>> GetCaminhos()
        {
            client = DBWebApiHttpClient.GetClient();

            List<CaminhoDTO> caminhos = null;
            var response = await client.GetAsync("api/Caminho/");

            if (response.IsSuccessStatusCode)
            {
                caminhos = (List<CaminhoDTO>)await response.Content.ReadAsAsync<IEnumerable<CaminhoDTO>>();
                return caminhos;
            }

            return null;
        }
    }
}
