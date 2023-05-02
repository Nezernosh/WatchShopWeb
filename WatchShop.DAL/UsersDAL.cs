using Microsoft.EntityFrameworkCore;
using WatchShop.DAL.DbModels;
using WatchShop.DAL.Interfaces;
using WatchShop.Entities;

namespace WatchShop.DAL
{
    public class UsersDAL : IUsersDAL
    {
        public async Task<List<Entities.User>> GetAllAsync()
        {
            using (var context = new DefaultDbContext())
            {
                return context.Users.Include(item => item.OneTimePasswords).ToList().Select(user => new Entities.User()
                {
                    ID = user.Id,
                    Login = user.Login,
                    Password = user.OneTimePasswords.FirstOrDefault(item => item.UserId == user.Id).Password
                }).ToList();
            }
        }
        public async Task<Entities.User> GetByIDAsync(int id)
        {
            using (var context = new DefaultDbContext())
            {
                var user = context.Users.Where(user => user.Id == id).FirstOrDefault();
                return new Entities.User()
                {
                    ID = user.Id,
                    Login = user.Login,
                    Password = user.OneTimePasswords.FirstOrDefault(item => item.UserId == user.Id).Password
                };
            }
        }
        public async Task<Entities.User> GetByLogin(string login)
        {
            using (var context = new DefaultDbContext())
            {
                var dalUser = context.Users.FirstOrDefault(item => item.Login == login);
                if (dalUser == null) return null;
                var dalPassword = context.OneTimePasswords.OrderBy(e => e.Id).LastOrDefault(item => item.UserId == dalUser.Id);
                if (dalPassword == null)
                    return new Entities.User()
                    {
                        ID = dalUser.Id,
                        Login = dalUser.Login,
                    };
                else return new Entities.User()
                {
                    ID = dalUser.Id,
                    Login = dalUser.Login,
                    Password = dalPassword.Password
                };
            }
        }
        public async Task<bool> Add(string login, string password)
        {
            using (var context = new DefaultDbContext())
            {
                if (context.Users.FirstOrDefault(item => item.Login == login) == null)
                {
                    var dalUser = new DbModels.User()
                    {
                        Login = login,
                    };
                    context.Users.Add(dalUser);
                    await context.SaveChangesAsync();
                    dalUser = context.Users.FirstOrDefault(item => item.Login == login);
                    var dalPass = new DbModels.OneTimePassword()
                    {
                        Password = password,
                        UserId = dalUser.Id,
                        IsUsed = false
                    };
                    context.OneTimePasswords.Add(dalPass);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> Check(string login, string password)
        {
            using (var context = new DefaultDbContext())
            {
                bool flag = false;
                var dalUser = context.Users.FirstOrDefault(item => item.Login == login);
                var dalPass = context.OneTimePasswords.FirstOrDefault(item => item.UserId == dalUser.Id);
                if (dalPass.Password == password)
                {
                    flag = true;
                }
                return flag;
            }
        }

        public async Task<bool> ChangePass(string login, string password)
        {
            using (var context = new DefaultDbContext())
            {
                bool flag = false;
                var dalUser = context.Users.FirstOrDefault(item => item.Login == login);
                if (dalUser != null)
                {
                    var dalPass = context.OneTimePasswords.FirstOrDefault(item => item.UserId == dalUser.Id);
                    dalPass.Password = password;
                    context.OneTimePasswords.Update(dalPass);
                    await context.SaveChangesAsync();
                    flag = true;
                }
                return flag;
            }
        }
    }
}