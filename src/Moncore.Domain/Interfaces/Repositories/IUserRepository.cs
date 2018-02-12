using System;
using System.Linq;
using System.Linq.Expressions;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Helpers;

namespace Moncore.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        PagedList<User> Pagination(UserParameters parameters, Expression<Func<User, bool>> predicate = null);
    }
}
