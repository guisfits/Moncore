using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Moncore.Api.Filters;
using Moncore.Api.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;
using Newtonsoft.Json;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Helpers;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UserController : BaseController<User>
    {
        private readonly IUserRepository _repository;
        private readonly IPostRepository _postRepository;

        public UserController(IUserRepository repository, IPostRepository postRepository, IUrlHelper urlHelper) 
            : base(urlHelper)
        {
            _repository = repository;
            _postRepository = postRepository;
        }

        [HttpGet(Name = "GetUsers")]
        public IActionResult Get(UserParameters parameters)
        {
            var users = _repository.Pagination(parameters);
            string previousPage = users.HasPrevious
                ? CreateResourceUri(parameters, ResourceUriType.PreviousPage)
                : null;

            string nextPage = users.HasNext
                ? CreateResourceUri(parameters, ResourceUriType.NextPage)
                : null;

            var result = Mapper.Map<List<UserDto>>(users);

            var paginationMetadata = new
            {
                totalCount = users.TotalCount,
                pageSize = users.PageSize,
                currentPage = users.CurrentPage,
                totalPages = users.TotalPages,
                previousPageLink = previousPage,
                nextPageLink = nextPage
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _repository.Get(id);

            return user == null 
                ? (IActionResult) NotFound() 
                : Ok(user);
        }

        [ValidateModelState]
        [HttpPost]
        public IActionResult Create([FromBody] UserForCreatedDto model)
        {
            var user = Mapper.Map<User>(model);
            if (!ValidateEntity(user))
                return new UnprocessableEntityResult(ModelState);

            var result = _repository.Add(user);

            return result.IsFaulted
                ? (IActionResult) new StatusCodeResult(StatusCodes.Status501NotImplemented)
                : Created("Get", user);
        }

        [HttpPost("{id:guid}")]
        public IActionResult CreateForInvalidInput(string id)
        {
            var user = _repository.Get(id);

            return user == null 
                ? NotFound()
                : new StatusCodeResult(StatusCodes.Status409Conflict);
        }

        [ValidateModelState]
        [HttpPut("{id:guid}")]
        public IActionResult Update(string id, [FromBody] UserForCreatedDto model)
        {
            var user = _repository.Get(id).Result;
            if (user == null)
                return NotFound();

            Mapper.Map(model, user);
            if (!ValidateEntity(user))
                return new UnprocessableEntityResult(ModelState);

            var result = _repository.Update(id, user);

            return result.Result
                ? (IActionResult) Ok(user)
                : new StatusCodeResult(StatusCodes.Status501NotImplemented);
        }

        [ValidateModelState]
        [HttpPatch("{id:guid}")]
        public IActionResult Patch(string id, [FromBody] JsonPatchDocument<UserForCreatedDto> model)
        {
            var userDb = _repository.Get(id).Result;
            if (userDb == null)
                return NotFound();

            var userForPatch = Mapper.Map<UserForCreatedDto>(userDb);
            model.ApplyTo(userForPatch, ModelState);
            Mapper.Map(userForPatch, userDb);

            var validationResult = ValidateEntity(userDb);
            if (!ModelState.IsValid || !validationResult)
                return new UnprocessableEntityResult(ModelState);

            var result = _repository.Update(id, userDb);

            return result.Result
                ? (IActionResult)Ok(userDb)
                : new StatusCodeResult(StatusCodes.Status501NotImplemented);
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
                _postRepository.Delete(post.Id);

            return NoContent();
        }

        private string CreateResourceUri(UserParameters parameters, ResourceUriType type)
        {
            var actionName = "GetUsers";
            switch (type)
            {
                case ResourceUriType.NextPage:
                    return _urlHelper.Link(actionName, new
                    {
                        email = parameters.Email,
                        page = parameters.Page + 1,
                        size = parameters.Size
                    });
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link(actionName, new
                    {
                        email = parameters.Email,
                        page = parameters.Page - 1,
                        size = parameters.Size
                    });
                default:
                    return _urlHelper.Link(actionName, new
                    {
                        email = parameters.Email,
                        page = parameters.Page,
                        size = parameters.Size
                    });
            }
        }
    }
}