namespace Euroland.NetCore.ToolsFramework.Setting
{
    public enum SETTING_LEVEL
    {
        /// <summary>
        /// General setting level for all tools
        /// </summary>
        GENERAL,
        /// <summary>
        /// General company setting level. This level is applied to all tools of a company
        /// </summary>
        COMPANY,
        /// <summary>
        /// General setting level for a tool. This level is applied to all companies
        /// </summary>
        TOOL,
        /// <summary>
        /// Setting level for a tool of a company
        /// </summary>
        COMPANY_TOOL
    }
}
