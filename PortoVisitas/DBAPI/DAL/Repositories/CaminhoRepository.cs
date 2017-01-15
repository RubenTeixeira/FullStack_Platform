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
            return await context.Database.SqlQuery<CaminhoDTO>("select c.POIID,c.ConnectedPOIID from Caminho c, POI p where c.POIID = p.POIID and p.Approved IS NOT NULL and p.Approved NOT LIKE 'no' and c.ConnectedPOIID NOT IN (select q.ConnectedPOIID from Caminho q, POI d where q.ConnectedPOIID = d.POIID and (d.Approved IS NULL or d.Approved LIKE 'no'))").ToListAsync();
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