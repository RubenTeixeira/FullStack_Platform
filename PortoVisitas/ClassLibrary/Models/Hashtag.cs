using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Models
{
    public class Hashtag
    {

        public int HashtagID { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "varchar")]
        [Index(IsUnique = true)]
        public string Text { get; set; }

        public virtual ICollection<POI> ReferencedPOIs { get; set; }

        public Hashtag()
        {
            ReferencedPOIs = new List<POI>();
        }
    }
}
