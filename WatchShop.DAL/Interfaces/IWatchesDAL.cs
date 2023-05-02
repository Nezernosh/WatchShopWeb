using WatchShop.Entities;

namespace WatchShop.DAL.Interfaces
{
    public interface IWatchesDAL
    {
        public Task<List<Watch>> GetAllAsync();
        public Task<Watch> GetByIDAsync(int ID);
    }
}
