using ClassLibrary.DTO;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DBAPI.DAL.RepositoryInterfaces
{

    public interface IPercursoRepository : IDisposable
    {
        Task<List<Percurso>> FindPercursos();
        Task<List<Percurso>> FindPercursosByUser(string user);
        Task<Percurso> FindPercursoByIDAsync(int? percursoID);
        Task<int> CreatePercurso(Percurso percurso);
        Task<bool> DeletePercurso(int percursoID);
        Task<bool> UpdatePercurso(Percurso percurso);
        bool PercursoExists(int id);

        Percurso ConvertDTOToModel(PercursoDTO dto);
        PercursoDTO ConvertModelToDTO(Percurso percurso);
        List<PercursoDTO> ConvertModelListToDTO(List<Percurso> modelList);
    }

}