using ClassLibrary.DTO;
using ClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBAPI.DAL.RepositoryInterfaces
{
    public interface IHashtagRepository : IDisposable
    {
        Task<List<Hashtag>> FindHashtags();
        Task<Hashtag> FindHashtagAsync(string text);
        Task<Hashtag> FindHashtagByIDAsync(int? hashtagID);
        Task<int> CreateHashtag(Hashtag hashtag);
        Task<bool> DeleteHashtag(int hashtagID);
        Task<int> UpdateHashtag(Hashtag hashtag);
        bool HashtagExists(int id);

        Hashtag ConvertDTOToModel(HashtagDTO dto);
        HashtagDTO ConvertModelToDTO(Hashtag tag);
        List<HashtagDTO> ConvertModelListToDTO(ICollection<Hashtag> hashtags);
    }
}
