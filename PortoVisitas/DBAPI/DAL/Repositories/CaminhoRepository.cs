using DBAPI.DAL.RepositoryInterfaces;
using DBAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary.DTO;
using System.Threading.Tasks;

namespace DBAPI.DAL.Repositories
{
    public class CaminhoRepository : IDisposable, ICaminhoRepository
    {

        ApplicationDbContext context;

        public CaminhoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<CaminhoDTO>> FindCaminhos()
        {
            return await context.Database.SqlQuery<CaminhoDTO>("select POIID,ConnectedPOIID from Caminho").ToListAsync();
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