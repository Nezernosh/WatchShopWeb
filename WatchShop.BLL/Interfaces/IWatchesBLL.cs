using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.Entities;

namespace WatchShop.BLL.Interfaces
{
    public interface IWatchesBLL
    {
        public Task<List<Watch>> GetAllAsync();
        public Task<Watch> GetByIDAsync(int ID);
    }
}
