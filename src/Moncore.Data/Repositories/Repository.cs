using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moncore.Data.Context;
using Moncore.Domain.Entities.Base;
using Moncore.Domain.Interfaces;
using MongoDB.Driver;

namespace Moncore.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IMongoCollection<TEntity> document;

        public Repository(ApplicationContext context)
        {
            document = context.MongoDb.GetCollection<TEntity>(typeof(TEntity).Name + "s");
        }

        public async Task<ICollection<TEntity>> GetAll()
        {
            return await document.Find(c => true).ToListAsync();
        }

        public Task<TEntity> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task Add(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, TEntity obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
