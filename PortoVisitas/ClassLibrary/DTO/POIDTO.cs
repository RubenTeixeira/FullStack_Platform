using ClassLibrary.Models;
using System;
using System.Collections.Generic;
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
        public decimal GPS_Lat { get; set; }
        public decimal GPS_Long { get; set; }
        public UserDTO Creator { get; set; }
        public UserDTO Approved { get; set; }
        public List<POIConnectedDTO> ConnectedPOI { get; set; }

        public POIDTO()
        {
            ConnectedPOI = new List<POIConnectedDTO>();
        }

    }
}
