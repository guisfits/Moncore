using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moncore.Domain.Entities;

namespace Moncore.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> Get(Guid id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> List();
        Task<ICollection<TEntity>> List(Expression<Func<TEntity, bool>> predicate);


        Task<int> Add(TEntity obj);
        Task Add(ICollection<TEntity> objs);
        Task<bool> Update(Guid id, TEntity obj);
        Task<bool> Update(Expression<Func<TEntity, bool>> predicate, TEntity obj);
        Task<bool> Delete(Guid id);
        Task<bool> Delete(Expression<Func<TEntity, bool>> predicate);
    }
}
