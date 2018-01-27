using Moncore.CrossCutting.Extensions;
using Moncore.Domain.Entities;
using Moncore.Domain.ValueObjects;
using MongoDB.Bson.Serialization;

namespace Moncore.Data.Helpers
{
    public static class MappingElements
    {
        public static void Initialize()
        {
            BsonClassMap.RegisterClassMap<Entity>(map =>
            {
                map.MapMember(c => c.Id).SetElementName("_id");
            });

            MapClass<User>();
            MapClass<Address>();
            MapClass<Company>();
            MapClass<Geo>();
            MapClass<Post>();
        }

        private static void MapClass<T>()
        {
            BsonClassMap.RegisterClassMap<T>(map =>
            {
                var properties = typeof(T).GetProperties();
                foreach (var property in properties)
                {
                    if (property.Name != "Id")
                    {
                        map.MapMember(property).SetElementName(property.ToCamelCase());
                    }
                }
            });
        }
    }
}
