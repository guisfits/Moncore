using Microsoft.Extensions.Options;
using Moncore.Data.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Entities.UserAggregate;
using MongoDB.Driver;

namespace Moncore.Data.Context
{
    public class ApplicationContext
    {
        public readonly IMongoDatabase MongoDb;
        public readonly MongoClient ClientDb;

        public ApplicationContext(IOptions<DbSettings> settings)
        {
            ClientDb = new MongoClient(settings.Value.ConnectionString);

            if (ClientDb != null)
                MongoDb = ClientDb.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<User> Users => MongoDb.GetCollection<User>("Users");
        public IMongoCollection<Post> Posts => MongoDb.GetCollection<Post>("Posts");
        public IMongoCollection<Album> Albums => MongoDb.GetCollection<Album>("Albums");
        public IMongoCollection<Photo> Photos => MongoDb.GetCollection<Photo>("Photos");
    }
}
