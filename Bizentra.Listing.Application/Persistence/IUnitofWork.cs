namespace Bizentra.Listing.Application.Persistence
{
    public interface IUnitofWork
    {
        //IDbContextTransaction BeginTransaction();

        Task<bool> SubmitChangesAsync();

        Task Refresh();
    }
}
