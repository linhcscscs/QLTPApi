using DataAccess.Caching.Interface;
using Force.DeepCloner;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text.Json;

namespace DataAccess.Caching.Redis
{
    public class RedisCacheProvider : ICacheProvider
    {
        private IConnectionMultiplexer _connection;
        private IDatabase _database;
        public RedisCacheProvider(IConnectionMultiplexer connection)
        {
            _connection = connection;
            _database = _connection.GetDatabase();
        }
        private List<IServer> GetAllServer()
        {
            return _connection.GetServers().ToList();
        }
        private T? DeserializeHandle<T>(string jsonObject)
        {
            return JsonConvert.DeserializeObject<T?>(jsonObject, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
        private string SerializeHandle<T>(T? obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }

        public List<string> GetAllKey()
        {
            var keys = new List<string>();
            foreach (var server in GetAllServer())
            {
                var lstKey = server.Keys().Select(s => (string)s).ToList();
                keys.AddRange(lstKey);
            }
            return keys;
        }

        public T? Get<T>(string key)
        {
            var value = _database.StringGet(key);
            return value.HasValue ? DeserializeHandle<T>(value!) : default;
        }

        public object? Get(string key)
        {
            var value = _database.StringGet(key);
            return value.HasValue ? value.ToString() : null;
        }

        public void Set(string key, object data, double cacheTime)
        {
            var serializedData = SerializeHandle(data);
            _database.StringSet(key, serializedData, TimeSpan.FromMinutes(cacheTime));
        }

        public void Set(string key, object data, int cacheTime)
        {
            Set(key, data, (double)cacheTime);
        }

        public bool IsSet(string key)
        {
            return _database.KeyExists(key);
        }

        public void Invalidate(string key)
        {
            _database.KeyDelete(key);
        }

        public void RemoveAll()
        {
            foreach (var server in GetAllServer())
            {
                server.FlushAllDatabases();
            }
        }

        public void Remove(string cacheKey)
        {
            _database.KeyDelete(cacheKey);
        }

        public void Remove(List<string> cacheKeys)
        {
            foreach (var key in cacheKeys)
            {
                _database.KeyDelete(key);
            }
        }

        public void RemoveByFirstName(string name)
        {
            foreach (var server in GetAllServer())
            {
                server.Keys(pattern: $"*{name}*").ToList().ForEach(key => _database.KeyDelete(key));
            }
        }

        public List<string> GetAll()
        {
            var keys = new List<string>();
            foreach (var server in GetAllServer())
            {
                keys.AddRange(server.Keys().Select(k => (string)k).ToList());
            }
            return keys.Distinct().ToList();
        }

        public T? GetByKey<T>(Func<T?> getDataSource,
               string key,
               double cacheTime = CachingTime.CACHING_TIME_DEFAULT_IN_5_MINUTES,
               bool isDeepClone = true)
               where T : new()
        {
            T? result = default;
            if (!IsSet(key))
            {
                result = getDataSource.Invoke();
                Set(key, result, cacheTime);
            }
            else
            {
                try
                {
                    result = Get<T>(key);
                }
                catch
                {
                    Invalidate(key);
                }
            }

            return isDeepClone && result != null ? result.DeepClone() : result;
        }

        public string BuildCachedKey(params object[] objects)
        {
            return string.Join("_", objects);
        }
    }
}
