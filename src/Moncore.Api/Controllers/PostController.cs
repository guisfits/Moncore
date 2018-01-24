﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/users/{userId}/posts")]
    [Route("api/posts")]
    public class PostController : Controller
    {
        private readonly IPostRepository _repository;

        public PostController(IPostRepository repository) 
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid? userId)
        {
            Task<ICollection<Post>> postsResult;
            if (userId.HasValue)
                postsResult = _repository.List(c => c.UserId == userId.Value);
            else
                postsResult = _repository.List();

            if (postsResult.Result == null)
                return NotFound();

            return Ok(await postsResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid? userId, Guid id)
        {
            var postResult = userId.HasValue ? _repository.Get(c => c.UserId == userId.Value && c.Id == id) : _repository.Get(id);
            if (postResult.Result == null)
                return NotFound();

            return Ok(await postResult);
        }
    }
}