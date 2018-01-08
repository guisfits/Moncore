using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class PostController : BaseController<Post>
    {
        public PostController(IPostRepository repository) 
            : base(repository)
        {
        }
    }
}