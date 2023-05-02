using System.Security.Claims;

namespace WatchShop.UI.Models
{
    public class CustomUserIdentity : ClaimsIdentity
    {
        public string _login { get; set; }
        public string _password { get; set; }
        public CustomUserIdentity(string login, string password, string authenticationType = "Cookie") : base(GetUserClaims(login, password), authenticationType)
        {
            _login = login;
            _password = password;
        }

        private static List<Claim> GetUserClaims(string login, string password)
        {
            var result = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login),
            };
            return result;
        }
    }
}
