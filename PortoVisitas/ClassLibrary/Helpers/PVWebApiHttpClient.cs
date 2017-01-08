using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ClassLibrary.Helpers
{
    public class PVWebApiHttpClient
    {
        public const string WebApiBaseAddress = "http://10.8.11.86/PVAPI/";

        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(WebApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var session = HttpContext.Current.Session;
            if (session["token"] != null)
            {
                TokenResponse tokenResponse = getToken();
                Debug.Write("TESTE " + tokenResponse.AccessToken);
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("bearer", tokenResponse.AccessToken);
            }

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

        public static void storeUsername(string username)
        {
            var session = HttpContext.Current.Session;
            session["username"] = username;
        }

        public static string getUsername()
        {
            var session = HttpContext.Current.Session;
            return (string) session["username"];
        }

        public static void storeRoles(ICollection<string> roles)
        {
            var session = HttpContext.Current.Session;
            session["roles"] = roles;
        }

        public static ICollection<string> getRoles()
        {
            var session = HttpContext.Current.Session;
            return (ICollection<string>) session["roles"];
        }

        public static void clearToken()
        {
            var session = HttpContext.Current.Session;
            session["token"] = null;
            session["username"] = null;
            session["roles"] = null;
        }
    }
}
