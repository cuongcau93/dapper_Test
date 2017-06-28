using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class XmlSettingFactoryTest
    {
        [Fact]
        public void Can_create_default_app_setting()
        {
            var factory = new Setting.XmlSettingFactory("ApplicationTest", new XmlFilePathBuilder());

            Assert.NotNull(factory);

            var appSetting = factory.Create();
            Assert.Equal("ApplicationTest", appSetting.ApplicationName);
            Assert.NotNull(appSetting.SettingItem);
            Assert.NotEmpty(appSetting.SettingItem.Children);
        }
    }
}
