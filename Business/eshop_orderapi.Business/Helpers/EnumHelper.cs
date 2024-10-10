using eshop_orderapi.Business.ViewModels.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eshop_orderapi.Business.Helpers
{
    public class EnumHelper
    {
        public static string GetDisplayValue(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];
            return descriptionAttributes != null && (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        public static List<LookupItem> GetLookUpItems<T>()
        {
            var items = new List<LookupItem>();
            foreach (var i in Enum.GetValues(typeof(T)).Cast<T>())
            {
                items.Add(new LookupItem
                {
                    Id = Convert.ToInt32(i),
                    Name = GetDisplayValue((Enum)(object)i)
                });
            }
            return items;
        }

        public static string GetCustomDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return (descriptionAttributes != null && descriptionAttributes.Length > 0) ? descriptionAttributes[0].Description : string.Empty;
        }
    }
}