using System.Collections.Generic;

namespace ClassLibrary.DTO
{
    public class PercursoDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public List<POIDTO> PercursoPOI { get; set; }

        public PercursoDTO()
        {
            PercursoPOI = new List<POIDTO>();
        }
    }
}
