using System;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// Class contains the setting values of an application
    /// </summary>
    public abstract class AppSetting
    {
        /// <summary>
        /// Gets setting provider object
        /// </summary>
        [MetaData.Xml2PropIgnore]
        public ISettingProvider Provider { get; private set; }

        /// <summary>
        /// Gets the initialization state of setting
        /// </summary>
        [MetaData.Xml2PropIgnore]
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets application name
        /// </summary>
        [MetaData.Xml2PropIgnore]
        public string ApplicationName { get; private set; }

        /// <summary>
        /// Gets or sets the root setting item
        /// </summary>
        [MetaData.Xml2PropIgnore]
        public SettingItemBase SettingItem { get; set; }

        public AppSetting(string applicationName) {
            this.ApplicationName = applicationName;
        }

        /// <summary>
        /// Initializes setting with provided a provider
        /// </summary>
        /// <param name="provider">Setting provider</param>
        /// <exception cref="SettingException"></exception>
        public void Initialize(ISettingProvider provider)
        {
            if (this.IsInitialized)
                return;

            this.Provider = provider ?? throw new SettingException(
                    string.Format(Lang.ExceptionMessage.NullParameter, "provider"), new 
                    ArgumentNullException("provider")
                );

            this.OnInitialized();
            this.IsInitialized = true;
        }

        protected abstract void OnInitialized();
    }
}
