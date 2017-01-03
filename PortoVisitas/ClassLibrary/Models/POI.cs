using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class POI
    {
        public int POIID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O nome não pode ter mais de 50 caracters.")]
        public string Name{ get; set; }

        [StringLength(250, ErrorMessage = "A descrição não pode ter mais de 250 caracteres.")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}", ErrorMessage = "Latitude inválida")]
        [Display(Name = "Gps - Latitude")]
        public decimal GPS_Lat { get; set; }

        [Required]
        [RegularExpression(@"([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}", ErrorMessage = "Longitude inválida")]
        [Display(Name = "Gps - Longitude")]
        public decimal GPS_Long { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ApplicationUser Approved { get; set; }
    }
}
