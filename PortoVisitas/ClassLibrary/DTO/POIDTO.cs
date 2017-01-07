using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.DTO
{
    public class POIDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime OpenHour { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime CloseHour { get; set; }

        public decimal GPS_Lat { get; set; }
        public decimal GPS_Long { get; set; }
        public string Creator { get; set; }
        public string Approved { get; set; }
        public List<POIConnectedDTO> ConnectedPOI { get; set; }

        public POIDTO()
        {
            ConnectedPOI = new List<POIConnectedDTO>();
        }

    }
}
