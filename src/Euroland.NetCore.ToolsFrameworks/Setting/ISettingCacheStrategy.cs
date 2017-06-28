using System;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// Interface to manage cache of setting
    /// </summary>
    public interface ISettingCacheStrategy
    {
        /// <summary>
        /// Adds an app settting to the cache
        /// </summary>
        /// <param name="appSetting">Object to be cached</param>
        /// <param name="cacheKey">The key of cache</param>
        void Add(AppSetting appSetting, string cacheKey);

        /// <summary>
        /// Adds an app settting to the cache
        /// </summary>
        /// <param name="appSetting">Object to be cached</param>
        /// <param name="cacheKey">The key of cache</param>
        /// <param name="expires">The time to a cache will be expired</param>
        void Add(AppSetting appSetting, string cacheKey, TimeSpan expires);

        /// <summary>
        /// Gets a setting object from cache
        /// </summary>
        /// <param name="cacheKey">The key of cache to get</param>
        /// <returns>An object of <see cref="AppSetting"/>. Rerturns Null if not found</returns>
        AppSetting Get(string cacheKey);

        /// <summary>
        /// Removes a setting from cache by providing the key of cache
        /// </summary>
        /// <param name="cacheKey">The key of cache</param>
        void Remove(string cacheKey);
    }
}
