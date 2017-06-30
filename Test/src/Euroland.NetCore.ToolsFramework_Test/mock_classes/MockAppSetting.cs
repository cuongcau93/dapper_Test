using Euroland.NetCore.ToolsFramework.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class MockAppSetting : AppSetting
    {
        public MockAppSetting(string applicationName) : base(applicationName)
        {
        }

        protected override void OnInitialized()
        {
            this.Provider.Read(this);
        }
    }
}
