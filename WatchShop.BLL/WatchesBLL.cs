using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.BLL.Interfaces;
using WatchShop.Entities;
using WatchShop.DAL;
using WatchShop.DAL.Interfaces;

namespace WatchShop.BLL
{
    public class WatchesBLL : IWatchesBLL
    {
        private IWatchesDAL _watchesDAL;
        public WatchesBLL(IWatchesDAL watchesDAL)
        {
            _watchesDAL = watchesDAL;
        }
        public async Task<List<Watch>> GetAllAsync()
        {
            return await _watchesDAL.GetAllAsync();
        }
        public async Task<Watch> GetByIDAsync(int ID)
        {
            return await _watchesDAL.GetByIDAsync(ID);
        }
    }
}
