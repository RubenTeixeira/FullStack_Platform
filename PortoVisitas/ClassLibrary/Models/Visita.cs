using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class Visita
    {
        [Key]
        public int VisitaID { get; set; }

        [Display(Name = "Creator")]
        public string Creator { get; set; }

        public int PercursoID { get; set; }
        public virtual Percurso Percurso { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

    }
}
