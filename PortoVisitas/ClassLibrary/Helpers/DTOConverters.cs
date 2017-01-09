using ClassLibrary.DTO;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Helpers
{
    public class DTOConverters
    {
        public static POI ConvertDTOToModel(POIDTO dto)
        {
            POI poi = new POI();

            poi.POIID = dto.ID;
            poi.Name = dto.Name;
            poi.Description = dto.Description;
            poi.OpenHour = dto.OpenHour;
            poi.CloseHour = dto.CloseHour;
            poi.GPS_Lat = dto.GPS_Lat;
            poi.GPS_Long = dto.GPS_Long;
            poi.Altitude = dto.Altitude;
            poi.ConnectedPOIs = new List<POI>();
            poi.Hashtags = new List<Hashtag>();

            if(dto.ConnectedPOI != null) { 
                foreach (POIConnectedDTO poiDTO in dto.ConnectedPOI)
                {
                    POI poiCon = new POI();

                    poiCon.POIID = poiDTO.ID;
                    poiCon.Name = poiDTO.Name;

                    poi.ConnectedPOIs.Add(poiCon);
                }
            }

            if (dto.Creator != null)
            {
                poi.Creator = dto.Creator;
            }

            if (dto.Approved != null)
            {
                poi.Approved= dto.Approved;
            }

            if (dto.Hashtags != null)
            {
                foreach (HashtagDTO tagDTO in dto.Hashtags)
                {
                    Hashtag tag = new Hashtag();
                    tag.HashtagID = tagDTO.HashtagID;
                    tag.Text = tagDTO.Text;
                    tag.ReferencedPOIs = new List<POI>();
                    poi.Hashtags.Add(tag);
                }
            }

            return poi;
        }

        public static Percurso ConvertDTOToModel(PercursoDTO dto)
        {
            Percurso p = new Percurso();

            p.PercursoID = dto.ID;
            p.Name = dto.Name;
            p.Description = dto.Description;

            if (dto.PercursoPOI != null)
            {
                foreach (POIDTO poiDTO in dto.PercursoPOI)
                {
                    POI poiCon = new POI();

                    poiCon.POIID = poiDTO.ID;
                    poiCon.Name = poiDTO.Name;
                    poiCon.Description = poiDTO.Description;
                    poiCon.GPS_Lat = poiDTO.GPS_Lat;
                    poiCon.GPS_Long = poiDTO.GPS_Long;

                    p.PercursoPOIs.Add(poiCon);
                }
            }

            if (dto.Creator != null)
            {
                p.Creator = dto.Creator;
            }

            return p;
        }
    }
}
