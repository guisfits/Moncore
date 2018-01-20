using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/albums")]
    [Route("api/users/{userId}/albums")]
    public class AlbumController : Controller
    {
        private readonly IAlbumRepository _repository;

        public AlbumController(IAlbumRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Get(int? userId)
        {
            Task<ICollection<Album>> albumResut;

            if (userId.HasValue)
                albumResut = _repository.Get(c => c.UserId == userId);
            else
                albumResut = _repository.Get();

            if (albumResut.Result == null)
                return NotFound();

            return Ok(await albumResut);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? userId, int id)
        {
            var postResult = userId.HasValue ? _repository.Find(c => c.UserId == userId.Value && c.Id == id) : _repository.Get(id);
            if (postResult.Result == null)
                return NotFound();

            return Ok(await postResult);
        }
    }
}