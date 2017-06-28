namespace Euroland.NetCore.ToolsFramework.Setting
{
    public partial class AppXmlSetting: AppSetting
    {
        public AppXmlSetting(string applicationName)
            : base(applicationName)
        {

        }

        protected override void OnInitialized()
        {
            Provider.Read(this);
        }
    }
}
