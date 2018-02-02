using System.Collections.Generic;
using Moncore.Api.Services;

namespace Moncore.Api.Interfaces
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
    }
}