using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels
{
    public class PercursoViewModel
    {
        public Percurso percurso { get; set; }

        public ConnectedPOIViewModel percursoPoi { get; set; }
    }
}
