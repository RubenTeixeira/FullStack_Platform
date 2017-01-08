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
    public class HashtagRepository : IDisposable, IHashtagRepository
    {

        ApplicationDbContext context;

        public HashtagRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Hashtag ConvertDTOToModel(HashtagDTO dto)
        {
            throw new NotImplementedException();
        }

        public List<HashtagDTO> ConvertModelListToDTO(ICollection<Hashtag> hashtags)
        {
            List<HashtagDTO> list = new List<HashtagDTO>();
            foreach (Hashtag tag in hashtags)
            {
                HashtagDTO dto = this.ConvertModelToDTO(tag);
                list.Add(dto);
            }
            return list;
        }

        public HashtagDTO ConvertModelToDTO(Hashtag tag)
        {
            var dto = new HashtagDTO();
            dto.HashtagID = tag.HashtagID;
            dto.Text = tag.Text;
            return dto;
        }

        public async Task<int> CreateHashtag(Hashtag hashtag)
        {
            context.Hashtags.Add(hashtag);
            return await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteHashtag(int hashtagID)
        {
            Hashtag tag = await this.FindHashtagByIDAsync(hashtagID);
            if (tag != null)
            {
                context.Hashtags.Remove(tag);
                return true;
            }

            return false;
        }

        public async Task<Hashtag> FindHashtagByIDAsync(int? hashtagID)
        {
            return await context.Hashtags.FindAsync(hashtagID);
        }

        public async Task<Hashtag> FindHashtagAsync(string text)
        {
            return await context.Hashtags.FirstAsync(h => h.Text.Equals(text));
        }

        public Task<List<Hashtag>> FindHashtags()
        {
            return context.Hashtags.ToListAsync();
        }

        public bool HashtagExists(int id)
        {
            return context.Hashtags.Count(h => h.HashtagID == id) > 0;
        }

        public async Task<int> UpdateHashtag(Hashtag hashtag)
        {
            context.Entry(hashtag).State = EntityState.Modified;
            return await context.SaveChangesAsync();
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