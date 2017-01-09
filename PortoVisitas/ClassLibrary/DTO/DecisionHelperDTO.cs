using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTO
{
    public class DecisionHelperDTO
    {
        public int poiOrigem { get; set; }

        public List<int> listaPOIs { get; set; }

        public int inclinacao { get; set; }

        public int minutoInicial { get; set; }

        public String tipoTransporte { get; set; }
    }
}
