using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moncore.Data.Context;
using Moncore.Domain.Entities.Base;
using Moncore.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Moncore.Data.Repositories
{
    internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoCollection<TEntity> document;

        protected Repository(ApplicationContext context)
        {
            document = context.MongoDb.GetCollection<TEntity>(typeof(TEntity).Name + "s");
        }

        public virtual async Task<ICollection<TEntity>> Get()
        {
            return await document.Find(c => true).ToListAsync();
        }

        public virtual async Task<TEntity> Get(int id)
        {
            return await document.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<ICollection<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await document.Find(predicate).ToListAsync();
        }

        public virtual async Task Add(TEntity obj)
        {
            await document.InsertOneAsync(obj);
        }

        public virtual async Task AddRange(ICollection<TEntity> objs)
        {
            await document.InsertManyAsync(objs);
        }

        public virtual async Task<bool> Update(int id, TEntity obj)
        {
            var actionResult = await document.ReplaceOneAsync(n => n.Id.Equals(id), obj, new UpdateOptions { IsUpsert = true });
            return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
        }

        public virtual async Task<bool> UpdateRange(Expression<Func<TEntity, bool>> predicate, TEntity obj)
        {
            var update = new BsonDocumentUpdateDefinition<TEntity>(obj.ToBsonDocument());
            var result = await document.UpdateManyAsync(predicate, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var result = await document.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public virtual async Task<bool> DeleteRange(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await document.DeleteManyAsync(predicate);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
