using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/photos")]
    [Route("api/albums/{albumId}/photos")]
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _repository;
        private readonly IAlbumRepository _albumRepository;

        public PhotoController(IPhotoRepository repository, IAlbumRepository albumRepository)
        {
            _repository = repository;
            _albumRepository = albumRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? userId, int? albumId)
        {
            Task<ICollection<Photo>> result;

            if (!albumId.HasValue)
                result = _repository.Get();
            else
                result = _repository.Get(c => c.AlbumId == albumId.Value);

            if (result.Result == null || !result.Result.Any())
                return NotFound();

            return Ok(await result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? albumId, int id)
        {
            var postResult = albumId.HasValue ? _repository.Find(c => c.AlbumId == albumId.Value && c.Id == id) : _repository.Get(id);
            if (postResult.Result == null)
                return NotFound();

            return Ok(await postResult);
        }
    }
}