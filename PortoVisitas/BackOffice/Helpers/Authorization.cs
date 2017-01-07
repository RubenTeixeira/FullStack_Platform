using ClassLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackOffice.Helpers
{
    public class Authorization
    {
        public static bool verifyAuthorization()
        {
            if (PVWebApiHttpClient.getUsername() == null)
            {
                return false;
            }
            return true;
        }

    }
}