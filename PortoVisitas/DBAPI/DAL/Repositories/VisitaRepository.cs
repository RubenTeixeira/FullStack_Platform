using DBAPI.DAL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary.Models;
using System.Threading.Tasks;
using DBAPI.Models;
using System.Data.Entity;
using ClassLibrary.DTO;

namespace DBAPI.DAL.Repositories
{
    public class VisitaRepository : IDisposable, IVisitaRepository
    {

        ApplicationDbContext context;

        public VisitaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<Visita> CreateVisita(Visita visita)
        {
            context.Visitas.Add(visita);
            await context.SaveChangesAsync();
            return visita;

        }

        public async Task<bool> DeleteVisita(int visitaID)
        {
            Visita visita = await this.FindVisitaByIDAsync(visitaID);
            if (visita != null)
            {
                context.Visitas.Remove(visita);
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<Visita> FindVisitaByIDAsync(int? visitaID)
        {
            return await context.Visitas.FindAsync(visitaID);
        }

        public async Task<List<Visita>> FindVisitas()
        {
            return await context.Visitas.Include(v => v.Percurso).ToListAsync();
        }

        public async Task<int> UpdateVisita(Visita visita)
        {
            context.Entry(visita).State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }

        public bool VisitaExists(int id)
        {
            return context.Visitas.Count(v => v.VisitaID == id) > 0;
        }


        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public async Task<List<Visita>> FindUserVisitasAsync(string email)
        {
            return await context.Visitas.Include(v => v.Percurso).Where(v => v.Creator.Equals(email)).ToListAsync();
        }

        public Visita ConvertDTOToModel(VisitaDTO dto)
        {
            Visita visita = new Visita();

            visita.VisitaID = dto.VisitaID;
            visita.Creator = dto.Creator;
            visita.Date = dto.Date;
            Percurso percurso = context.Percursos.Find(dto.Percurso.ID);
            visita.Percurso = percurso;
            visita.PercursoID = percurso.PercursoID;

            return visita;
        }

        public VisitaDTO ConvertModelToDTO(Visita visita)
        {
            VisitaDTO dto = new VisitaDTO();
            dto.VisitaID = visita.VisitaID;
            dto.Creator = visita.Creator;
            dto.Date = visita.Date;
            dto.Percurso = this.ConvertModelToDTO(visita.Percurso);
            return dto;
        }

        public List<VisitaDTO> ConvertModelListToDTO(ICollection<Visita> modelList)
        {
            List<VisitaDTO> dtoList = new List<VisitaDTO>();
            foreach (Visita visita in modelList)
            {
                dtoList.Add(this.ConvertModelToDTO(visita));
            }
            return dtoList;
        }

        public PercursoDTO ConvertModelToDTO(Percurso percurso)
        {
            var dto = new PercursoDTO()
            {
                ID = percurso.PercursoID,
                Name = percurso.Name,
                Description = percurso.Description,
                Creator = percurso.Creator,
                StartHour = percurso.StartHour,
                FinishHour = percurso.FinishHour,
                PercursoPOIsOrder = percurso.PercursoPOIsOrder

            };


            foreach (POI connected in percurso.PercursoPOIs)
            {
                POIDTO poiCon = new POIDTO();

                poiCon.ID = connected.POIID;
                poiCon.Name = connected.Name;

                dto.PercursoPOIs.Add(poiCon);
            }

            return dto;
        }
    }
}