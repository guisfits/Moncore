using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Moncore.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IMongoCollection<TEntity> document;

        protected Repository(ApplicationContext context)
        {
            document = context.MongoDb.GetCollection<TEntity>(typeof(TEntity).Name + "s");
        }

        public virtual async Task<ICollection<TEntity>> List()
        {
            return await document
                .Find(c => true)
                .Sort("{_id: 1}")
                .ToListAsync();
        }

        public virtual async Task<TEntity> Get(string id)
        {
            var result = document.Find(entity => entity.Id == id);
            return await result.FirstOrDefaultAsync();
        }

        public virtual async Task<ICollection<TEntity>> List(Expression<Func<TEntity, bool>> predicate)
        {
            return await document
                .Find(predicate)
                .Sort("{_id: 1}")
                .ToListAsync();
        }

        public virtual async Task<TEntity> Get(Expression<Func<TEntity, bool>> expression)
        {
            return await document
                .Find(expression)
                .FirstOrDefaultAsync();
        }

        public virtual async Task Add(TEntity obj)
        {
            obj.Id = Guid.NewGuid().ToString();
            await document.InsertOneAsync(obj);
        }

        public virtual async Task Add(ICollection<TEntity> objs)
        {
            foreach (var obj in objs)
            {
                obj.Id = Guid.NewGuid().ToString();
            }
            await document.InsertManyAsync(objs);
        }

        public virtual async Task<bool> Update(string id, TEntity obj)
        {
            var actionResult = await document.ReplaceOneAsync(n => n.Id.Equals(id), obj, new UpdateOptions { IsUpsert = true });
            return actionResult.IsAcknowledged && actionResult.ModifiedCount > 0;
        }

        public virtual async Task<bool> Update(Expression<Func<TEntity, bool>> predicate, TEntity obj)
        {
            var update = new BsonDocumentUpdateDefinition<TEntity>(obj.ToBsonDocument());
            var result = await document.UpdateManyAsync(predicate, update);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public virtual async Task<bool> Delete(string id)
        {
            var result = await document.DeleteOneAsync(c => c.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public virtual async Task<bool> Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await document.DeleteManyAsync(predicate);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
