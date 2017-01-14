using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ALGAVAPI.Helper
{
    public class CaminhoHelper: IEquatable<CaminhoHelper>
    {
        public int POI1 { get; set; }
        public int POI2 { get; set; }

        public bool Equals(CaminhoHelper other)
        {
            return this.POI1 == other.POI1 && this.POI2 == other.POI2;
        }
    }
}