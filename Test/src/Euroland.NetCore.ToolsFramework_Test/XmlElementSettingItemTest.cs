using Euroland.NetCore.ToolsFramework.Setting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class XmlElementSettingItemTest
    {
        private System.Xml.Linq.XDocument loadDoc()
        {
            return  System.Xml.Linq.XDocument.Parse(@"<settings><node>Value</node></settings>");
        }

        [Fact]
        public void Can_instanticate_Setting_item()
        {
            var doc = this.loadDoc();
            var settingItem = new XmlElementSettingItem(doc.Root);

            Assert.NotNull(settingItem);
            Assert.Equal("settings", settingItem.Name);
        }

        [Fact]
        public void Can_get_a_child()
        {
            var doc = this.loadDoc();
            var settingItem = new XmlElementSettingItem(doc.Root);

            var child = settingItem.GetChild("node");

            Assert.NotNull(child);

        }

        [Fact]
        public void Can_get_an_attribute()
        {
            string xml = @"<settings><node nodeAttr=""1"">Value</node></settings>";
            var doc = System.Xml.Linq.XDocument.Parse(xml);

            var settingItem = new XmlElementSettingItem(doc.Root);

            Assert.NotNull((settingItem.GetChild("node") as XmlElementSettingItem).GetAttribute("nodeAttr"));
            Assert.Equal("1", (settingItem.GetChild("node") as XmlElementSettingItem).GetAttribute("nodeAttr").Value);
        }

        [Fact]
        public void Can_get_a_child_with_insensitive_case()
        {
            var doc = this.loadDoc();
            var settingItem = new XmlElementSettingItem(doc.Root);

            var child = settingItem["NODE"];
            Assert.NotNull(child);
        }
    }
}
