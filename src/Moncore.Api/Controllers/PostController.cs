using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        public PostController(IPostRepository repository) 
        {
            _repository = repository;
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