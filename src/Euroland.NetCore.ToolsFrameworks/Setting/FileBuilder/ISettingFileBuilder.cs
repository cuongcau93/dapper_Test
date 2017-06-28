using System;
using System.Collections.Generic;
using System.Text;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    public interface ISettingFileBuilder
    {
        /// <summary>
        /// Locates a file with provided level of setting
        /// </summary>
        /// <param name="settingLevel">Level of setting</param>
        /// <returns>The file information. Caller must check Exists property</returns>
        ISettingFileInfo GetSettingFileInfo(SETTING_LEVEL settingLevel);
    }
}
