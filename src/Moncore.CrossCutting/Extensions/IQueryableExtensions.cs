using System;
using System.Collections.Generic;
using System.Linq;
using Moncore.CrossCutting.Helpers;
using System.Linq.Dynamic.Core;

namespace Moncore.CrossCutting.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, 
                                                 string orderBy, 
                                                 Dictionary<string, PropertyMappingValue> service)
        {
            if(source == null)
                throw new ArgumentException("source");

            if (orderBy.IsNullEmptyOrWhiteSpace())
                return source;

            if(service == null)
                throw new ArgumentException("service");
            
            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByCause in orderByAfterSplit)
            {
                var trimmedOrderByClause = orderByCause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!service.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = service[propertyName];

                if(propertyMappingValue == null)
                    throw new ArgumentException(nameof(propertyMappingValue));

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                        orderDescending = !orderDescending;

                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }
            return source;
        }
    }
}
