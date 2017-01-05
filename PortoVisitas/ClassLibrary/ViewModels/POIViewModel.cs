using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels
{
    public class POIViewModel
    {
        public POI poi { get; set; }

        public ConnectedPOIViewModel connectedPoi { get; set; }
    }
}
