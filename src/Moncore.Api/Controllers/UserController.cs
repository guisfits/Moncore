using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities.UserAggregate;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _repository.Get();
        }

        public async Task<User> Get(int id)
        {
            return await _repository.Get(id);
        }

        public async void Post([FromBody]User user)
        {
            await _repository.Add(user);
        }

        public async void Put(int id, [FromBody]User user)
        {
            await _repository.Update(id, user);
        }

        public async void Delete(int id)
        {
            await _repository.Delete(id);
        }
    }
}