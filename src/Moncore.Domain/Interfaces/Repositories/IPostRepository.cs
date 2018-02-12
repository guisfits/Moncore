using System;
using System.Linq.Expressions;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Helpers;

namespace Moncore.Domain.Interfaces.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        PagedList<Post> Pagination(PostParameters parameters, Expression<Func<Post, bool>> predicate = null);

    }
}
