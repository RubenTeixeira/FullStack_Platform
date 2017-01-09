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
        [Display(Name = "Name", ResourceType = typeof(Resources.POI))]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.POI), ErrorMessageResourceName = "POI_Name_Error")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(Resources.POI), ErrorMessageResourceName = "POI_Description_Error")]
        [Display(Name = "Description", ResourceType = typeof(Resources.POI))]
        public string Description { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [Display(Name = "OpenHour", ResourceType = typeof(Resources.POI))]
        public DateTime OpenHour { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        [Display(Name = "CloseHour", ResourceType = typeof(Resources.POI))]
        public DateTime CloseHour { get; set; }

        [Required]
        [RegularExpression(@"[-]?([1-8]?[1-9]|[1-9]0)\.{1}\d{1,6}", ErrorMessageResourceType = typeof(Resources.POI), ErrorMessageResourceName = "POI_Lat_Error")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        [Display(Name = "GPS_Lat", ResourceType = typeof(Resources.POI))]
        public decimal GPS_Lat { get; set; }

        [Required]
        [RegularExpression(@"[-]?([1]?[1-7][1-9]|[1]?[1-8][0]|[1-9]?[0-9])\.{1}\d{1,6}", ErrorMessageResourceType = typeof(Resources.POI), ErrorMessageResourceName = "POI_Long_Error")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        [Display(Name = "GPS_Long", ResourceType = typeof(Resources.POI))]
        public decimal GPS_Long { get; set; }

        [Required]
        [RegularExpression(@"[1][0-9][0-9]|[1-2][1-6][0-9]|[1][5-9]|[2-9][0-9]|270", ErrorMessageResourceType = typeof(Resources.POI), ErrorMessageResourceName = "POI_Altitude_Error")]
        [Display(Name = "Altitude", ResourceType = typeof(Resources.POI))]
        public int Altitude { get; set; }

        [Display(Name = "Creator", ResourceType = typeof(Resources.POI))]
        public string Creator { get; set; }

        [Display(Name = "Approved", ResourceType = typeof(Resources.POI))]
        public string Approved { get; set; }

        [Display(Name = "ConnectedPOIs", ResourceType = typeof(Resources.POI))]
        public virtual ICollection<POI> ConnectedPOIs { get; set; }

        public virtual ICollection<Hashtag> Hashtags { get; set; }

        public POI()
        {
            ConnectedPOIs = new List<POI>();
            Hashtags = new List<Hashtag>();
        }
    }
}
