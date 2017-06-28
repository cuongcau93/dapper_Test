namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// An abstract class used to instantiate a setting
    /// </summary>
    public interface IAppSettingFactory
    {
        /// <summary>
        /// Gets the name of running application
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Gets the cache-strategy object to cache setting object. Default <see cref="InMemoryCacheStrategy"/> is created
        /// </summary>
        ISettingCacheStrategy CacheStrategy { get;}

        /// <summary>
        /// Gets setting provider which is responsible for get setting value from various sources.
        /// </summary>
        ISettingProvider SettingProvider { get; }

        /// <summary>
        /// Creates an application setting
        /// </summary>
        /// <returns>The setting of application.</returns>
        AppSetting Create();
    }
}
