using DataAccess.Caching.Interface;
using Force.DeepCloner;
using System.Runtime.Caching;
using System.Text;

namespace DataAccess.Caching.MemCache
{
    public class MemCacheProvider : ICacheProvider
    {
        private ObjectCache Cache
        {
            get { return MemoryCache.Default; }
        }
        public List<string> GetAllKey()
        {
            var cacheKeys = new List<string>();
            cacheKeys = Cache.Select(kvp => kvp.Key).ToList();
            return cacheKeys;
        }
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }
        public object Get(string key)
        {
            return Cache[key];
        }
        public void Set(string key, object data, double cacheTime)
        {
            if (data != null)
            {
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

                if (!string.IsNullOrEmpty(key))
                    Cache.Add(new CacheItem(key, data), policy);
            }
        }
        public void Set(string key, object data, int cacheTime)
        {
            if (data != null)
            {
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);

                if (!string.IsNullOrEmpty(key))
                    Cache.Add(new CacheItem(key, data), policy);
            }
        }
        public bool IsSet(string key)
        {
            return (Cache[key] != null);
        }
        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }
        public void RemoveAll()
        {
            try
            {
                List<string> cacheKeys = Cache.Select(kvp => kvp.Key).ToList();
                foreach (string cacheKey in cacheKeys)
                {
                    Cache.Remove(cacheKey);
                }
            }
            catch { }
        }
        public void Remove(string cacheKey)
        {
            try
            {
                Cache.Remove(cacheKey);
            }
            catch { }
        }

        public void Remove(List<string> cacheKeys)
        {
            try
            {
                foreach (string cacheKey in cacheKeys)
                {
                    Remove(cacheKey);
                }
            }
            catch { }
        }

        public void RemoveByFirstName(string name)
        {
            try
            {
                List<string> cacheKeys =
                    Cache.Where(kvp => kvp.Key.ToLower().Contains(name.ToLower())).Select(kvp => kvp.Key).ToList();
                foreach (string cacheKey in cacheKeys)
                {
                    Cache.Remove(cacheKey);
                }
            }
            catch { }
        }
        public List<string> GetAll()
        {
            var cacheKeys = new List<string>();
            cacheKeys = Cache.Select(kvp => kvp.Key).ToList();
            return cacheKeys;
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
            StringBuilder stringBuilder = new StringBuilder();
            if (objects != null)
            {
                foreach (var item in objects)
                {
                    stringBuilder.AppendFormat("{0}_", item);
                }
            }
            return stringBuilder.ToString().TrimEnd('_');
        }
    }
}
