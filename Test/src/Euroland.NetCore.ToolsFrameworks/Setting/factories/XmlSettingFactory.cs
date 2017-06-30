using System;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    public class XmlSettingFactory : IAppSettingFactory
    {
        /// <summary>
        /// Gets the name of running application
        /// </summary>
        public string ApplicationName { get; private set; }

        /// <summary>
        /// Gets the path builder object to generate (build) the path of xml files
        /// </summary>
        public ISettingFileBuilder XmlPathBuilder { get; private set; }

        /// <summary>
        /// Gets the cache-strategy object to cache setting object. Default <see cref="InMemoryCacheStrategy"/> is created
        /// </summary>
        public ISettingCacheStrategy CacheStrategy { get; private set; }

        /// <summary>
        /// Gets setting provider which is responsible for get setting value from various sources.
        /// </summary>
        public ISettingProvider SettingProvider { get; private set; }

        /// <summary>
        /// Instantiate an object for the setting factory
        /// </summary>
        /// <param name="applicationName">Name of application</param>
        /// <param name="xmlPathBuilder">Path builder object which is used to generate (build) the path of xml files</param>
        public XmlSettingFactory(string applicationName, ISettingFileBuilder xmlPathBuilder)
            : this(applicationName, xmlPathBuilder, null, null)
        {

        }

        /// <summary>
        /// Instantiate an object for the setting factory
        /// </summary>
        /// <param name="applicationName">Name of application</param>
        /// <param name="xmlPathBuilder">Path builder object which is used to generate (build) the path of xml files</param>
        /// <param name="cacheStrategy">Cache-strategy object to cache setting object</param>
        public XmlSettingFactory(string applicationName, ISettingFileBuilder xmlPathBuilder, ISettingCacheStrategy cacheStrategy)
            : this(applicationName, xmlPathBuilder, cacheStrategy, null)
        {

        }

        /// <summary>
        /// Instantiate an object for the setting factory
        /// </summary>
        /// <param name="applicationName">Name of application</param>
        /// <param name="xmlPathBuilder">Path builder object which is used to generate (build) the path of xml files</param>
        /// <param name="settingProvider">Setting provider object which is responsible for get setting value from various sources</param>
        public XmlSettingFactory(string applicationName, ISettingFileBuilder xmlPathBuilder, ISettingProvider settingProvider)
            : this(applicationName, xmlPathBuilder, null, settingProvider)
        {
            
        }

        /// <summary>
        /// Instantiate an object for the setting factory
        /// </summary>
        /// <param name="applicationName">Name of application</param>
        /// <param name="xmlPathBuilder">Path builder object which is used to generate (build) the path of xml files</param>
        /// <param name="cacheStrategy">Cache-strategy object to cache setting object</param>
        /// <param name="settingProvider">Setting provider object which is responsible for get setting value from various sources</param>
        public XmlSettingFactory(string applicationName, ISettingFileBuilder xmlPathBuilder, ISettingCacheStrategy cacheStrategy, ISettingProvider settingProvider)
        {
            if (string.IsNullOrEmpty(applicationName))
                throw new SettingException(
                    string.Format(Lang.ExceptionMessage.NullParameter, "applicationName"),
                    new ArgumentNullException("applicationName")
                 );

            ApplicationName = applicationName;

            XmlPathBuilder = xmlPathBuilder ?? throw new SettingException(
                    string.Format(Lang.ExceptionMessage.NullParameter, "xmlPathBuilder"),
                    new ArgumentNullException("xmlPathBuilder")
                 );

            SettingProvider = settingProvider ?? CreateDefaultSettingProvider();
            CacheStrategy = cacheStrategy ?? CreateDefaultCacheStrategy();            
        }

        /// <summary>
        /// Creates default object of <see cref="InMemoryCacheStrategy"/>
        /// </summary>
        /// <returns></returns>
        private ISettingCacheStrategy CreateDefaultCacheStrategy()
        {
            return new InMemoryCacheStrategy();
        }

        // <summary>
        /// Creates default object of <see cref="HierarchicalXmlSettingProvider"/>
        /// </summary>
        /// <returns></returns>
        private ISettingProvider CreateDefaultSettingProvider()
        {
            return new HierarchicalXmlSettingProvider(XmlPathBuilder);
        }

        public AppSetting Create()
        {
            var cacheKey = this.GetCacheKey();
            AppSetting setting = CacheStrategy.Get(cacheKey);
            if(setting == null)
            {
                setting = new AppXmlSetting(ApplicationName);
                // Init setting 
                setting.Initialize(SettingProvider);
                CacheStrategy.Add(setting, cacheKey);
            }

            return setting;
        }

        /// <summary>
        /// Gets cache's key for the setting by using hash of paths of xml files
        /// </summary>
        /// <returns></returns>
        private string GetCacheKey()
        {
            string paths = string.Join("|", new string[] {
                this.XmlPathBuilder.GetSettingFileInfo(SETTING_LEVEL.GENERAL).PhysicalPath??"",
                this.XmlPathBuilder.GetSettingFileInfo(SETTING_LEVEL.COMPANY).PhysicalPath??"",
                this.XmlPathBuilder.GetSettingFileInfo(SETTING_LEVEL.TOOL).PhysicalPath??"",
                this.XmlPathBuilder.GetSettingFileInfo(SETTING_LEVEL.COMPANY_TOOL).PhysicalPath??""
            });

            return Utilities.StringUtils.GetHashString(paths.ToLower());
        }
    }
}
