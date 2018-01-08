using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class AlbumController : BaseController<Album>
    {
        public AlbumController(IAlbumRepository repository) 
            : base(repository)
        {
        }
    }
}