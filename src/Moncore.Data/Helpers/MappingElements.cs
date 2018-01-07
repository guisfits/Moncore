using Moncore.CrossCutting.Extensions;
using Moncore.Domain.Entities;
using Moncore.Domain.Entities.Base;
using Moncore.Domain.Entities.UserAggregate;
using MongoDB.Bson.Serialization;

namespace Moncore.Data.Helpers
{
    public static class MappingElements
    {
        public static void Initialize()
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(map =>
            {
                map.MapMember(c => c.Id).SetElementName("_id");
            });

            MapClass<User>();
            MapClass<Address>();
            MapClass<Company>();
            MapClass<Geo>();
            MapClass<Post>();
            MapClass<Comment>();
            MapClass<Album>();
            MapClass<Photo>();
            MapClass<ToDo>();
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
