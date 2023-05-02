using WatchShop.Entities;

namespace WatchShop.UI.Models
{
    public class UsersModel
    {
        public int ID { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }

        public static UsersModel FromEntity(User user)
        {
            return new UsersModel()
            {
                ID = user.ID,
                Login = user.Login,
                Password = user.Password
            };
        }
    }
}
