using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace WX.Helper
{
    public class CacheHelper
    {
        private static Cache _cache = System.Web.HttpRuntime.Cache;

        public static void Add(string key, object value, int timeOut)
        {
            if (!string.IsNullOrWhiteSpace(key) && value != null)
            {
                if (timeOut <= 0)
                {
                    _cache.Add(key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
                else
                {
                    _cache.Add(key, value, null, DateTime.Now.AddMinutes(timeOut), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, null);
                }
            }
        }

        public static T Get<T>(string key)
        {

            object obj = _cache.Get(key);
            if (obj == null)
                return default(T);
            return (T)_cache.Get(key);
        }

        public static Object Get(string key)
        {
            return _cache.Get(key);
        }

        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        public static void Update(string key, object value, int timeOut)
        {
            Remove(key);
            Add(key, value, timeOut);
        }
    }
}
