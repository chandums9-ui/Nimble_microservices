using Common.Domain.DTO.App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Domain.DTO.Enums
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            throw new ArgumentException("Item not found.", nameof(enumValue));
        }
        public static string GetDisplayName(this Enum enumValue, bool getShortName = false)
        {
            try
            {
                if (getShortName)
                {
                    return enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetShortName()
                                   ??
                                   enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName();
                }
                else
                {
                    string enumVal= enumValue.GetType().GetMember(enumValue.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName();
                    return enumVal;
                }
            }
            catch { return string.Empty; }
        }
        //public static int GetOrder(this Enum enumValue)
        //{
        //    try
        //    {
        //        //return OrderHelper.GetOrder(enumValue);

        //        return enumValue.GetType()
        //                        .GetMember(enumValue.ToString())
        //                        .First()
        //                        .GetCustomAttribute<OrderAttribute>()
        //                        .GetPriority<Enum>();

        //    }
        //    catch { return 0; }
        //}
        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            return Enum.TryParse(str, true, out T val) ? val : default;
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            try { return (T)Enum.ToObject(enumType, intValue); } catch { return default; };
        }

        public static List<KeyValuePairObject<int, string>> GetEnumList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(value => new KeyValuePairObject<int, string>((int)(object)value, value.ToString()))
                       .ToList();
        }

        public static List<KeyValuePairObject<int, string>> GetEnumListWithDisplayName<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(value => new KeyValuePairObject<int, string>((int)(object)value, value.GetDisplayName().ToString()))
                       .ToList();
        }

        public static List<KeyValuePairObject<short, string>> GetEnumListWithDisplayNameShort<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                       .Cast<TEnum>()
                       .Select(value => new KeyValuePairObject<short, string>((short)Convert.ChangeType(value, typeof(short)), value.GetDisplayName().ToString()))
                       .ToList();
        }

        public static List<KeyValuePairObject<int, string>> GetEnumListWithDisplayNameByEnumOrder1<TEnum>() where TEnum : Enum
        {
            return typeof(TEnum)
                .GetFields(BindingFlags.Public | BindingFlags.Static) // Get enum fields in declaration order
                .Select(field => new
                {
                    Value = (TEnum)field.GetValue(null),
                    Name = field.Name
                })
                .Select(x => new KeyValuePairObject<int, string>((int)(object)x.Value, x.Value.GetDisplayName()))
                .ToList();
        }



        public static List<KeyValuePairObject<int, string, int>> GetEnumListWithDisplayNamePriority<TEnum>() where TEnum : Enum
        {
            List<KeyValuePairObject<int, string, int>> priorityInfo = new List<KeyValuePairObject<int, string, int>>();
            MemberInfo[] members = typeof(TEnum).GetMembers();
            foreach (MemberInfo member in members)
            {
                object[] attrs = member.GetCustomAttributes(typeof(OrderAttribute), false);
                foreach (object attr in attrs)
                {
                    OrderAttribute orderAttr = attr as OrderAttribute;

                    priorityInfo.Add(new KeyValuePairObject<int, string, int>(Convert.ToInt32(EnumExtensions.GetEnumValue<DateFilterEnum>(member.Name)), member.GetCustomAttribute<DisplayAttribute>().GetName() ?? member.Name, ((orderAttr != null) ? orderAttr.priority : int.MaxValue)));
                }
            }
            return priorityInfo;
        }
        public static List<KeyValuePairObject<int, string, int>> GetEnumListWithShortNamePriority<TEnum>() where TEnum : Enum
        {
            List<KeyValuePairObject<int, string, int>> priorityInfo = new List<KeyValuePairObject<int, string, int>>();
            MemberInfo[] members = typeof(TEnum).GetMembers();

            foreach (MemberInfo member in members)
            {
                object[] attrs = member.GetCustomAttributes(typeof(OrderAttribute), false);
                foreach (object attr in attrs)
                {
                    OrderAttribute orderAttr = attr as OrderAttribute;

                    priorityInfo.Add(new KeyValuePairObject<int, string, int>(Convert.ToInt32(EnumExtensions.GetEnumValue<DateFilterEnum>(member.Name)),
                        member.GetCustomAttribute<DisplayAttribute>().GetShortName() ?? member.GetCustomAttribute<DisplayAttribute>().GetName() ?? member.Name,
                        ((orderAttr != null) ? orderAttr.priority : int.MaxValue)));
                }
            }
            return priorityInfo;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class OrderAttribute : Attribute
    {
        public readonly int priority;

        public OrderAttribute(int priority)
        {
            this.priority = priority;
        }
        //public int GetPriority<TEnum>(TEnum value) where TEnum : struct
        //{
        //    return OrderHelper.GetOrder<TEnum>(value);
        //}
    }

    public static class OrderHelper
    {
        public static int GetOrder<TEnum>(TEnum value) where TEnum : struct
        {
            int order;

            if (!OrderHelperImpl<TEnum>.Values.TryGetValue(value, out order))
            {
                order = int.MaxValue;
            }

            return order;
        }

        private static class OrderHelperImpl<TEnum>
        {
            public static readonly Dictionary<TEnum, int> Values;

            static OrderHelperImpl()
            {
                var values = new Dictionary<TEnum, int>();

                var fields = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);

                int unordered = int.MaxValue - 1;

                for (int i = fields.Length - 1; i >= 0; i--)
                {
                    FieldInfo field = fields[i];

                    var order = (OrderAttribute)field.GetCustomAttributes(typeof(OrderAttribute), false).FirstOrDefault();

                    int order2;

                    if (order != null)
                    {
                        order2 = order.priority;
                    }
                    else
                    {
                        order2 = unordered;
                        unordered--;
                    }

                    values[(TEnum)field.GetValue(null)] = order2;
                }

                Values = values;
            }
        }
    }
}
