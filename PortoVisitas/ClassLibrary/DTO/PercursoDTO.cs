using System;
using System.Collections.Generic;

namespace ClassLibrary.DTO
{
    public class PercursoDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime FinishHour { get; set; }
        public string PercursoPOIsOrder { get; set; }
        public List<POIDTO> PercursoPOIs { get; set; }

        public PercursoDTO()
        {
            PercursoPOIs = new List<POIDTO>();
        }
    }
}
