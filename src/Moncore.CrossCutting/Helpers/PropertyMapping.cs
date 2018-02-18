using System.Collections.Generic;
using Moncore.CrossCutting.Interfaces;

namespace Moncore.CrossCutting.Helpers
{
    public class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary )
        {
            this.MappingDictionary = mappingDictionary;
        }

        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; set; }
    }
}