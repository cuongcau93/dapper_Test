using System;
using System.Collections.Generic;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Setting.MetaData
{
    /// <summary>
    /// Meta-data class used to detect that a property/field of a class should be bound value automatically from an xml element.
    /// This class is used when a subclass of <see cref="IServiceProvider"/> reads the xml document.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field| AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class Xml2PropBindingAttribute: System.Attribute
    {
        private string name;
        private bool useXmlAttribute = false;
        
        /// <summary>
        /// Instanticate an object of <see cref="Xml2PropBindingAttribute"/>
        /// </summary>
        public Xml2PropBindingAttribute() { }

        /// <summary>
        /// Instanticate an object of <see cref="Xml2PropBindingAttribute"/>
        /// </summary>
        /// <param name="name">Name of xml element. If this value is not specfied, default name of property/field will be used</param>
        public Xml2PropBindingAttribute(string name)
        {

        }

        /// <summary>
        /// Instanticate an object of <see cref="Xml2PropBindingAttribute"/>
        /// </summary>
        /// <param name="useXmlAttribute">Use the value of xml attribute instead of xml element for binding. Name of xml attribute will be the name of property/field.</param>
        public Xml2PropBindingAttribute(bool useXmlAttribute)
        {

        }

        /// <summary>
        /// Instanticate an object of <see cref="Xml2PropBindingAttribute"/>
        /// </summary>
        /// <param name="name">Name of xml element. If this value is not specfied, default name of property/field will be used</param>
        /// <param name="useXmlAttribute">Use the value of xml attribute instead of xml element for binding</param>
        public Xml2PropBindingAttribute(string name, bool useXmlAttribute)
        {

        }

        /// <summary>
        /// Gets or sets the name of xml element or xml attribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the value to indicate that use the value of xml attribute instead of xml element for binding
        /// </summary>
        public bool UseXmlAttribute { get; set; }
    }
}
