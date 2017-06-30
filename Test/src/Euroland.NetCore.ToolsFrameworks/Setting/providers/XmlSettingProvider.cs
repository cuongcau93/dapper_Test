using System;
using System.Text;
using System.IO;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// The class used to read the setting information stored in XML document
    /// </summary>
    public class XmlSettingProvider: ISettingProvider
    {
        private System.Xml.Linq.LoadOptions XmlLoadOptions = System.Xml.Linq.LoadOptions.PreserveWhitespace;
        /// <summary>
        /// Gets the path of xml file.
        /// </summary>
        public string XmlUri { get; protected set; }

        /// <summary>
        /// Instantiate an object of <see cref="XmlSettingProvider"/>
        /// </summary>
        public XmlSettingProvider(){ }

        /// <summary>
        /// Instantiate an object of <see cref="XmlSettingProvider"/>
        /// </summary>
        /// <param name="xmlUri">
        ///     Path of xml file.
        ///     The path must be a file system relative or absolute path
        /// </param>
        public XmlSettingProvider(string xmlUri)
        {
            if(string.IsNullOrEmpty(xmlUri))
                throw new SettingException(string.Format(Lang.ExceptionMessage.NullParameter, "xmlUri"), new ArgumentNullException("xmlUri"));

            this.XmlUri = xmlUri;
        }

        /// <summary>
        /// Reads and fills setting information into an object of type <see cref="AppSetting"/>
        /// </summary>
        /// <param name="setting">Object to be filled in setting information</param>
        /// <returns>The result of operation</returns>
        public virtual bool Read(AppSetting setting)
        {
            try
            {
                System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(this.XmlUri);
                return this.FillToObject(setting, document);
            }
            catch (Exception ex)
            {
                throw new SettingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads and fills setting information from input XML into an object of type <see cref="AppSetting"/>
        /// </summary>
        /// <param name="setting">Object to be filled in setting information</param>
        /// <param name="xml">The XML string contains setting information</param>
        /// <returns>The result of operation</returns>
        public virtual bool Read(AppSetting setting, string xml)
        {
            try
            {
                System.IO.StringReader stringReader = new System.IO.StringReader(xml);
                System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(stringReader, this.XmlLoadOptions);
                return this.FillToObject(setting, document);
            }
            catch(Exception ex)
            {
                throw new SettingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reads and fills setting information into setting object
        /// </summary>
        /// <param name="setting">Object to be filled setting information</param>
        /// <param name="document">Document to read setting informatino</param>
        /// <returns></returns>
        protected virtual bool FillToObject(AppSetting setting, System.Xml.Linq.XDocument document)
        {
            if(setting.SettingItem == null)
                setting.SettingItem = new SettingItemGroup(document.Root.Name.LocalName);

            foreach (var elm in document.Root.Elements())
            {
                var item = new SettingItemGroup(elm.Name.LocalName, elm.HasElements ? string.Empty : elm.Value);
                this.ReadRecursively(item, elm);

                setting.SettingItem.Add(item);
            }
            return true;
        }

        public virtual string WriteAsString(AppSetting setting)
        {
            return this.ConvertSettingObjectToXDocument(setting).ToString();
        }

        public virtual void WriteAsString(AppSetting setting, Stream outputStream)
        {
            using (System.IO.StreamWriter writer = new StreamWriter(outputStream, System.Text.Encoding.UTF8))
            {
                writer.Write(this.ConvertSettingObjectToXDocument(setting).ToString());
            }
        }

        private void ReadRecursively(SettingItemGroup settingItem, System.Xml.Linq.XElement element)
        {
            if (element.HasAttributes)
            {
                foreach (var attr in element.Attributes())
                {
                    settingItem.Add(new SettingAttributeItem(attr.Name.LocalName, attr.Value));
                }
            }

            if(element.HasElements)
            {
                foreach (var elm in element.Elements())
                {
                    var item = new SettingItemGroup(elm.Name.LocalName, elm.HasElements ? string.Empty : elm.Value);
                    this.ReadRecursively(item, elm);
                    settingItem.Add(item);
                }
            }
        }

        /// <summary>
        /// Converts the setting object to <see cref="System.Xml.Linq.XDocument"/>
        /// </summary>
        /// <param name="setting">Setting object to convert</param>
        /// <returns></returns>
        protected virtual System.Xml.Linq.XDocument ConvertSettingObjectToXDocument(AppSetting setting)
        {
            if (setting.SettingItem == null)
                return System.Xml.Linq.XDocument.Parse(@"<settings>Empty setting object</settings>");

            StringBuilder xmlBuilder = new StringBuilder();
            var xDoc = new System.Xml.Linq.XDocument();
            var rootElement = new System.Xml.Linq.XElement(setting.SettingItem.Name);
            xDoc.Add(rootElement);
            this.WriteSettingObjectRecursively(rootElement, setting.SettingItem);

            return xDoc;
        }

        private void WriteSettingObjectRecursively(System.Xml.Linq.XElement element, SettingItemBase settingItem)
        {
            if(settingItem is SettingItemGroup)
            {
                var settingItemGroup = settingItem as SettingItemGroup;
                if (settingItemGroup.HasAttributes)
                {
                    foreach (var attr in settingItemGroup.Attributes)
                    {
                        element.Add(new System.Xml.Linq.XAttribute(attr.Name, attr.Value));
                    }
                }
            }

            if (settingItem.HasChildren)
            {
                bool hasChildren = false;
                foreach (var child in settingItem.Children)
                {
                    hasChildren = child.HasChildren;
                    var xElement = hasChildren
                        ? new System.Xml.Linq.XElement(child.Name)
                        : new System.Xml.Linq.XElement(child.Name, child.Value);

                    element.Add(xElement);

                    this.WriteSettingObjectRecursively(xElement, child);
                }
            }
        }
    }
}
