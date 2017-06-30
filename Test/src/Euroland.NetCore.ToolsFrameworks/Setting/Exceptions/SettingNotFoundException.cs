using System;

namespace Euroland.NetCore.ToolsFramework.Setting.Exceptions
{
    public class SettingNotFoundException: SettingException
    {
        public SettingNotFoundException(Exception innerException)
            : base(Lang.ExceptionMessage.SettingFileNotFound, innerException) { }
    }
}
