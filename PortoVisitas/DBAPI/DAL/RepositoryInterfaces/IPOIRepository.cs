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
        Task<List<POI>> FindPOIsToApprove();
        Task<List<POI>> FindUserPOIs(string email);
        Task<POI> FindPOIByIDAsync(int? poiID);
        Task<POI> CreatePOI(POI poi);
        Task<bool> DeletePOI(int poiID);
        Task<bool> UpdatePOI(POI poi);
        bool POIExists(int id);

        POI ConvertDTOToModel(POIDTO dto);
        POIDTO ConvertModelToDTO(POI poi);
        List<POIDTO> ConvertModelListToDTO(ICollection<POI> modelList);
    }

}