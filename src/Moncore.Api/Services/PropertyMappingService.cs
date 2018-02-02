using System;
using System.Collections.Generic;
using System.Linq;
using Moncore.Api.Interfaces;
using Moncore.Api.Models;
using Moncore.Domain.Entities;

namespace Moncore.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _userPropertyMapping =
            new Dictionary<string, PropertyMappingValue>()
            {
                {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
                {"Username", new PropertyMappingValue(new List<string>() {"Username"})},
                {"Email", new PropertyMappingValue(new List<string>() {"Email"})}
            };

        private IList<IPropertyMapping> propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            propertyMappings.Add(new PropertyMapping<UserDto, User>(_userPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource).Name}>");
        }
    }
}