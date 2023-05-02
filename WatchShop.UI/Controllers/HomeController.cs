using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WatchShop.UI.Models;
using Microsoft.AspNetCore.Authorization;
using WatchShop.BLL.Interfaces;

namespace WatchShop.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWatchesBLL _watchesBLL;

        public HomeController(ILogger<HomeController> logger, IWatchesBLL watchesBLL)
        {
            _logger = logger;
            _watchesBLL = watchesBLL;
        }

        public async Task<IActionResult> Index()
        {
            var watches = await _watchesBLL.GetAllAsync();
            var watchesModel = watches.Select(WatchesModel.FromEntity).ToList();
            return View(watchesModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}