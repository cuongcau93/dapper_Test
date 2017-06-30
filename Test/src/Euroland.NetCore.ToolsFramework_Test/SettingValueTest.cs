using Euroland.NetCore.ToolsFramework.Setting;
using System;
using Xunit;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class SettingValueTest: System.Dynamic.DynamicObject
    {
        public int a = 0;
    }

    class B
    {
        public B()
        {
            dynamic test = new SettingValueTest();
            test["jhehe"] = 1;
        }
    }
}
