using System;
using System.Collections.Generic;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Setting.MetaData
{
    /// <summary>
    /// Class used to indicate an property/field of a class should be aware from <see cref="ISettingProvider"/> for binding
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class Xml2PropIgnoreAttribute: System.Attribute
    {
        public Xml2PropIgnoreAttribute() { }
    }
}
