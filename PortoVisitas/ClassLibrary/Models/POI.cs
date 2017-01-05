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
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Global), ErrorMessageResourceName = "POI_Name_Error")]
        public string Name{ get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Resources.Global), ErrorMessageResourceName = "POI_Description_Error" )]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [RegularExpression(@"([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}", ErrorMessageResourceType = typeof(Resources.Global), ErrorMessageResourceName = "POI_Lat_Error")]
        [Display(Name = "Gps - Latitude")]
        public decimal GPS_Lat { get; set; }

        [Required]
        [RegularExpression(@"([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}", ErrorMessageResourceType = typeof(Resources.Global), ErrorMessageResourceName = "POI_Long_Error")]
        [Display(Name = "Gps - Longitude")]
        public decimal GPS_Long { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ApplicationUser Approved { get; set; }

        public virtual ICollection<POI> ConnectedPOIs { get; set; }

        public POI()
        {
            ConnectedPOIs = new List<POI>();
        }

    }
}
