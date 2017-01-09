using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTO
{
    public class CaminhoDTO
    {
        
        public int ID { get; set; }
        public int POIID { get; set; }

        public int ConnectedPOIID { get; set; }

        public int Inclinacao { get; set; }
        
    }
}
