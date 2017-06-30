using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Euroland.NetCore.ToolsFramework.Setting;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class HierarchicalXmlSettingProviderTest
    {
        [Fact]
        public void Can_load_full_4_levels()
        {
            var provider = new HierarchicalXmlSettingProvider(new XmlFilePathBuilder());
            var mockSetting = new MockAppSetting("TestApp");
            mockSetting.Initialize(provider);

            Assert.NotNull(mockSetting.SettingItem);
        }

        [Fact]
        public void Can_load_full_4_levels_with_value()
        {
            var provider = new HierarchicalXmlSettingProvider(new XmlFilePathBuilder());
            var mockSetting = new MockAppSetting("TestApp");
            mockSetting.Initialize(provider);

            Assert.True(mockSetting.SettingItem.HasChildren);
            Assert.Equal("Level4", mockSetting.SettingItem["level"].Value);
        }
    }
}
