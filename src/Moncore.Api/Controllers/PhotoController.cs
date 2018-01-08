using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class PhotoController : BaseController<Photo>
    {
        private readonly IPhotoRepository _photoRepository;

        public PhotoController(IPhotoRepository repository) 
            : base(repository)
        {
            _photoRepository = repository;
        }
    }
}