using WatchShop.Entities;

namespace WatchShop.BLL.Interfaces
{
    public interface IUsersBLL
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIDAsync(int id);
        Task<User> GetByLogin(string login);
        Task<bool> Add(string login, string password);
        Task<bool> Check(string login, string password);
        Task<bool> ChangePass(string login, string password);
        Task<bool> UsedPass(string login);
        Task<bool?> IsUsedPass(string login);
    }
}
