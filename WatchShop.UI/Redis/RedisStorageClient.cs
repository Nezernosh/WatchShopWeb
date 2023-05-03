using Newtonsoft.Json;
using WatchShop.Entities;
using StackExchange.Redis;

namespace WatchShop.UI.Redis
{
    public class RedisStorageClient
    {
        private string _host;
        private string _port;
        private string _password;
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisStorageClient()
        {
            this._host = "redis";
            this._port = "6379";
            this._password = null;

            if (string.IsNullOrEmpty(this._password))
            {
                this._connectionMultiplexer = ConnectionMultiplexer.Connect(
                    new ConfigurationOptions
                    {
                        EndPoints = { $"{this._host}:{this._port}" },
                        AbortOnConnectFail = false
                    });
            }
            else
            {
                this._connectionMultiplexer = ConnectionMultiplexer.Connect(
                   new ConfigurationOptions
                   {
                       EndPoints = { $"{this._host}:{this._port}" },
                       Password = this._password
                   });
            }
        }

        public User GetUser(string key)
        {
            return this.Get<User>(key);
        }

        private T Get<T>(string key) where T : class
        {
            var user = _connectionMultiplexer.GetDatabase();
            var stringVal = user.StringGet(key);
            if (string.IsNullOrEmpty(stringVal))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(stringVal);
        }

        public void SetUser(string key, User value, int expirationSeconds)
        {
            this.Set<User>(key, value, TimeSpan.FromSeconds(expirationSeconds));
        }

        private void Set<T>(string key, T value, TimeSpan expiration)
        {
            var user = _connectionMultiplexer.GetDatabase();
            user.StringSet(key, JsonConvert.SerializeObject(value), expiration);
        }

        public void SetGlobalItem<T>(string key, T value, TimeSpan time) where T : class
        {
            this.Set(key, value, time);
        }

        public bool TryGetGlobalItem<T>(string key, out T value) where T : class
        {
            value = this.Get<T>(key);
            if (value == null)
            {
                return false;
            }
            return true;
        }

        public void Remove(string key)
        {
            try
            {
                var user = _connectionMultiplexer.GetDatabase();
                user.KeyDelete(key);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }
    }
}
