using System;
using System.Collections.Generic;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    public class InMemoryCacheStrategy : ISettingCacheStrategy
    {
        static Dictionary<string, CacheObject> _cache;
        static Dictionary<string, CacheObject> Cache
        {
            get
            {
                if (_cache == null)
                    _cache = new Dictionary<string, CacheObject>();

                return _cache;
            }
        }
        public void Add(AppSetting appSetting, string cacheKey)
        {
            this.Add(appSetting, cacheKey, TimeSpan.FromMinutes(5));
        }

        public void Add(AppSetting appSetting, string cacheKey, TimeSpan expires)
        {
            if (!Cache.ContainsKey(cacheKey))
            {
                Cache.Add(cacheKey, new CacheObject(appSetting, expires));
            }
            else
            {
                Cache[cacheKey] = new CacheObject(appSetting, expires);
            }
        }

        public AppSetting Get(string cacheKey)
        {
            if (!Cache.ContainsKey(cacheKey))
                return null;

            var caceObj = Cache[cacheKey];

            // Cache is expired
            if (caceObj.Expires < DateTime.UtcNow.TimeOfDay)
                return null;

            return caceObj.Setting;
        }

        public void Remove(string cacheKey)
        {
            if (Cache.ContainsKey(cacheKey))
                Cache.Remove(cacheKey);
        }

        class CacheObject
        {
            public AppSetting Setting { get; private set; }

            public TimeSpan Expires { get; private set; }

            /// <summary>
            /// Instantiate an object of type <see cref="CacheObject"/> with default time expires is 5 minutes
            /// </summary>
            /// <param name="setting">Setting object</param>
            public CacheObject(AppSetting setting)
                : this(setting, TimeSpan.FromMinutes(5))
            {
                
            }

            public CacheObject(AppSetting setting, TimeSpan expires)
            {
                Setting = setting;
                Expires = DateTime.UtcNow.Add(expires).TimeOfDay;
            }
        }
    }
}
