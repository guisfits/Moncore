using System.Linq;
using Moncore.CrossCutting.Helpers;
using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) 
            : base(context)
        {
        }

        protected override IQueryable<User> SearchByParameters(IQueryable<User> objs, PaginationParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
