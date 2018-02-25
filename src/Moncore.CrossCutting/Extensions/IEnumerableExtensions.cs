using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace Moncore.CrossCutting.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if(source == null)
                throw new ArgumentException("source");

            var expandoObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();

            if (fields.IsNullEmptyOrWhiteSpace())
            {
                var propertyInfos = (typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance));
                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');
                foreach (var property in fieldsAfterSplit)
                {
                    var propertyName = property.Trim();
                    propertyInfoList.Add(typeof(TSource).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase));
                }
            }

            foreach (TSource sourceObject in source)
            {
                var dataSourceObject = new ExpandoObject();
                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)dataSourceObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(dataSourceObject);
            }

            return expandoObjectList;
        }
    }
}
