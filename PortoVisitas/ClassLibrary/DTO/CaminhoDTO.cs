using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTO
{
    public class CaminhoDTO
    {
        public int ID { get; set; }

        public int poiOrigID { get; set; }

        public int poiDestID { get; set; }

        public int inclinacao { get; set; }
    }
}
