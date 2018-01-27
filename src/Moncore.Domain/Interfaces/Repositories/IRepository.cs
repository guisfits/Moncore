using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;

namespace Moncore.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> Get(string id);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> List();
        Task<ICollection<TEntity>> List(Expression<Func<TEntity, bool>> predicate);
        PagedList<TEntity> Pagination(PaginationParameters parameters);
        PagedList<TEntity> Pagination(PaginationParameters parameters, Expression<Func<TEntity, bool>> predicate);

        Task Add(TEntity obj);
        Task Add(ICollection<TEntity> objs);
        Task<bool> Update(string id, TEntity obj);
        Task<bool> Update(Expression<Func<TEntity, bool>> predicate, TEntity obj);
        Task<bool> Delete(string id);
        Task<bool> Delete(Expression<Func<TEntity, bool>> predicate);
    }
}
