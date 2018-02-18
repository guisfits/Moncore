using System.Collections.Generic;
using Moncore.CrossCutting.Helpers;

namespace Moncore.CrossCutting.Interfaces
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMappings<TSource, TDestination>();
    }
}