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
        HashtagRepository tagRepo = null;

        public POIRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<POI>> FindPOIs()
        {
            return await context.POIs.Include(p => p.ConnectedPOIs).Include(p => p.Hashtags).Where(p => p.Approved != null && p.Approved!="no").ToListAsync();
        }

        public async Task<List<POI>> FindPOIsToApprove()
        {
            return await context.POIs.Include(p => p.ConnectedPOIs).Include(p => p.Hashtags).Where(p => p.Approved == null).ToListAsync();
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

            foreach (Hashtag tag in poi.Hashtags)
            {   // Wont duplicate hashtags
                Hashtag existingTag = await getHashtagRepository().FindHashtagAsync(tag.Text);
                if (existingTag != null) {
                    tag.HashtagID = existingTag.HashtagID;
                    context.Entry(tag).State = EntityState.Unchanged;
                }
            }

            context.POIs.Add(poi);
            await context.SaveChangesAsync();
            context.Entry(poi).Collection(x => x.ConnectedPOIs).Load();
            context.Entry(poi).Collection(x => x.Hashtags).Load();

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
            context.Database.ExecuteSqlCommand("delete from POIHashtag where POI_POIID = {0}", poi.POIID);
            await context.SaveChangesAsync();

            foreach (Hashtag tag in poi.Hashtags)
            {
                Hashtag existingTag = null;
                existingTag = await getHashtagRepository().FindHashtagAsync(tag.Text);
                if (existingTag == null)
                {
                    context.Hashtags.Add(tag);
                } 
                else
                {
                    tag.HashtagID = existingTag.HashtagID;
                    context.Entry(tag).State = EntityState.Unchanged;
                }
                await context.SaveChangesAsync();

                context.Database.ExecuteSqlCommand("Insert Into POIHashtag (POI_POIID,Hashtag_HashtagID)" +
                    "Values('" + poi.POIID + "','" + tag.HashtagID + "')");
            }

            context.Entry(poi).State = EntityState.Modified;

            foreach (POI connected in poi.ConnectedPOIs)
            {
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
            POIDTO dto = new POIDTO()
            {
                ID = poi.POIID,
                Name = poi.Name,
                Description = poi.Description,
                OpenHour = poi.OpenHour,
                CloseHour = poi.CloseHour,
                VisitDuration = poi.VisitDuration,
                GPS_Lat = poi.GPS_Lat,
                GPS_Long = poi.GPS_Long,
                Altitude = poi.Altitude,
                Creator = poi.Creator,
                Approved = poi.Approved,
                ConnectedPOI = new List<POIConnectedDTO>(),
                Hashtags = getHashtagRepository().ConvertModelListToDTO(poi.Hashtags)
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



        public List<POIDTO> ConvertModelListToDTO(ICollection<POI> modelList)
        {
            List<POIDTO> list = new List<POIDTO>();
            foreach (POI poi in modelList)
            {
                var dto = this.ConvertModelToDTO(poi);
                list.Add(dto);
            }
            return list;
        }

        private HashtagRepository getHashtagRepository()
        {
            if (this.tagRepo == null)
                this.tagRepo = new HashtagRepository(this.context);
            return this.tagRepo;
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
