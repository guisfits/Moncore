using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Models;
using System.Linq;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Get()
        {
            var users = await _repository.Get();
            var result = Mapper.Map<List<UserDto>>(users);
            return Ok(result.OrderBy(c => c.Id));
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            var result = await _repository.Get(id);
            if (result == null)
                return NotFound(null);

            return Ok(result);
        }
    }
}