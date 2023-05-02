using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WatchShop.BLL.Interfaces;
using WatchShop.UI.Models;

namespace WatchShop.UI.Controllers
{
    public class WatchesController : Controller
    {
        private readonly ILogger<WatchesController> _logger;
        private IWatchesBLL _watchesBLL;

        public WatchesController(ILogger<WatchesController> logger, IWatchesBLL watchesBLL)
        {
            _logger = logger;
            _watchesBLL = watchesBLL;
        }
        public async Task<IActionResult> GetAllAsync()
        {
            var watches = await _watchesBLL.GetAllAsync();
            var watchesModels = watches.Select(WatchesModel.FromEntity).ToList();
            return View(watchesModels);
        }

        public async Task<IActionResult> GetByIDAsync(int ID)
        {
            var watch = await _watchesBLL.GetByIDAsync(ID);
            var watchesModel = WatchesModel.FromEntity(watch);
            return View(watchesModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
