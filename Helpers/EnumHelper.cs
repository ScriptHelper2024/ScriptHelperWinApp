using System;
using System.ComponentModel;
using System.Reflection;

namespace Helpers
{
    public static class EnumHelper
    {
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static T GetEnumValueFromDescription<T>(this string description)
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                string desc = item.DescriptionAttr();
                if (desc.HasValue())
                {
                    if (desc == description)
                        return (T)item;
                }
            }

            return default(T);
        }
    }
}
