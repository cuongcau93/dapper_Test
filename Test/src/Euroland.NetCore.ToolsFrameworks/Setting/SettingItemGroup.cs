using System;
using System.Collections.Generic;
using System.Linq;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// Class presents for a xml node, which contains node children and attributes
    /// </summary>
    public class SettingItemGroup : SettingItemBase
    {
        /// <summary>
        /// Children of current setting item
        /// </summary>
        private List<SettingItemBase> children;

        /// <summary>
        /// List of attribute of current setting item
        /// </summary>
        private List<SettingItemBase> attributes;

        public SettingItemGroup(string name)
            : this(name, string.Empty)
        {

        }

        public SettingItemGroup(string name, string value)
            : base(name, value)
        {
            this.children = new List<SettingItemBase>();
            this.attributes = new List<SettingItemBase>();
        }

        /// <summary>
        /// Gets a child base on its name and attribute's value
        /// </summary>
        /// <param name="name">Name of child</param>
        /// <param name="attribute">Value of attribute</param>
        /// <returns></returns>
        private SettingItemBase GetChild(string name, string attribute)
        {
            return this.children.Where(stt =>
                stt is SettingItemGroup
                && string.Equals(name, stt.Name, StringComparison.CurrentCultureIgnoreCase)
                && ((SettingItemGroup)stt).HasAttributes
                && ((SettingItemGroup)stt).attributes.Any(attr => string.Equals(attr.Value, attribute, StringComparison.CurrentCultureIgnoreCase))
            ).FirstOrDefault();
        }

        #region public methods

        public override SettingItemBase GetChild(string name, object info)
        {
            if(info is string)
            {
                return this.GetChild(name, (string)info);
            }

            return null;
        }

        /// <summary>
        /// Gets a child of current setting item. If has more than one child, the first child will returned
        /// </summary>
        /// <param name="index">The index of child</param>
        /// <returns>Return found setting item. Otherwise, return Null</returns>
        public override SettingItemBase GetChild(int index)
        {
            if (index < this.children.Count)
                return this.children[index];

            return null;
        }

        /// <summary>
        /// Gets a child of current setting item. If has more than one child, the first child will returned
        /// </summary>
        /// <param name="name">The name of setting. It's equivalent to XML element name</param>
        /// <returns>Returns found setting item. Otherwise, return Null</returns>
        public override SettingItemBase GetChild(string name)
        {
            return this.children.Where(stt => string.Equals(stt.Name, name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        /// <summary>
        /// Checks that whether current setting item contains a child
        /// </summary>
        /// <param name="childName">Name of child setting item</param>
        /// <returns></returns>
        public override bool ContainsChild(string childName)
        {
            return this.children.Any(child => string.Equals(child.Name, childName, StringComparison.CurrentCultureIgnoreCase));
        }

        public override bool ContainsChild(string name, object info)
        {
            return this.GetChild(name, info) != null;
        }

        /// <summary>
        /// Adds a setting item or attribute item into the group
        /// </summary>
        /// <param name="child">Item to add</param>
        public override void Add(SettingItemBase child)
        {
            if (child == null)
                return;

            if (child is SettingAttributeItem)
            {
                var attr = this.GetAttribute(child.Name);
                if (attr == null)
                    this.attributes.Add(child);
                else
                    attr.Value = child.Value;
            }
            else if(child is SettingItemGroup)
            {
                SettingItemBase uniqueKeyAttr = null;
                IEnumerable<SettingItemBase> attributes = null;
               
                SettingItemGroup itemGroup = child as SettingItemGroup;
                uniqueKeyAttr = itemGroup.GetAttribute(CONST.SETTING_UNIQUE_KEY_ATTRIBUTE);
                attributes = itemGroup.Attributes;
               

                SettingItemBase foundChild = uniqueKeyAttr != null 
                    ? this.GetChild(child.Name, uniqueKeyAttr.Value)
                    : this.GetChild(child.Name);

                if (foundChild == null)
                    this.children.Add(child);
                else
                {
                    foundChild.Value = child.Value;
                    if(attributes != null)
                    {
                        foreach (var attr in attributes)
                        {
                            foundChild.Add(attr);
                        }
                    }
                }
            }
        }

        public override void Remove(int index)
        {
            if (index < this.children.Count)
                this.children.RemoveAt(index);
        }

        public override void Remove(string name)
        {
            var settingItem = this.GetChild(name);
            if (settingItem != null)
                this.children.Remove(settingItem);
        }

        public void RemoveAttribute(string attributeName)
        {
            var attrItem = this.GetAttribute(attributeName);
            if (attrItem != null)
                this.attributes.Remove(attrItem);
        }

        /// <summary>
        /// Gets attribute item  of current setting item
        /// </summary>
        /// <param name="attributeName">Name of attribute</param>
        /// <returns>Returns found setting item. Returns Null if not found</returns>
        public SettingItemBase GetAttribute(string attributeName)
        {
            return this.attributes.Where(
                attr => string.Equals(attr.Name, attributeName, StringComparison.CurrentCultureIgnoreCase)
            ).FirstOrDefault();
        }
        
        /// <summary>
        /// Checks that whether current setting item contains a child
        /// </summary>
        /// <param name="childName">Name of child setting item</param>
        /// <param name="uniqueKey">Unique key value of child setting item.</param>
        /// <returns></returns>
        //public bool ContainsChild(string childName, string uniqueKey)
        //{
        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Checks that whether current setting item contains an attribute
        /// </summary>
        /// <param name="attributeName">Name of attribute</param>
        /// <returns></returns>
        public bool ContainsAttribute(string attributeName)
        {
            return this.attributes.Any(attr => string.Equals(attr.Name, attributeName, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IEnumerable<SettingItemBase> Children => this.children.AsEnumerable();

        public IEnumerable<SettingItemBase> Attributes => this.attributes.AsEnumerable();

        public override bool HasChildren => this.children.Count > 0;

        public bool HasAttributes => this.attributes.Count > 0;

        #endregion public methods
    }
}
