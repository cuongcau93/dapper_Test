using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Euroland.NetCore.ToolsFramework.Setting;
using System.Linq;

namespace Euroland.NetCore.ToolsFramework.Test
{
    public class SettingItemGroupTest
    {
        const string SettingName = "TestGroup";
        [Fact]
        public void Can_instantiate_setting()
        {
            var settingItem = new SettingItemGroup(SettingName);
            Assert.Equal(SettingName, settingItem.Name);
        }

        [Fact]
        public void Can_add_children()
        {
            var settingItem = new SettingItemGroup(SettingName);
            var child = new SettingItemGroup("ChildItem", "ChildItemValue");
            settingItem.Add(child);

            var testChild = settingItem.GetChild("ChildItem");

            Assert.NotEmpty(settingItem.Children);
            Assert.Equal<SettingItemBase>(child, testChild);
        }
        
        [Fact]
        public void Can_add_and_find_attribute()
        {
            var settingItem = new SettingItemGroup(SettingName);
            var attr = new SettingAttributeItem("Attr1", "AttributeValue1");

            settingItem.Add(attr);

            var foundAttr = settingItem.GetAttribute("Attr1");

            Assert.NotNull(foundAttr);
            Assert.Equal("AttributeValue1", foundAttr.Value);
        }

        [Fact]
        public void Setting_name_is_case_insensitive()
        {
            var settingItem = new SettingItemGroup(SettingName);
            var child = new SettingItemGroup("ChildItem", "ChildItemValue");
            var attr  = new SettingAttributeItem("Attr1", "AttributeValue1");

            settingItem.Add(child);
            settingItem.Add(attr);

            var foundChild = settingItem.GetChild("childitem");
            var foundAttr = settingItem.GetAttribute("attr1");

            Assert.NotNull(foundChild);
            Assert.NotNull(foundAttr);
        }
        
        [Fact]
        public void Can_overwrite_value_of_existing_child()
        {
            var settingItem = new SettingItemGroup(SettingName);
            var child = new SettingItemGroup("ChildItem", "ChildItemValue");
            settingItem.Add(child);

            var overwriteChildItem = new SettingItemGroup("ChildItem", "Overwrited");
            settingItem.Add(overwriteChildItem);

            var testChild = settingItem.GetChild("ChildItem");



            Assert.Equal<SettingItemBase>(overwriteChildItem, testChild);
        }

        [Fact]
        public void Can_overwrite_attribute_value_of_existing_child()
        {
            var settingItem = new SettingItemGroup(SettingName);
            var attr = new SettingAttributeItem("Attr1", "AttributeValue1");

            settingItem.Add(attr);

            var overwriteAttr2 = new SettingAttributeItem("Attr1", "OverwriteAttr");

            settingItem.Add(overwriteAttr2);

            var foundAttr = settingItem.GetAttribute("Attr1");

            Assert.Equal<SettingItemBase>(overwriteAttr2, foundAttr);
        }

        [Fact]
        public void Can_merge_attributes_of_two_item_has_same_name()
        {
            var settingItem = new SettingItemGroup(SettingName);
            var child = new SettingItemGroup("ChildItem", "ChildItemValue");
            child.Add(new SettingAttributeItem("myAttribute1", "1"));
            settingItem.Add(child);

            var child2 = new SettingItemGroup("ChildItem", "ChildItemValue1");
            child2.Add(new SettingAttributeItem("myAttribute2", "2"));

            settingItem.Add(child2);

            Assert.Equal(1, settingItem.Children.Count());

            var foundChild = settingItem["ChildItem"] as SettingItemGroup;
            Assert.True(foundChild.HasAttributes);
            Assert.Equal(2, foundChild.Attributes.Count());
        }

        [Fact]
        public void Can_add_multiple_item_with_same_name_but_has_different_unique_key_attribute()
        {
            var settingItem = new SettingItemGroup(SettingName);
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                var child = new SettingItemGroup("node", "Value-" + i.ToString());
                child.Add(new SettingAttributeItem(Euroland.NetCore.ToolsFramework.Setting.CONST.SETTING_UNIQUE_KEY_ATTRIBUTE, i.ToString()));
                settingItem.Add(child);
            }

            Assert.Equal(count, settingItem.Children.Count());
        }
    }
}
