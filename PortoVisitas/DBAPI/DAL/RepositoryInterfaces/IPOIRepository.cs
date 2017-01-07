using ClassLibrary.DTO;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DBAPI.DAL.RepositoryInterfaces
{

    public interface IPOIRepository : IDisposable
    {
        Task<List<POI>> FindPOIs();
        Task<POI> FindPOIByIDAsync(int? poiID);
        Task<int> CreatePOI(POI poi);
        Task<bool> DeletePOI(int poiID);
        Task<bool> UpdatePOI(POI poi);
        bool POIExists(int id);

        POI ConvertDTOToModel(POIDTO dto);
        POIDTO ConvertModelToDTO(POI poi);
        List<POIDTO> ConvertModelListToDTO(List<POI> modelList);
    }

}