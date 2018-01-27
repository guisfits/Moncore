using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Filters;
using Moncore.Api.Helpers;
using Moncore.Api.Models;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/users/{userId:guid}/posts")]
    [Route("api/posts")]
    public class PostController : BaseController<Post>
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
            var postsResult = !string.IsNullOrEmpty(userId) 
                ? _repository.List(c => c.UserId == userId) 
                : _repository.List();

            if (postsResult.Result == null)
                return NotFound();

            return Ok(await postsResult);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(string userId, string id)
        {
            var postResult = !string.IsNullOrEmpty(userId) 
                ? _repository.Get(c => c.UserId == userId && c.Id == id) 
                : _repository.Get(id);

            if (postResult.Result == null)
                return NotFound();

            return Ok(await postResult);
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Create(string userId, [FromBody] PostForCreatedDto model)
        {
            var post = Mapper.Map<Post>(model);

            if (!string.IsNullOrEmpty(userId))
            {
                var user = _userRepository.Get(userId);
                if (user == null)
                    return NotFound();

                post.UserId = userId;
            }

            if(!ValidateEntity(post))
                return new UnprocessableEntityResult(ModelState);

            var result = _repository.Add(post);

            return result.IsFaulted
                ? (IActionResult) new StatusCodeResult(StatusCodes.Status501NotImplemented)
                : Created("Get", post);
        }

        [HttpPost("{id:guid}")]
        public IActionResult CreateForErrorInput(string id)
        {
            var user = _repository.Get(id);

            return user == null 
                ? NotFound()
                : new StatusCodeResult(StatusCodes.Status409Conflict);
        }

        [ValidateModelState]
        [HttpPut("{id:guid}")]
        public IActionResult Update(string userId, string id, [FromBody] PostForCreatedDto model)
        {
            Task<Post> postTask;
            if (!string.IsNullOrEmpty(userId))
            {
                postTask = _repository.Get(c => c.UserId == userId && c.Id == id);
                model.UserId = userId;
            }
            else
            {
                postTask = _repository.Get(id);
            }

            if (postTask.Result == null) 
                return NotFound();

            var post = Mapper.Map(model, postTask.Result);

            if (!ValidateEntity(post))
                return new UnprocessableEntityResult(ModelState);

            var result = _repository.Update(id, post);

            return result.IsFaulted
                ? (IActionResult) new StatusCodeResult(StatusCodes.Status501NotImplemented) 
                : Ok(post);
        }

        [ValidateModelState]
        [HttpPatch("{id:guid}")]
        public IActionResult Patch(string userId, string id, [FromBody] JsonPatchDocument<PostForCreatedDto> model)
        {
            var postDb = _repository.Get(id).Result;
            if (postDb == null)
                return NotFound();

            var postPatch = Mapper.Map<PostForCreatedDto>(postDb);
            model.ApplyTo(postPatch, ModelState);
            TryValidateModel(postPatch);
            Mapper.Map(postPatch, postDb);

            var validateResult = ValidateEntity(postDb);
            if (!ModelState.IsValid || !validateResult)
                return new UnprocessableEntityResult(ModelState);

            var result = _repository.Update(id, postDb);

            return result.IsFaulted
                ? (IActionResult) new StatusCodeResult(StatusCodes.Status501NotImplemented)
                : Ok(postDb);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(string userId, string id)
        {
            var post = !string.IsNullOrEmpty(userId) 
                ? _repository.Get(c => c.UserId == userId && c.Id == id) 
                : _repository.Get(id);

            if (post == null)
                return NotFound();

            _repository.Delete(id);
            return NoContent();
        }
    }
}