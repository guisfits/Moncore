using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities.UserAggregate;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class UserController : BaseController<User>
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
            :base(repository)
        {
            _repository = repository;
        }
    }
}