using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatchShop.UI.Models;
using WatchShop.BLL.Interfaces;
using WatchShop.Entities;

namespace WatchShop.UI.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private IUsersBLL _usersBLL;

        public UsersController(ILogger<UsersController> logger, IUsersBLL usersBLL)
        {
            _logger = logger;
            _usersBLL = usersBLL;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsersModel model)
        {
            if (ModelState.IsValid)
            {
                var login = model.Login;
                var password = model.Password;

                if (login != null && password != null)
                {
                    if (await IsValidAccountData(login, password))
                    {
                        await Authenticate(login);

                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Неверные логин или пароль.");
                }
                else
                    ModelState.AddModelError("", "Все поля должны быть заполнены.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UsersModel model)
        {
            if (ModelState.IsValid)
            {
                var login = model.Login;
                var password = model.Password;

                if (login != null && password != null)
                {
                    if (await _usersBLL.Add(login, password))
                    {
                        await Authenticate(login);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Введён занятый логин");
                    }
                }
                else
                    ModelState.AddModelError("", "Все поля должны быть заполнены.");
            }
            return View();
        }

        private async Task<bool> IsValidAccountData(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;
            if (!(await _usersBLL.Check(login, password)))
                return false;
            return true;
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
