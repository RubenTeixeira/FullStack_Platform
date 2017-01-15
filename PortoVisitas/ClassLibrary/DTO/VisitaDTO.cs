using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTO
{
    public class VisitaDTO
    {
        public int VisitaID { get; set; }

        public string Creator { get; set; }

        public PercursoDTO Percurso { get; set; }

        public DateTime Date { get; set; }
    }
}
