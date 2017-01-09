using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class Percurso
    {
        public int PercursoID { get; set; }

        [Required]
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Percurso), ErrorMessageResourceName = "Percurso_Name_Error")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Percurso))]
        public string Name { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Percurso), ErrorMessageResourceName = "Percurso_Description_Error")]
        [Display(Name = "Description", ResourceType = typeof(Resources.Percurso))]
        public string Description { get; set; }

        [Display(Name = "Creator", ResourceType = typeof(Resources.Percurso))]
        public string Creator { get; set; }

        [Required]
        [Display(Name = "PercursoPOIs", ResourceType = typeof(Resources.Percurso))]
        public virtual ICollection<POI> PercursoPOIs { get; set; }

        public Percurso()
        {
            PercursoPOIs = new List<POI>();
        }
    }
}
