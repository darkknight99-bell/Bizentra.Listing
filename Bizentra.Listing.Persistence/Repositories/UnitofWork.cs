using Bizentra.Listing.Application.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bizentra.Listing.Persistence.Repositories
{
    public class UnitofWork : IUnitofWork
    {
        private readonly BizentraListingDbContext _context;

        public UnitofWork(BizentraListingDbContext dataEngineDbContext)
        {
            this._context = dataEngineDbContext;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task Refresh()
        {
            try
            {
                await _context.Database.CurrentTransaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> SubmitChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
