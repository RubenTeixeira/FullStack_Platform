using ClassLibrary.DTO;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAPI.DAL.RepositoryInterfaces
{
    public interface IVisitaRepository : IDisposable
    {

        Task<List<Visita>> FindVisitas();
        Task<Visita> FindVisitaByIDAsync(int? visitaID);
        Task<Visita> CreateVisita(Visita visita);
        Task<bool> DeleteVisita(int visitaID);
        Task<int> UpdateVisita(Visita visita);
        bool VisitaExists(int id);
        Task<List<Visita>> FindUserVisitasAsync(string email);

        Visita ConvertDTOToModel(VisitaDTO dto);
        VisitaDTO ConvertModelToDTO(Visita visita);
        List<VisitaDTO> ConvertModelListToDTO(ICollection<Visita> modelList);
    }
}
