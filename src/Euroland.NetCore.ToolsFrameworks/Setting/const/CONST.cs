namespace Euroland.NetCore.ToolsFramework.Setting
{
    public struct CONST
    {
        /// <summary>
        /// Name of root element of a xml document
        /// </summary>
        public const string ROOT_ELEMENT_NAME = "Settings";

        /// <summary>
        /// Unique key to distinguish an element with each other which has same name.
        /// When a item has children which have same name, this nique-key value
        /// is to distinguish with each other belong to same parent.
        /// In xml document, unique-key is an attribute of a element
        /// </summary>
        public const string SETTING_UNIQUE_KEY_ATTRIBUTE = "_key";
    }
}
