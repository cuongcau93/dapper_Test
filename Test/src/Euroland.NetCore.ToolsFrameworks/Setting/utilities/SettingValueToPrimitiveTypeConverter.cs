using System;
using System.Reflection;

namespace Euroland.NetCore.ToolsFramework.Setting.Utilities
{
    public static class SettingValueToPrimitiveTypeConverter
    {
        /// <summary>
        /// Tries to convert setting item's value to a primitive type (Int, Boolean, Float, etc.)
        /// </summary>
        /// <typeparam name="T">Primitive type</typeparam>
        /// <param name="settingItem">Setting item</param>
        /// <returns></returns>
        public static T ToPrimitive<T>(this SettingItemBase settingItem)
        {
            try
            {
                string value = settingItem.Value.Trim();

                TypeInfo typeInfo = typeof(T).GetTypeInfo();

                if (typeInfo.IsEquivalentTo(typeof(Boolean)) || typeInfo.IsEquivalentTo(typeof(Boolean?)))
                {
                    if (string.IsNullOrEmpty(value))
                        return (T)Convert.ChangeType("False", typeof(T));
                    // For compability with older config, which is using 0 or 1 for boolean type
                    else if (value == "0")
                        return (T)Convert.ChangeType("False", typeof(T));
                    else if (value == "1")
                        return (T)Convert.ChangeType("True", typeof(T));
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                throw new SettingException(Lang.ExceptionMessage.InvalidCastingToPrimitiveType, ex);
            }
        }

        /// <summary>
        /// Tries to convert setting item's value to a primitive type (Int, Boolean, Float, etc.)
        /// </summary>
        /// <typeparam name="T">Primitive type</typeparam>
        /// <param name="settingItem">Setting item</param>
        /// <param name="defaultValue">Default value returned if the conversion operation fails</param>
        /// <returns></returns>
        public static T ToPrimitive<T>(this SettingItemBase settingItem, T defaultValue)
        {
            try
            {
                return settingItem.ToPrimitive<T>();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
