using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ViewModels
{
    public class ConnectedPOIViewModel
    {
        public IEnumerable<int> SelectedItemIds { get; set; }

        public IEnumerable<POI> Items { get; set; }
    }
}
