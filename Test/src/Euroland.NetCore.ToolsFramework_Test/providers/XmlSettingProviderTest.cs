using Euroland.NetCore.ToolsFramework.Setting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Linq;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class XmlSettingProviderTest
    {
        ISettingProvider GetProvider()
        {
            return new XmlSettingProvider("test.xml");
        }

        [Fact]
        public void Can_instantiate()
        {
            XmlSettingProvider xmlProvider = new XmlSettingProvider("http://113.190.248.146/tools/subscriptioncentre2/config/company/s-volv.xml");
            
            Assert.NotNull(xmlProvider);
        }

        [Fact]
        public void Can_read_xml_setting_and_fill_into_object()
        {
            var mockAppSetting = new Mock<AppSetting>("TestApplication");
            var appSetting = mockAppSetting.Object;
            Assert.NotNull(appSetting);

            var provider = GetProvider();
            bool result = provider.Read(appSetting);

            Assert.Equal(true, result);
        }

        [Fact]
        public void Can_initialize_app_setting_with_xml_setting_provider()
        {
            var appSetting = new MockAppSetting("TestApplication");
            var provider = GetProvider();

            Assert.Equal(false, appSetting.IsInitialized);

            appSetting.Initialize(provider);

            Assert.Equal(true, appSetting.IsInitialized);

            Assert.NotNull(appSetting.SettingItem);
        }

        [Fact]
        public void Can_read_from_xml_string()
        {
            string xml = @"<settings><child>child value</child></settings>";
            var appSetting = new MockAppSetting("TestApplication");
            var provider = new XmlSettingProvider();
            provider.Read(appSetting, xml);

            Assert.NotNull(appSetting.SettingItem);
        }

        [Fact]
        public void Can_overwrite_attribute_value_of_2_elements_have_same_name_but_not_unique_key()
        {
            string xml = @"<settings><child>child value1</child><child>child value2</child></settings>";
            var appSetting = new MockAppSetting("TestApplication");
            var provider = new XmlSettingProvider();
            provider.Read(appSetting, xml);

            Assert.Equal(1, appSetting.SettingItem.Children.Count());
            Assert.Equal("child value2", appSetting.SettingItem["child"].Value);
        }

        [Fact]
        public void Can_read_multiple_item_with_same_name_but_has_different_unique_key_attribute()
        {
            string xml = @"<settings><child _key=""key1"">child value1</child><child _key=""key2"">child value2</child></settings>";
            var appSetting = new MockAppSetting("TestApplication");
            var provider = new XmlSettingProvider();
            provider.Read(appSetting, xml);

            Assert.NotEmpty(appSetting.SettingItem.Children);
            Assert.Equal(2, appSetting.SettingItem.Children.Count());
        }

        [Fact]
        public void Can_merge_attributes_of_two_item_has_same_name()
        {
            string xml = @"<settings><child format=""MMddyyyy"">child value1</child><child culture=""en-GB"">child value2</child></settings>";
            var appSetting = new MockAppSetting("TestApplication");
            var provider = new XmlSettingProvider();
            provider.Read(appSetting, xml);

            Assert.Equal(1, appSetting.SettingItem.Children.Count());
            Assert.Equal("child value2", appSetting.SettingItem["child"].Value);

            var itemGroup = appSetting.SettingItem["child"] as SettingItemGroup;

            Assert.NotNull(itemGroup);
            Assert.True(itemGroup.HasAttributes);
            Assert.Equal(2, itemGroup.Attributes.Count());
            Assert.NotNull(itemGroup.GetAttribute("format"));
            Assert.NotNull(itemGroup.GetAttribute("culture"));
            Assert.Equal("MMddyyyy", itemGroup.GetAttribute("format").Value);
            Assert.Equal("en-GB", itemGroup.GetAttribute("culture").Value);
        }
    }
}
