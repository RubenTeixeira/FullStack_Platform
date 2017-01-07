using ClassLibrary.DTO;
using ClassLibrary.Models;
using DBAPI.DAL.RepositoryInterfaces;
using DBAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DBAPI.DAL.Repositories
{
    public class POIRepository : IDisposable, IPOIRepository
    {
        ApplicationDbContext context;

        public POIRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<List<POI>> FindPOIs()
        {
            return context.POIs.Include(p => p.ConnectedPOIs).ToListAsync();
        }

        public Task<POI> FindPOIByIDAsync(int? poiID)
        {
            return context.POIs.FindAsync(poiID);
        }

        public async Task<int> CreatePOI(POI poi)
        {
            //ApplicationUser user = context.Users.Find(userID);
            //poi.Creator= user;

            foreach (POI connected in poi.ConnectedPOIs) { context.Entry(connected).State = EntityState.Unchanged; }

            context.POIs.Add(poi);
            await context.SaveChangesAsync();
            context.Entry(poi).Collection(x => x.ConnectedPOIs).Load();

            return 1;
        }

        public async Task<bool> DeletePOI(int poiID)
        {
            POI poi = await FindPOIByIDAsync(poiID);

            if (poi == null)
            {
                return false;
            }

            context.Database.ExecuteSqlCommand("delete from Caminho where POIID = {0}", poi.POIID);
            await context.SaveChangesAsync();

            context.Database.ExecuteSqlCommand("delete from Caminho where ConnectedPOIID = {0}", poi.POIID);
            await context.SaveChangesAsync();

            context.POIs.Remove(poi);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePOI(POI poi)
        {

            context.Database.ExecuteSqlCommand("delete from Caminho where POIID = {0}", poi.POIID);
            await context.SaveChangesAsync();

            context.Entry(poi).State = EntityState.Modified;

            foreach (POI connected in poi.ConnectedPOIs) { 
                context.Database.ExecuteSqlCommand("Insert Into Caminho (POIID,ConnectedPOIID)" +
                    "Values('" + poi.POIID + "','" + connected.POIID + "')");
            }

            await context.SaveChangesAsync();

            return true;
        }

        public bool POIExists(int id)
        {
            return context.POIs.Count(e => e.POIID == id) > 0;
        }


        public POI ConvertDTOToModel(POIDTO dto)
        {
            throw new NotImplementedException();
        }

        public POIDTO ConvertModelToDTO(POI poi)
        {
            var dto = new POIDTO()
            {
                ID = poi.POIID,
                Name = poi.Name,
                Description = poi.Description,
                OpenHour = poi.OpenHour,
                CloseHour = poi.CloseHour,
                GPS_Lat = poi.GPS_Lat,
                GPS_Long = poi.GPS_Long,
                Creator = poi.Creator,
                Approved = poi.Approved
            };


            foreach (POI connected in poi.ConnectedPOIs)
            {
                POIConnectedDTO poiCon = new POIConnectedDTO();

                poiCon.ID = connected.POIID;
                poiCon.Name = connected.Name;

                dto.ConnectedPOI.Add(poiCon);
            }

            return dto;
        }

        public List<POIDTO> ConvertModelListToDTO(List<POI> modelList)
        {
            List<POIDTO> list = new List<POIDTO>();
            foreach (POI poi in modelList)
            {
                var dto = new POIDTO()
                {
                    ID = poi.POIID,
                    Name = poi.Name,
                    Description = poi.Description,
                    OpenHour = poi.OpenHour,
                    CloseHour = poi.CloseHour,
                    GPS_Lat = poi.GPS_Lat,
                    GPS_Long = poi.GPS_Long,
                    Creator = poi.Creator,
                    Approved = poi.Approved
                };

                foreach (POI connected in poi.ConnectedPOIs)
                {
                    POIConnectedDTO poiCon = new POIConnectedDTO();

                    poiCon.ID = connected.POIID;
                    poiCon.Name = connected.Name;

                    dto.ConnectedPOI.Add(poiCon);
                }

                list.Add(dto);
            }
            return list;
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
    }
}