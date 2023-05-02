using WatchShop.BLL.Interfaces;
using WatchShop.DAL.Interfaces;
using WatchShop.Entities;

namespace WatchShop.BLL
{
    public class UsersBLL : IUsersBLL
    {
        private IUsersDAL _usersDAL;

        public UsersBLL(IUsersDAL usersDal)
        {
            _usersDAL = usersDal;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _usersDAL.GetAllAsync();
        }

        public async Task<User> GetByIDAsync(int id)
        {
            return await _usersDAL.GetByIDAsync(id);
        }

        public async Task<User> GetByLogin(string login)
        {
            return await _usersDAL.GetByLogin(login);
        }
        public async Task<bool> Add(string login, string password)
        {
            return await _usersDAL.Add(login, password);
        }
        public async Task<bool> Check(string login, string password)
        {
            return await _usersDAL.Check(login, password);
        }

        public async Task<bool> ChangePass(string login, string password)
        {
            return await _usersDAL.ChangePass(login, password);
        }
    }
}