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
    }
}
