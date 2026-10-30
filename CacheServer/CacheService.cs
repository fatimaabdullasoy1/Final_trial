
using System.Collections.Generic;
using Final_Project_Cache;
using Final_Project_Cache;

namespace CacheServer
{
    public class CacheService
    {
        private Dictionary<string, Product> cache =
            new Dictionary<string, Product>();

        public Product Get(string key)
        {
            if (cache.ContainsKey(key))
            {
                return cache[key];
            }

            return null!;
        }

        public void Set(string key, Product product)
        {
            cache[key] = product;
        }

        public void Remove(string key)
        {
            if (cache.ContainsKey(key))
            {
                cache.Remove(key);
            }
        }
    }
}


