using WatchShop.Entities;

namespace WatchShop.UI.Models
{
    public class WatchesModel
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public string? Picture { get; set; }
        public float Rating { get; set; }
        public static WatchesModel FromEntity(Watch watch)
        {
            return new WatchesModel
            {
                ID = watch.ID,
                Name = watch.Name,
                Picture = watch.Picture,
                Price = watch.Price,
                Rating = watch.Rating
            };
        }
    }
}
