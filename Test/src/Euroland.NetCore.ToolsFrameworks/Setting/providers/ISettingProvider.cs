namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// The setting provider interface that provides read and write the app configuration 
    /// </summary>
    public interface ISettingProvider
    {
        /// <summary>
        /// Reads setting information into provided setting object instance
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        bool Read(AppSetting setting);

        /// <summary>
        /// Reads setting information from an XML string into provided setting object instance
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        bool Read(AppSetting setting, string xml);
        
        /// <summary>
        /// Writes setting for a provided setting object and returns as a string
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        string WriteAsString(AppSetting setting);

        /// <summary>
        /// Writes setting for a provided setting object into a Stream
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="outputStream"></param>
        void WriteAsString(AppSetting setting, System.IO.Stream outputStream);
    }
}
