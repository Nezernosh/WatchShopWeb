using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WatchShop.UI.Models;
using WatchShop.BLL.Interfaces;
using WatchShop.Entities;
using WatchShop.UI.RabbitMQ;
using WatchShop.UI.Redis;
using WatchShop.TelegramBot;
using WatchShop.BLL;

namespace WatchShop.UI.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private IUsersBLL _usersBLL;
        public RabbitMQClient rabbit { get; set; }
        //public RedisStorageClient redis { get; set; }

        public UsersController(ILogger<UsersController> logger, IUsersBLL usersBLL)
        {
            _logger = logger;
            _usersBLL = usersBLL;
            rabbit = SingleRabbitAndRedis.Instance.Rabbit;
            //redis = SingleRabbitAndRedis.Instance.Redis;
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
                        var isUsed = await _usersBLL.IsUsedPass(login);
                        if (isUsed != null && isUsed == false)
                        {
                            await Authenticate(login, password);
                            rabbit.Send($"Пользователь {login} успешно зашёл в систему.");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            string err = "Действие пароля истекло.";
                            ModelState.AddModelError("", err);
                            rabbit.Send($"{err} Для пользователя {login}");
                        }
                    }
                    else
                    {
                        string err = "Неверные логин или пароль.";
                        ModelState.AddModelError("", err);
                        rabbit.Send(err);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Все поля должны быть заполнены.");
                }
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
                        await Authenticate(login, password);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string err = "Введён занятый логин.";
                        ModelState.AddModelError("", err);
                        rabbit.Send($"{err} - {login}");
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

        private async Task Authenticate(string login, string password)
        {
            var identity = new CustomUserIdentity(login, password);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            await _usersBLL.UsedPass(login);
        }
    }
}
