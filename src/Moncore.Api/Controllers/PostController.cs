﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Filters;
using Moncore.Api.Helpers;
using Moncore.Api.Models;
using Moncore.CrossCutting.Extensions;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Helpers;
using Moncore.Domain.Interfaces.Repositories;
using Moncore.Domain.Interfaces.Services;
using Newtonsoft.Json;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class PostController : BaseController<Post>
    {
        private readonly IPostRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IEntityHelperServices _entityHelperServices;

        public PostController(IPostRepository repository, IUserRepository userRepository, IUrlHelper urlHelper, IEntityHelperServices entityHelperServices)
            :base(urlHelper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _entityHelperServices = entityHelperServices;
        }

        [HttpGet]
        [Route("api/posts", Name = "GetPosts")]
        public IActionResult Get(PostParameters parameters)
        {
            if (!_entityHelperServices.EntityHasProperties<Post>(parameters.Fields))
                return BadRequest();

            var posts = _repository.Pagination<PostDto>(parameters);

            if (posts == null)
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

            Response.Headers.Add("X-PaginationResources", JsonConvert.SerializeObject(paginationMetadata));
            var vm = Mapper.Map<List<PostDto>>(posts);
            return Ok(vm.ShapeData(parameters.Fields));
        }

        [HttpGet]
        [Route("api/users/{userId:guid}/posts", Name = "GetPostsByUser")]
        public IActionResult Get(string userId, PostParameters parameters)
        {
            var posts = _repository.Pagination<PostsByUserDto>(parameters, c => c.UserId == userId);

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
            var vm = Mapper.Map<List<PostsByUserDto>>(posts);
            return Ok(vm.ShapeData(parameters.Fields));
        }

        [HttpGet]
        [Route("api/posts/{id:guid}")]
        [Route("api/users/{userId:guid}/posts/{id:guid}")]
        public async Task<IActionResult> Get(string userId, string id, [FromQuery] string fields)
        {
            if (!_entityHelperServices.EntityHasProperties<Post>(fields))
                return BadRequest();

            var post = !userId.IsNullEmptyOrWhiteSpace()
                ? await _repository.Get(c => c.UserId == userId && c.Id == id) 
                : await _repository.Get(id);

            return post == null 
                   ? (IActionResult) NotFound()
                   : Ok(post.ShapeData<Post>(fields));
        }

        [ValidateModelState]
        [HttpPost]
        [Route("api/posts")]
        [Route("api/users/{userId:guid}/posts")]
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
            var post = !string.IsNullOrEmpty(userId) 
                ? _repository.Get(c => c.UserId == userId && c.Id == id) 
                : _repository.Get(id);

            if (post == null)
                return NotFound();

            _repository.Delete(id);
            return NoContent();
        }

        #region Helpers

        private string CreateResourceUri(PostParameters parameters, ResourceUriType type, string userId = null)
        {
            if(userId != null)
            {
                switch (type)
                {
                    case ResourceUriType.NextPage:
                        return _urlHelper.Link("GetPostsByUser", new
                        {
                            userId = userId,
                            page = parameters.Page + 1,
                            size = parameters.Size,
                            search = parameters.Search,
                            title = parameters.Title,
                            body = parameters.Body,
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields
                        });
                    case ResourceUriType.PreviousPage:
                        return _urlHelper.Link("GetPostsByUser", new
                        {
                            userId = userId,
                            page = parameters.Page - 1,
                            size = parameters.Size,
                            search = parameters.Search,
                            title = parameters.Title,
                            body = parameters.Body,
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields
                        });
                    default:
                        return _urlHelper.Link("GetPostsByUser", new
                        {
                            userId = userId,
                            page = parameters.Page,
                            size = parameters.Size,
                            search = parameters.Search,
                            title = parameters.Title,
                            body = parameters.Body,
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields
                        });
                }
            }
            else 
            {
               switch (type)
                {
                    case ResourceUriType.NextPage:
                        return _urlHelper.Link("GetPosts", new
                        {
                            page = parameters.Page + 1,
                            size = parameters.Size,
                            search = parameters.Search,
                            title = parameters.Title,
                            body = parameters.Body,
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields
                        });
                    case ResourceUriType.PreviousPage:
                        return _urlHelper.Link("GetPosts", new
                        {
                            page = parameters.Page - 1,
                            size = parameters.Size,
                            search = parameters.Search,
                            title = parameters.Title,
                            body = parameters.Body,
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields
                        });
                    default:
                        return _urlHelper.Link("GetPosts", new
                        {
                            page = parameters.Page,
                            size = parameters.Size,
                            search = parameters.Search,
                            title = parameters.Title,
                            body = parameters.Body,
                            orderBy = parameters.OrderBy,
                            fields = parameters.Fields
                        });
                } 
            }
            
        }

        #endregion
    }
}