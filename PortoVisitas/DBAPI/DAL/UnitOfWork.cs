using DBAPI.DAL.Repositories;
using DBAPI.DAL.RepositoryInterfaces;
using DBAPI.Models;
using System;
using System.Threading.Tasks;

namespace DBAPI.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        private IPOIRepository poiRepository;

        public IPOIRepository POIRepository
        {
            get
            {
                if (this.poiRepository == null)
                {
                    this.poiRepository = new POIRepository(context);
                }
                return poiRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}