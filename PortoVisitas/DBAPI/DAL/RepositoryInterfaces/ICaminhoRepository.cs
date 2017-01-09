using ClassLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DBAPI.DAL.RepositoryInterfaces
{
    public interface ICaminhoRepository: IDisposable
    {
        Task<List<CaminhoDTO>> FindCaminhos();
    }
}