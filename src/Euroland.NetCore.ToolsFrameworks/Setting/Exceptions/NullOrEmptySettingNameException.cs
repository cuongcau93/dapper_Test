using System;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    public class NullOrEmptySettingNameException: SettingException
    {
        public NullOrEmptySettingNameException(string paramName)
            : base(Lang.ExceptionMessage.SettingNameShouldNotBeNull, new ArgumentNullException(paramName))
        {

        }
    }
}
