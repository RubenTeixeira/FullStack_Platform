using ClassLibrary.DTO;
using ClassLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ALGAVAPI.Controllers
{
    public class DecisionHelperController : ApiController
    {
        public HttpClient client;

        [HttpPost]
        [Route("api/Algav1")]
        // primeiro predicado - findpath recebe
        public async Task<IHttpActionResult> Algav1(Algav1DTO request)
        {
            try
            {
                string parameters1 = ConfigurationManager.AppSettings["PARAMETERS1"];
                string loader1 = ConfigurationManager.AppSettings["LOADER1"];
                string results1 = ConfigurationManager.AppSettings["RESULTS1"];

                // Carrega base de conhecimento
                await loadsKnowledgeBase();
                // Escreve parametros 1

                File.WriteAllText(@results1, String.Empty);
                File.WriteAllText(@parameters1, String.Empty);
                File.AppendAllText(@parameters1, request.poiOrigem + "." + Environment.NewLine);

                string str = null;
                foreach (int poisId in request.poiList)
                {
                    str = str + poisId + ",";
                }
                str = str.Remove(str.Length - 1);


                string horaInicio = Convert.ToString(request.horaInicialVisita.Hour * 60 + request.horaInicialVisita.Minute);

                File.AppendAllText(@parameters1, "[" + str + "]." + Environment.NewLine);
                File.AppendAllText(@parameters1, request.inclinacaoMax + "." + Environment.NewLine);
                File.AppendAllText(@parameters1, horaInicio + "." + Environment.NewLine);
                File.AppendAllText(@parameters1, request.tipoVeiculo + "." + Environment.NewLine);
                File.AppendAllText(@parameters1, request.kilometrosMax + ".");

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;     Esconde janela cmd
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = loader1;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                // Read result
                string[] lines = File.ReadAllLines(@results1);
                List<int> poisCaminho = new List<int>();
                if (lines.Length > 0)
                {
                    string line = lines[0];
                    poisCaminho = processResultString(line, poisCaminho);
                }

                var response = new
                {
                    percurso = poisCaminho,
                    duracao = double.Parse(lines[3], CultureInfo.InvariantCulture),
                    kilometros = lines[2]
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("api/Algav2")]
        // segundo predicado - findRingPathWithMaxHours
        public async Task<IHttpActionResult> Algav2(Algav2DTO request)
        {
            try
            {
                string parameters2 = ConfigurationManager.AppSettings["PARAMETERS2"];
                string loader2 = ConfigurationManager.AppSettings["LOADER2"];
                string results2 = ConfigurationManager.AppSettings["RESULTS2"];
                // Carrega base de conhecimento
                await loadsKnowledgeBase();

                File.WriteAllText(@results2, String.Empty);

                // Escreve parametros 2
                File.WriteAllText(@parameters2, String.Empty);
                File.AppendAllText(@parameters2, request.poiOrigem + "." + Environment.NewLine);
                File.AppendAllText(@parameters2, request.maxHorasVisita + "." + Environment.NewLine);
                File.AppendAllText(@parameters2, "time(" + request.horaInicialVisita.Hour.ToString() + "," + request.horaInicialVisita.Minute.ToString() + "," + request.horaInicialVisita.Second.ToString() + ")." + Environment.NewLine);
                File.AppendAllText(@parameters2, request.inclinacaoMax + "." + Environment.NewLine);
                File.AppendAllText(@parameters2, request.tipoVeiculo + "." + Environment.NewLine);
                File.AppendAllText(@parameters2, request.kilometrosMax + "." + Environment.NewLine);

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;     Esconde janela cmd
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = loader2;
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();


                // Read result
                string[] lines = File.ReadAllLines(@results2);
                string line0 = lines[0];
                string line1 = lines[1];
                List<int> poisCaminho = new List<int>();
                poisCaminho = processResultString(line0, poisCaminho);
                poisCaminho = processResultString(line1, poisCaminho);


                var response = new
                {
                    percurso = poisCaminho,
                    duracao = double.Parse(lines[3], CultureInfo.InvariantCulture),
                    kilometros = lines[2]
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private async Task<bool> loadsKnowledgeBase()
        {

            string knowledge = ConfigurationManager.AppSettings["KNOWLEDGE"];
            // Get all pois
            List<POIDTO> pois = await GetPOIs();
            // Get caminhos
            List<CaminhoDTO> caminhos = await GetCaminhos();


            // Escreve base de conhecimento
            using (StreamWriter file =
            new StreamWriter(@knowledge))

                foreach (POIDTO poidto in pois)
                {
                    string id = poidto.ID.ToString();
                    string horaAberturaHora = poidto.OpenHour.Hour.ToString();
                    string horaAberturaMinutos = poidto.OpenHour.Minute.ToString();
                    string horaAberturaSegundos = "0";
                    string horaFechoHora = poidto.CloseHour.Hour.ToString();
                    string horaFechoMinutos = poidto.CloseHour.Minute.ToString();
                    string horaFechoSegundos = "0";
                    string tempoEstimadoVisita = poidto.VisitDuration.ToString();
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
            using (StreamWriter file =
            new StreamWriter(@knowledge, true))
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

                    double distancia = 1000 * getDistanceFromLatLonInKm(poiInicial.GPS_Lat, poiInicial.GPS_Long, poiFinal.GPS_Lat, poiFinal.GPS_Long);
                    // Sin (x) = Opposite / Hypotenuse
                    int inclinacao = Convert.ToInt32(((poiFinal.Altitude - poiInicial.Altitude) / distancia)*100);

                    file.WriteLine("caminho(" + idCaminho + "," + idPoiInicio + "," + idPoiFinal + "," + inclinacao + ").");
                    idCaminho++;
                    file.WriteLine("caminho(" + idCaminho + "," + idPoiFinal + "," + idPoiInicio + "," + (-1) * inclinacao + ").");
                    idCaminho++;
                }

            File.AppendAllText(@knowledge, "transporte(pe, 0.05)." + Environment.NewLine);
            File.AppendAllText(@knowledge, "transporte(carro, 0.3)." + Environment.NewLine);
            File.AppendAllText(@knowledge, "transporte(autocarro, 0.2)." + Environment.NewLine);
            File.AppendAllText(@knowledge, "transporte(tuk, 0.1).");

            return true;
        }


        private async Task<List<POIDTO>> GetPOIs()
        {
            client = DBWebApiHttpClient.GetClient();

            List<POIDTO> pois = null;
            var response = await client.GetAsync("api/POI/");

            if (response.IsSuccessStatusCode)
            {
                pois = (List<POIDTO>)await response.Content.ReadAsAsync<IEnumerable<POIDTO>>();
                return pois;
            }

            return null;
        }

        private async Task<List<CaminhoDTO>> GetCaminhos()
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

        private static double getDistanceFromLatLonInKm(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
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
        private static double deg2rad(decimal deg)
        {
            return (double)deg * (Math.PI / 180);
        }

        private static List<int> processResultString(string line, List<int> poisCaminho)
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
