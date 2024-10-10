using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace eshop_orderapi.Business.Helpers
{
    public class MemoryCacheHelper
    {
        public static IMemoryCacheService memoryCache;

        public static void Initialize()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            memoryCache = new MemoryCacheService(cache);
        }
    }

    #region Interface

    public interface IMemoryCacheService
    {
        void Set(string key, object value);

        T Get<T>(string key);

        void DeleteStartWidth(string key);
    }

    #endregion Interface

    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;

        private static MemoryCacheEntryOptions MemoryCacheEntryOptions = new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.NeverRemove);

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Use Set method using MemoryCacheHelper.memoryCache.Set(key, value);
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            //If value already exist then first delete
            if (_memoryCache.TryGetValue(key, out object objValue))
            {
                _memoryCache.Remove(key);
            }
            _memoryCache.Set(key, value, MemoryCacheEntryOptions);
        }

        public T Get<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T objValue))
            {
                return objValue;
            }
            return default(T);
        }

        public void DeleteStartWidth(string key)
        {
            var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var collection = field.GetValue(_memoryCache) as ICollection;
            var items = new List<string>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    var methodInfo = item.GetType().GetProperty("Key");
                    var val = methodInfo.GetValue(item);
                    if (val.ToString().StartsWith(key))
                    {
                        items.Add(val.ToString());
                    }
                }
            }
            foreach (var item in items)
            {
                _memoryCache.Remove(item);
            }
        }
    }
}