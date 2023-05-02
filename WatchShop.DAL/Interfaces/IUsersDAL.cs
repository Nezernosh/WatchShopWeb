using WatchShop.Entities;

namespace WatchShop.DAL.Interfaces
{
    public interface IUsersDAL
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIDAsync(int id);
        Task<User> GetByLogin(string login);
        Task<bool> Add(string login, string password);
        Task<bool> Check(string login, string password);
        Task<bool> ChangePass(string login, string password);
    }
}
