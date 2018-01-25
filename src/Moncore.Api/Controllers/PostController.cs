using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.MappingProfiles;
using Moncore.Api.Models;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/users/{userId:guid}/posts")]
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IPostRepository _repository;
        private readonly IUserRepository _userRepository;

        public PostController(IPostRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string userId)
        {
            Task<ICollection<Post>> postsResult;
            if (!string.IsNullOrEmpty(userId))
                postsResult = _repository.List(c => c.UserId == userId);
            else
                postsResult = _repository.List();

            if (postsResult.Result == null)
                return NotFound();

            return Ok(await postsResult);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(string userId, string id)
        {
            var postResult = !string.IsNullOrEmpty(userId) ? _repository.Get(c => c.UserId == userId && c.Id == id) : _repository.Get(id);
            if (postResult.Result == null)
                return NotFound();

            return Ok(await postResult);
        }

        [HttpPost]
        public IActionResult Create(string userId, [FromBody] PostForCreatedDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dados incorretos");

            var post = Mapper.Map<Post>(model);

            if (!string.IsNullOrEmpty(userId))
            {
                var user = _userRepository.Get(userId);
                if (user == null)
                    return NotFound();

                post.UserId = userId;
            }

            if (string.IsNullOrEmpty(post.UserId))
                return BadRequest("É necessário informar o UserId");

            var result = _repository.Add(post);

            if (result.IsFaulted)
                return BadRequest("Não foi possível adicionar um novo post");

            return Created("Get", post);
        }

        [HttpPost("{id:guid}")]
        public IActionResult Create(string id)
        {
            var user = _repository.Get(id);
            if (user == null)
                return NotFound();

            return new StatusCodeResult(StatusCodes.Status409Conflict);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(string userId, string id)
        {
            Task<Post> postResult;
            if (!string.IsNullOrEmpty(userId))
                postResult = _repository.Get(c => c.UserId == userId && c.Id == id);
            else
                postResult = _repository.Get(id);

            if (postResult == null)
                return NotFound();

            _repository.Delete(id);
            return NoContent();
        }
    }
}