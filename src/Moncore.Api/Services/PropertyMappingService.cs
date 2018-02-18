using System;
using System.Collections.Generic;
using System.Linq;
using Moncore.Api.Models;
using Moncore.CrossCutting.Helpers;
using Moncore.CrossCutting.Interfaces;
using Moncore.Domain.Entities;

namespace Moncore.Api.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private IList<IPropertyMapping> _propertyMappings;

        #region EntitiesPropertyMappings

        private Dictionary<string, PropertyMappingValue> _userPropertyMapping 
            = new Dictionary<string, PropertyMappingValue>()
        {
            {"Id", new PropertyMappingValue(new List<string>() {"Id"})},
            {"Username", new PropertyMappingValue(new List<string>() {"Username"})},
            {"Name", new PropertyMappingValue(new List<string>() {"Name"})},
            {"Email", new PropertyMappingValue(new List<string>() {"Email"})},
            {"Phone", new PropertyMappingValue(new List<string>() {"Phone"})},
            {"Website", new PropertyMappingValue(new List<string>() {"Website"})}
        };

        #endregion

        public Dictionary<string, PropertyMappingValue> GetPropertyMappings<TSource, TDestination> ()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if(matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }
            throw new ArgumentException($"Cannot find exact property mapping instance for <{typeof(TSource).Name}>, <{typeof(TDestination).Name}>");
        }

        public PropertyMappingService()
        {
            this._propertyMappings.Add(new PropertyMapping<UserDto, User>(_userPropertyMapping));
        }
    }
}