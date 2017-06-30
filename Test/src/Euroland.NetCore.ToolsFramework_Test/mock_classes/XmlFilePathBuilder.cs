using Euroland.NetCore.ToolsFramework.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class XmlFilePathBuilder : ISettingFileBuilder
    {
        string rootDir;
        public XmlFilePathBuilder()
        {
            this.rootDir = System.IO.Directory.GetCurrentDirectory();
        }

        public ISettingFileInfo GetSettingFileInfo(SETTING_LEVEL settingLevel)
        {
            string fileName = "";
            switch (settingLevel)
            {
                case SETTING_LEVEL.GENERAL:
                    fileName = System.IO.Path.Combine(this.rootDir, "tools", "config", "generalSetting.xml");
                    break;
                case SETTING_LEVEL.COMPANY:
                    fileName = System.IO.Path.Combine(this.rootDir, "tools", "config", "company", "companyTest.xml");
                    break;
                case SETTING_LEVEL.TOOL:
                    fileName = System.IO.Path.Combine(this.rootDir, "tools", "ToolTest", "config", "ToolTest.xml");
                    break;
                case SETTING_LEVEL.COMPANY_TOOL:
                    fileName = System.IO.Path.Combine(this.rootDir, "tools", "ToolTest", "config", "company", "companyTest.xml");
                    break;
            }

            if (!String.IsNullOrWhiteSpace(fileName))
                return new PhysicalSettingFileInfo(new System.IO.FileInfo(fileName));

            return new NotFoundFileInfo("Unknown");
        }
    }

}
