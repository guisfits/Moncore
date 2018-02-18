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
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            this._propertyMappings.Add(new PropertyMapping<User, UserDto>(_userPropertyMapping));
            this._propertyMappings.Add(new PropertyMapping<Post, PostDto>(_postPropertyMapping));
            this._propertyMappings.Add(new PropertyMapping<Post, PostsByUserDto>(_postByUserPropertyMapping));
        }

        #region EntitiesPropertyMappings

        private Dictionary<string, PropertyMappingValue> _userPropertyMapping = new Dictionary<string, PropertyMappingValue>()
        {
            {"id", new PropertyMappingValue(new List<string>() {"Id"})},
            {"name", new PropertyMappingValue(new List<string>() {"Name"})},
            {"username", new PropertyMappingValue(new List<string>() {"Username"})},
            {"email", new PropertyMappingValue(new List<string>() {"Email"})},
            {"phone", new PropertyMappingValue(new List<string>() {"Phone"})},
            {"website", new PropertyMappingValue(new List<string>() {"Website"})}
        };

        private Dictionary<string, PropertyMappingValue> _postPropertyMapping = new Dictionary<string, PropertyMappingValue>()
        {
            {"id", new PropertyMappingValue(new List<string>() {"Id"})},
            {"userId", new PropertyMappingValue(new List<string>() {"UserId"})},
            {"title", new PropertyMappingValue(new List<string>() {"Title"})},
            {"body", new PropertyMappingValue(new List<string>() {"Body"})},
        };

        private Dictionary<string, PropertyMappingValue> _postByUserPropertyMapping = new Dictionary<string, PropertyMappingValue>()
        {
            {"id", new PropertyMappingValue(new List<string>() {"Id"})},
            {"title", new PropertyMappingValue(new List<string>() {"Title"})},
            {"body", new PropertyMappingValue(new List<string>() {"Body"})},
        };

        #endregion

        public Dictionary<string, PropertyMappingValue> GetPropertyMappings<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if(matchingMapping.Count() == 1)
            {
                return matchingMapping.First().MappingDictionary;
            }
            throw new ArgumentException($"Cannot find exact property mapping instance for <{typeof(TSource).Name}>, <{typeof(TDestination).Name}>");
        }
    }
}