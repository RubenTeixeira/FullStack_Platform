﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult Change(string lang,string url)
        {
            if (lang != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = lang;
            Response.Cookies.Add(cookie);

            return Redirect(url);

        }
    }
}