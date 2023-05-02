using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchShop.DAL.DbModels;
using WatchShop.DAL.Interfaces;
using WatchShop.Entities;

namespace WatchShop.DAL
{
    public class WatchesDAL : IWatchesDAL
    {
        public async Task<List<Entities.Watch>> GetAllAsync()
        {
            using (var context = new DefaultDbContext())
            {
                return context.Items.ToList().Select(watch => new Entities.Watch()
                {
                    ID = watch.Id,
                    Name = watch.Name,
                    Picture = watch.Picture,
                    Price = (int)watch.Price,
                    Rating = (float)watch.Rating
                }).ToList();
            }
        }
        public async Task<Entities.Watch> GetByIDAsync(int ID)
        {
            using (var context = new DefaultDbContext())
            {
                var watch = context.Items.ToList().Where(watch => watch.Id == ID).FirstOrDefault();
                return new Entities.Watch()
                {
                    ID = watch.Id,
                    Name = watch.Name,
                    Picture = watch.Picture,
                    Price = (int)watch.Price,
                    Rating = (float)watch.Rating
                };
            }
        }
    }
}
