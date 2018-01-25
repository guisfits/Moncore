using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        private readonly IPostRepository _postRepository;

        public UserController(IUserRepository repository, IPostRepository postRepository)
        {
            _repository = repository;
            this._postRepository = postRepository;
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

        [HttpPost]
        public IActionResult Create(string id)
        {
            var user = _repository.Get(id);
            if (user == null)
                return NotFound();

            return new StatusCodeResult(StatusCodes.Status409Conflict);
        }
        
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(string id)
        {
            var user = _repository.Get(id);

            if (user == null)
                return NotFound();

            _repository.Delete(id);

            var posts = _postRepository.List(c => c.UserId == id).Result;
            foreach (var post in posts)
            {
                _postRepository.Delete(post.Id);
            }
            return NoContent();
        }
    }
}