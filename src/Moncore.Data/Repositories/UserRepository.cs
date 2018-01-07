using Moncore.Data.Context;
using Moncore.Domain.Entities.UserAggregate;
using Moncore.Domain.Interfaces;

namespace Moncore.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
