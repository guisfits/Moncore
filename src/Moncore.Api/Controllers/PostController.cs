using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Filters;
using Moncore.Api.Helpers;
using Moncore.Api.Models;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;
using Newtonsoft.Json;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class PostController : BaseController<Post>
    {
        private readonly IPostRepository _repository;
        private readonly IUserRepository _userRepository;

        public PostController(IPostRepository repository, IUserRepository userRepository, IUrlHelper urlHelper)
            :base(urlHelper)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("api/posts", Name = "GetPosts")]
        public IActionResult Get(PaginationParametersFiltersForPost parameters)
        {
            var posts = _repository.Pagination(parameters);

            if (posts == null || !posts.Any())
                return NotFound();

            string previousPage = posts.HasPrevious
                ? CreateResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            string nextPage = posts.HasNext
                ? CreateResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = posts.TotalCount,
                pageSize = posts.PageSize,
                currentPage = posts.CurrentPage,
                totalPages = posts.TotalPages,
                previousPageLink = previousPage,
                nextPageLink = nextPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(posts);
        }

        

        [HttpGet]
        [Route("api/users/{userId:guid}/posts", Name = "GetPostsByUser")]
        public IActionResult Get(string userId, PaginationParameters parameters)
        {
            var posts = _repository.Pagination(parameters, c => c.UserId == userId);

            if (posts == null)
                return NotFound();

            string previousPage = posts.HasPrevious
                ? CreateResourceUri(parameters, ResourceUriType.PreviousPage, userId)
                : null;

            string nextPage = posts.HasNext
                ? CreateResourceUri(parameters, ResourceUriType.NextPage, userId)
                : null;

            var paginationMetadata = new
            {
                totalCount = posts.TotalCount,
                pageSize = posts.PageSize,
                currentPage = posts.CurrentPage,
                totalPages = posts.TotalPages,
                previousPageLink = previousPage,
                nextPageLink = nextPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(posts);
        }

        [HttpGet]
        [Route("api/posts/{id:guid}")]
        [Route("api/users/{userId:guid}/posts/{id:guid}")]
        public async Task<IActionResult> Get(string userId, string id)
        {
            var postResult = !userId.IsNullEmptyOrWhiteSpace()
                ? _repository.Get(c => c.UserId == userId && c.Id == id) 
                : _repository.Get(id);

            if (postResult.Result == null)
                return NotFound();

            return Ok(await postResult);
        }

        [ValidateModelState]
        [HttpPost]
        [Route("api/posts")]
        [Route("api/users/{userId:guid}/posts")]
        public IActionResult Create(string userId, [FromBody] PostForCreatedDto model)
        {
            var post = Mapper.Map<Post>(model);

            if (!userId.IsNullEmptyOrWhiteSpace())
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

        [HttpPost]
        [Route("api/posts/{id:guid}")]
        [Route("api/users/{userId:guid}/posts/{id:guid}")]
        public IActionResult CreateForErrorInput(string id)
        {
            var user = _repository.Get(id);

            return user == null 
                ? NotFound()
                : new StatusCodeResult(StatusCodes.Status409Conflict);
        }

        [ValidateModelState]
        [HttpPut]
        [Route("api/posts/{id:guid}")]
        [Route("api/users/{userId:guid}/posts/{id:guid}")]
        public IActionResult Update(string userId, string id, [FromBody] PostForCreatedDto model)
        {
            Task<Post> postTask;
            if (!userId.IsNullEmptyOrWhiteSpace())
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
        [HttpPatch]
        [Route("api/posts/{id:guid}")]
        [Route("api/users/{userId:guid}/posts/{id:guid}")]
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

        [HttpDelete]
        [Route("api/posts/{id:guid}")]
        [Route("api/users/{userId:guid}/posts/{id:guid}")]
        public IActionResult Delete(string userId, string id)
        {
            var post = !userId.IsNullEmptyOrWhiteSpace()
                ? _repository.Get(c => c.UserId == userId && c.Id == id) 
                : _repository.Get(id);

            if (post == null)
                return NotFound();

            _repository.Delete(id);
            return NoContent();
        }

        #region Helpers

        private string CreateResourceUri(PaginationParameters parameters, ResourceUriType type, string userId)
        {
            var actionName = "GetPostsByUser";
            switch (type)
            {
                case ResourceUriType.NextPage:
                    return _urlHelper.Link(actionName, new
                    {
                        userId = userId,
                        page = parameters.Page + 1,
                        size = parameters.Size
                    });
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link(actionName, new
                    {
                        userId = userId,
                        page = parameters.Page - 1,
                        size = parameters.Size
                    });
                default:
                    return _urlHelper.Link(actionName, new
                    {
                        userId = userId,
                        page = parameters.Page,
                        size = parameters.Size
                    });
            }
        }

        #endregion
    }
}