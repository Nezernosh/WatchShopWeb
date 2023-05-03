using WatchShop.UI.RabbitMQ;
using WatchShop.UI.Redis;

namespace WatchShop.UI
{
    public class SingleRabbitAndRedis
    {
        private static SingleRabbitAndRedis _instance;
        public static SingleRabbitAndRedis Instance => _instance = _instance ?? new SingleRabbitAndRedis();
        public RabbitMQClient Rabbit => new RabbitMQClient();
        public RedisStorageClient Redis => new RedisStorageClient();

    }
}