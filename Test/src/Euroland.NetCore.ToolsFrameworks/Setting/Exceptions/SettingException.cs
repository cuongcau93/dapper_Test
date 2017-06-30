using System;

namespace Euroland.NetCore.ToolsFramework
{
    public class SettingException: Exception
    {
        public SettingException()
            : base()
        {
            
        }

        public SettingException(string message)
            : base(message)
        {
            
        }

        public SettingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
