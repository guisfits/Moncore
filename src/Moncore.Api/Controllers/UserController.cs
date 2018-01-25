using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Models;
using System.Linq;
using Moncore.Domain.Entities;
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
            var users = await _repository.List();
            var result = Mapper.Map<List<UserDto>>(users);
            return Ok(result.OrderBy(c => c.Id));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _repository.Get(id);
            if (result == null)
                return NotFound(null);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserForCreatedDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dados incorretos");

            var user = Mapper.Map<User>(model);
            var result = _repository.Add((User) user);

            if (result.Result <= 0)
                return BadRequest("Não foi possível adicionar um novo usuário");

            return CreatedAtAction("Get", new {id = result.Result}, user);
        }
    }
}