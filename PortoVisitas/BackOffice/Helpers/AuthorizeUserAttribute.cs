using ClassLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BackOffice.Helpers
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public string UserRole{ get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            List<string> userRoles = (List<string>) PVWebApiHttpClient.getRoles();

            if(userRoles == null)
            {
                return false;
            }

            bool allow = false;

            foreach(string role in userRoles)
            {
                if (role == this.UserRole)
                    allow = true;
            }

            return allow;
        }
    }
}