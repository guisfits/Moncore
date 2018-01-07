using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moncore.Domain.Entities.Base;

namespace Moncore.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<ICollection<TEntity>> GetAll();
        Task<TEntity> Get(int id);
        Task<ICollection<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        Task Add(TEntity obj);
        Task<bool> Update(int id, TEntity obj);
        Task<bool> Delete(int id);
    }
}
