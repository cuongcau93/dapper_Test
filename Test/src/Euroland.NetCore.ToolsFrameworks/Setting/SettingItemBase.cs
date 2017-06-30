using System;
using System.Collections.Generic;
using System.Reflection;

namespace Euroland.NetCore.ToolsFramework.Setting
{
    /// <summary>
    /// Base class presents for a xml node (xml element) or a setting item
    /// </summary>
    public abstract class SettingItemBase
    {
        public SettingItemBase() { }

        public SettingItemBase(string name) {
            this.Name = name;
        }

        public SettingItemBase(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets the name of current node
        /// </summary>
        public virtual string Name { get; internal set; }

        /// <summary>
        /// Gets the value of current node. 
        /// The value can be empty in case of having other children node 
        /// </summary>
        public virtual string Value { get; internal set; }

        /// <summary>
        /// Gets child collection
        /// </summary>
        public virtual IEnumerable<SettingItemBase> Children { get { return null; } }

        /// <summary>
        /// Compares the value and name of current setting with another setting. 
        /// </summary>
        /// <param name="obj">The instance of type <see cref="SettingItem"/> to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !typeof(SettingItemBase).GetTypeInfo().IsAssignableFrom(obj.GetType().GetTypeInfo()))
                return false;

            SettingItemBase settingToCompare = (SettingItemBase)obj;
            bool isEqual = string.Equals(this.Name, settingToCompare.Name, StringComparison.CurrentCultureIgnoreCase)
                && string.Equals(this.Value, settingToCompare.Value, StringComparison.CurrentCultureIgnoreCase);
            return isEqual;
        }

        /// <summary>
        /// Rerturn the value of setting
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Copies current object to a new object of type <see cref="SettingItemBase"/>
        /// </summary>
        /// <returns></returns>
        public SettingItemBase Clone()
        {
            return (SettingItemBase)this.MemberwiseClone();
        }

        /// <summary>
        /// Adds a setting item or attribute item into the group
        /// </summary>
        /// <param name="child">Item to add</param>
        public virtual void Add(SettingItemBase child){ }

        /// <summary>
        /// Gets a child of current setting item. If has more than one child, the first child will returned
        /// </summary>
        /// <param name="name">The name of setting. It's equivalent to XML element name</param>
        /// <returns>Returns found setting item. Otherwise, return Null</returns>
        public virtual SettingItemBase GetChild(string name) { return null; }

        /// <summary>
        /// Gets a child of current setting item.
        /// </summary>
        /// <param name="name">The name of setting. It's equivalent to XML element name</param>
        /// <param name="info">Object contains other infomation to look up a child</param>
        /// <returns>Returns found setting item. Otherwise, return Null</returns>
        public virtual SettingItemBase GetChild(string name, object info) { return null; }

        /// <summary>
        /// Gets a child of current setting item. If has more than one child, the first child will returned
        /// </summary>
        /// <param name="index">The index of child</param>
        /// <returns>Return found setting item. Otherwise, return Null</returns>
        public virtual SettingItemBase GetChild(int index) { return null; }

        /// <summary>
        /// Checks that whether current setting item contains a child
        /// </summary>
        /// <param name="name">Name of child setting item</param>
        /// <returns></returns>
        public virtual bool ContainsChild(string name) { return false; }

        /// <summary>
        /// Checks that whether current setting item contains a child
        /// </summary>
        /// <param name="name">Name of child setting item</param>
        /// <param name="info">Object contains other infomation to look up a child</param>
        /// <returns></returns>
        public virtual bool ContainsChild(string name, object info) {return false;}

        /// <summary>
        /// Removes a child out of child collection with provided index of child
        /// </summary>
        /// <param name="index">Index of child</param>
        public virtual void Remove(int index) { }

        /// <summary>
        /// Removes a child out of child collection
        /// </summary>
        /// <param name="name">Name of child</param>
        public virtual void Remove(string name) { }

        /// <summary>
        /// Gets a child of the current setting item node. 
        /// In case of a setting item node has more than one child and both children have same name,
        /// the first found child will be returned.
        /// </summary>
        /// <param name="name">Name of child</param>
        /// <returns></returns>
        public virtual SettingItemBase this[string name]
        {
            get
            {
                return this.GetChild(name) ?? new EmptySettingItem();
            }
        }

        /// <summary>
        /// Gets a child of the current setting item node
        /// </summary>
        /// <param name="name">Name of child</param>
        /// <param name="info">Object contains other infomation to look up a child</param>
        /// <returns></returns>
        public virtual SettingItemBase this[string name, object info]
        {
            get
            {
                return this.GetChild(name, info) ?? new EmptySettingItem();
            }
        }

        /// <summary>
        /// Gets value to indicate that whether current setting item has children
        /// </summary>
        public virtual bool HasChildren
        {
            get
            {
                return false;
            }
        }
    }
}
