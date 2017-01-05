﻿using ClassLibrary.DTO;
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
            poi.GPS_Lat = dto.GPS_Lat;
            poi.GPS_Long = dto.GPS_Long;

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
                poi.Creator.Id = dto.Creator.ID;
                poi.Creator.UserName = dto.Creator.Username;
            }

            if (dto.Approved != null)
            {
                poi.Approved.Id = dto.Approved.ID;
                poi.Approved.UserName = dto.Approved.Username;
            }

            return poi;
        }
    }
}