using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moncore.Domain.Entities.Base;

namespace Moncore.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<ICollection<TEntity>> Get();
        Task<TEntity> Get(int id);
        Task<ICollection<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<int> Add(TEntity obj);
        Task AddRange(ICollection<TEntity> objs);
        Task<bool> Update(int id, TEntity obj);
        Task<bool> UpdateRange(Expression<Func<TEntity, bool>> predicate, TEntity obj);
        Task<bool> Delete(int id);
        Task<bool> DeleteRange(Expression<Func<TEntity, bool>> predicate);
    }
}
