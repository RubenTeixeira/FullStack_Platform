using DBAPI.DAL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary.DTO;
using ClassLibrary.Models;
using System.Threading.Tasks;
using DBAPI.Models;
using System.Data.Entity;

namespace DBAPI.DAL.Repositories
{
    public class PercursoRepository : IDisposable, IPercursoRepository
    {

        ApplicationDbContext context;

        public PercursoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Percurso ConvertDTOToModel(PercursoDTO dto)
        {
            throw new NotImplementedException();
        }

        public List<PercursoDTO> ConvertModelListToDTO(List<Percurso> modelList)
        {
            List<PercursoDTO> list = new List<PercursoDTO>();
            foreach (Percurso percurso in modelList)
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

                list.Add(dto);
            }
            return list;
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

        public async Task<int> CreatePercurso(Percurso percurso)
        {
            foreach (POI connected in percurso.PercursoPOIs) { context.Entry(connected).State = EntityState.Unchanged; }

            context.Percursos.Add(percurso);
            await context.SaveChangesAsync();
            context.Entry(percurso).Collection(x => x.PercursoPOIs).Load();

            return 1;
        }

        public async Task<bool> DeletePercurso(int percursoID)
        {

            Percurso percurso = await FindPercursoByIDAsync(percursoID);

            if (percurso == null)
            {
                return false;
            }

            context.Database.ExecuteSqlCommand("delete from Percurso_POI where PercursoID = {0}", percurso.PercursoID);
            await context.SaveChangesAsync();

            context.Percursos.Remove(percurso);
            await context.SaveChangesAsync();

            return true;
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

        public Task<List<Percurso>> FindPercursosByUser(string user)
        {
            return context.Percursos.Include(p => p.PercursoPOIs).Where( p => p.Creator == user).ToListAsync();
        }

        public Task<Percurso> FindPercursoByIDAsync(int? percursoID)
        {
            return context.Percursos.FindAsync(percursoID);
        }

        public Task<List<Percurso>> FindPercursos()
        {
            return context.Percursos.Include(p => p.PercursoPOIs).ToListAsync();
        }

        public bool PercursoExists(int id)
        {
            return context.Percursos.Count(e => e.PercursoID == id) > 0;
        }

        public async Task<bool> UpdatePercurso(Percurso percurso)
        {
            context.Database.ExecuteSqlCommand("delete from Percurso_POI where PercursoID = {0}", percurso.PercursoID);
            await context.SaveChangesAsync();

            context.Entry(percurso).State = EntityState.Modified;

            foreach (POI connected in percurso.PercursoPOIs)
            {
                context.Database.ExecuteSqlCommand("Insert Into Percurso_POI (PercursoID,POIID)" +
                    "Values('" + percurso.PercursoID + "','" + connected.POIID + "')");
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}