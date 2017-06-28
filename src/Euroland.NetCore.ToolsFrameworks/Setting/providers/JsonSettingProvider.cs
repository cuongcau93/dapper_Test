using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    public class JsonSettingProvider : ISettingProvider
    {
        public bool Read(AppSetting setting)
        {
            throw new NotImplementedException();
        }

        public bool Read(AppSetting setting, string xml)
        {
            throw new NotImplementedException();
        }

        public string WriteAsString(AppSetting setting)
        {
            throw new NotImplementedException();
        }

        public void WriteAsString(AppSetting setting, Stream outputStream)
        {
            throw new NotImplementedException();
        }
    }
}
