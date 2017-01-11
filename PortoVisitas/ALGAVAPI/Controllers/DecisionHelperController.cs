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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Algav1")]
        // primeiro predicado - findpath recebe
        public async Task<IHttpActionResult> Algav1(int poiOrigem, int[] listPois, int inclinacaoMax, int startingMinute, string tipoVeiculo, int vel)
        {
            // Carrega base de conhecimento
            await loadsKnowledgeBase();
            // Escreve parametros 1
            File.WriteAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_1.txt", String.Empty);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_1.txt", poiOrigem + "." + System.Environment.NewLine);
            String str = null;
            foreach (int poisId in listPois)
            {
                str = str + poisId + ",";
            }
            str = str.Remove(str.Length - 1);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_1.txt", "[" + str + "]." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_1.txt", inclinacaoMax + "." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_1.txt", startingMinute + "." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_1.txt", tipoVeiculo + "." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_1.txt", vel + ".");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;     Esconde janela cmd
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C C:\\Users\\Gil\\Documents\\IISExpress\\loader_1.pl";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            // Read result
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Gil\Documents\IISExpress\Results_1.txt");
            string line = lines[0];

            List<int> poisCaminho = new List<int>();
            poisCaminho = processResultString(line, poisCaminho);


            return Ok(poisCaminho);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/Algav2")]
        // segundo predicado - findRingPathWithMaxHours
        public async Task<IHttpActionResult> Algav2(int poiOrigem, int maxHorasVisita, int horaInicialVisita, int inclinacaoMax, string tipoVeiculo, int vel)
        {
            // Carrega base de conhecimento
            await loadsKnowledgeBase();
            // Escreve parametros 2
            File.WriteAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_2.txt", String.Empty);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_2.txt", poiOrigem + "." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_2.txt", maxHorasVisita + "." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_2.txt", "time(" + horaInicialVisita + ",0,0).");
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_2.txt", inclinacaoMax + "." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_2.txt", tipoVeiculo + "." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\Parameters_2.txt", vel + "." + System.Environment.NewLine);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;     Esconde janela cmd
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C C:\\Users\\Gil\\Documents\\IISExpress\\loader_2.pl";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();


            // Read result
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Gil\Documents\IISExpress\Results_2.txt");
            string line0 = lines[0];
            string line1 = lines[1];
            List<int> poisCaminho = new List<int>();
            poisCaminho = processResultString(line0, poisCaminho);
            poisCaminho = processResultString(line1, poisCaminho);


            return Ok(poisCaminho);
        }


        public async Task<bool> loadsKnowledgeBase()
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

                    file.WriteLine("poi(" + id + "," + "time(" + horaAberturaHora + "," + horaAberturaMinutos + "," +
                                    horaAberturaSegundos + ")" + "," +
                                    "time(" + horaFechoHora + "," + horaFechoMinutos + "," +
                                    horaFechoSegundos + ")" + ","
                                    + tempoEstimadoVisita + "," + poiLatitude + "," + poiLongitude + ").");
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
                    foreach (POIDTO poi in pois)
                    {
                        if (poi.ID == poiInicialID) { poiInicial = poi; }
                        if (poi.ID == poiFinalID) { poiFinal = poi; }
                    }

                    Debug.Write(poiInicialID);
                    Debug.Write(poiInicial.ID);


                    double distancia = 1000 * getDistanceFromLatLonInKm(poiInicial.GPS_Lat, poiInicial.GPS_Long, poiFinal.GPS_Lat, poiFinal.GPS_Long);
                    // Sin (x) = Opposite / Hypotenuse
                    int inclinacao = Convert.ToInt32(((poiFinal.Altitude - poiInicial.Altitude) / distancia));

                    file.WriteLine("caminho(" + idCaminho + "," + idPoiInicio + "," + idPoiFinal + "," + inclinacao + ").");
                    idCaminho++;
                    file.WriteLine("caminho(" + idCaminho + "," + idPoiFinal + "," + idPoiInicio + "," + "-" + inclinacao + ").");
                    idCaminho++;
                }

            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(pe, 50)." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(carro, 300)." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(autocarro, 200)." + System.Environment.NewLine);
            File.AppendAllText(@"C:\Users\Gil\Documents\IISExpress\knowledge.pl", "transporte(tuk, 100).");

            return true;
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

        public static double getDistanceFromLatLonInKm(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(lat2 - lat1);  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        public static double deg2rad(decimal deg)
        {
            return (double)deg * (Math.PI / 180);
        }

        public static List<int> processResultString(string line, List<int> poisCaminho)
        {
            string[] splits = line.Split(')');       // Splits this -> [ (2,101,102), (3,102,103), (4,103,104)]
                                                      // into this   -> [ (2,101,102  -> ), (3,102,103  -> ..

            string first = splits[0].Substring(3);   // [ (2,101,102     --> 2,101,102

            string[] firstCaminho = first.Split(',');
            int idOrigem = Int32.Parse(firstCaminho[1]);
            int idDestino = Int32.Parse(firstCaminho[2]);

            if (poisCaminho.Count == 0)
            {
                poisCaminho.Add(idOrigem);
            }
            else
            {
                if (poisCaminho.Last() != idOrigem) { poisCaminho.Add(idOrigem); }
            }
            
            poisCaminho.Add(idDestino);
            System.Console.WriteLine(poisCaminho);
            for (int i = 1; i < splits.Length - 1; i++)
            {
                string middle = splits[i].Substring(3);
                string[] middleCaminho = middle.Split(',');
                int idOrigemMiddle = Int32.Parse(middleCaminho[1]);
                int idDestinoMiddle = Int32.Parse(middleCaminho[2]);

                if (poisCaminho.Last() != idOrigemMiddle) { poisCaminho.Add(idOrigemMiddle); }
                poisCaminho.Add(idDestinoMiddle);
                System.Console.WriteLine(poisCaminho);
            }

            return poisCaminho;
        }
    }
}
