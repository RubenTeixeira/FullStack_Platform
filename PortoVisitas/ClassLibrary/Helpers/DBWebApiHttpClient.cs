using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace ClassLibrary.Helpers
{
    public class DBWebApiHttpClient
    {
        public const string WebApiBaseAddress = "https://10.8.11.86/DBAPI/";

        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(WebApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "OcDTF2pMnLXwsYSMav6tnrSLWweAWJ-v9LGqXwe84rwE3WetGuaCZuypHibQTdnULh5wZSCEgyoyJUFGR6pEBKfjGge_UIVTPt8FJuya--uhVARzpvt1buIOdLOppfmhCQpqC3Gn2ilIi1o9csG9Sk4tv-YPQyhNm7-M3oyexw-wk1IxCU1va626SzLL5vMOcu3kBAL_ledR6PilsV45md2KMxkvUab8h8uI4mBpBhvISfEom2LVgumNFuwJP7mXIIVUQU5wOweQ_f9bXG4RUGoR8SibTaMCLSPdi_YxhTjvV80lnRMsD5p1aaGD2wbQhn3EkuBqeUfW3U7ogaBLKz8M-MF9C25Cd-VRZX_9Cd2cIzuSeewi8Q1uGfhc7prllEBxcZtI8_igpDm-hQBEFr40OD9Mz9mVPpwCNcdOcdSFNyL4Z_xfGltsft2CoB3a3ylZf5sCOKQK6lZzni9ltufW6ArHN3SckZGHfYvSY2znLhpPd59EXgJAgZMevMlD_aLnZTaxKWsTJV_t4g6PNg");

            return client;
        }

        public static void storeToken(TokenResponse token)
        {
            var session = HttpContext.Current.Session;
            session["token"] = token;
        }

        public static TokenResponse getToken()
        {
            var session = HttpContext.Current.Session;
            return (TokenResponse)session["token"];
        }

    }

}