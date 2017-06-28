using System;
using System.Collections.Generic;
using System.IO;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// Class to read herarchical xml setting with 4 levels of setting. See <see cref="SETTING_LEVEL"/>.
    /// LEVEL_4: Tool-company setting (Contains the setting information for a tool of a certain company).
    /// The priority of overridden setting is: 1. COMPANY_TOOL, 2. TOOL, 3. COMPANY, 4. GENERAL
    /// </summary>
    public sealed class HierarchicalXmlSettingProvider: XmlSettingProvider
    {
        private List<string> fileSettingPaths;
        public HierarchicalXmlSettingProvider(ISettingFileBuilder fileNameBuilder)
        {
            if(fileNameBuilder == null)
                throw new SettingException(
                    string.Format(Lang.ExceptionMessage.NullParameter, "fileNameBuilder"), new
                    ArgumentNullException("fileNameBuilder")
                );

            this.FileNameBuilder = fileNameBuilder;
            // default paths
            fileSettingPaths = new List<string>();
        }

        /// <summary>
        /// Gets the builder of xml setting's file name
        /// </summary>
        public ISettingFileBuilder FileNameBuilder { get; private set; }

        /// <summary>
        /// Gets array of path of heirarchical xml settings
        /// </summary>
        public string[] FileSettingPaths => this.fileSettingPaths.ToArray();

        public override bool Read(AppSetting setting)
        {
            return this.ReadHierarchicalSetting(setting);
        }

        public override bool Read(AppSetting setting, string xml)
        {
            // Do nothing here
            return false;
        }

        private void InternalFillXmlToObject(AppSetting setting, string xmlFileSystemPath)
        {
            try
            {
                System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(xmlFileSystemPath);
                this.FillToObject(setting, document);
            }
            catch (Exception ex)
            {
                throw new SettingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads 3 levels of setting first, LEVEL_2 will override LEVEL_1, LEVEL_3 will override LEVEL_2
        /// </summary>
        /// <param name="setting">Setting object to be filled values from xml</param>
        /// <returns></returns>
        private bool ReadHierarchicalSetting(AppSetting setting)
        {
            // TODO: 
            // The setting at LEVEL_1, LEVEL_2, LEVEL_3 not always be changed but has unchanged values
            // Should we re-read these levels again and again when has a new Setting is created?
            // Posible solutions:
            //      1. Memory cache by using Static class holding Setting objects of 3 levels
            //      2. Injectable cache by using external cache interface
            //
            // The level "TOOL" always contains entire setting of a tool (application).
            // The work-flow is: 
            //  #step1 Read xml of "TOOL" level => Read level "COMPANY_TOOL" to overwrite level "TOOL"
            //  #step2 Read xml of "GENERAL" level => Read level "COMPANY" to overwrite level "GENERAL"
            //  #step3 Add (fill) those setting items that #step2 does have but #step1 dose not have
            var toolXmlFileInfo =           this.FileNameBuilder.GetSettingFileInfo(SETTING_LEVEL.TOOL);
            var company_toolXmlFileInfo =   this.FileNameBuilder.GetSettingFileInfo(SETTING_LEVEL.COMPANY_TOOL);
            var companyXmlFileInfo =        this.FileNameBuilder.GetSettingFileInfo(SETTING_LEVEL.COMPANY);
            var generalXmlFileInfo =        this.FileNameBuilder.GetSettingFileInfo(SETTING_LEVEL.GENERAL);

            // TODO: xml path could not just only be system file, but network file also

            if (generalXmlFileInfo.Exists)
            {
                this.fileSettingPaths.Add(generalXmlFileInfo.PhysicalPath);

                using (System.IO.Stream str = generalXmlFileInfo.CreateReadStream())
                {
                    var doc = this.LoadXDocument(generalXmlFileInfo.CreateReadStream(), false);
                    if (doc != null)
                        this.FillToObject(setting, doc);
                }
            }

            if (companyXmlFileInfo.Exists)
            {
                this.fileSettingPaths.Add(companyXmlFileInfo.PhysicalPath);

                using (System.IO.Stream str = companyXmlFileInfo.CreateReadStream())
                {
                    var doc = this.LoadXDocument(str, false);
                    if (doc != null)
                        this.FillToObject(setting, doc);
                }
            }

            if (toolXmlFileInfo.Exists)
            {
                this.fileSettingPaths.Add(toolXmlFileInfo.PhysicalPath);

                using (System.IO.Stream str = toolXmlFileInfo.CreateReadStream())
                {

                    this.FillToObject(setting, this.LoadXDocument(str));
                }
            }
            else
                throw new Exceptions.SettingNotFoundException(
                    new FileNotFoundException(Lang.ExceptionMessage.SettingFileNotFound, toolXmlFileInfo.PhysicalPath)
                );

            if (company_toolXmlFileInfo.Exists)
            {
                this.fileSettingPaths.Add(company_toolXmlFileInfo.PhysicalPath);

                using (System.IO.Stream str = company_toolXmlFileInfo.CreateReadStream())
                {
                    this.FillToObject(setting, this.LoadXDocument(str));
                }
            }
            else
                throw new Exceptions.SettingNotFoundException(
                    new FileNotFoundException(Lang.ExceptionMessage.SettingFileNotFound, company_toolXmlFileInfo.PhysicalPath)
                );

            return true;
        }

        private System.Xml.Linq.XDocument LoadXDocument(Stream xmlFileStream, bool thrownException = true)
        {
            try
            {
                return System.Xml.Linq.XDocument.Load(xmlFileStream);
            }
            catch (Exception ex)
            {
                if (thrownException)
                    throw new SettingException(ex.Message, ex);
                else
                    return null;
            }
        }

        private void ReadRecursive(SettingItemGroup settingItem, System.Xml.Linq.XElement element)
        {
            if (element.HasAttributes)
            {
                foreach (var attr in element.Attributes())
                {
                    settingItem.Add(new SettingAttributeItem(attr.Name.LocalName, attr.Value));
                }
            }

            if (element.HasElements)
            {
                foreach (var elm in element.Elements())
                {
                    var item = new SettingItemGroup(elm.Name.LocalName, elm.HasElements ? string.Empty : elm.Value);
                    this.ReadRecursive(item, elm);
                    settingItem.Add(item);
                }
            }
        }
    }
}
