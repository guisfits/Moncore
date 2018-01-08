using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class CommentController : BaseController<Comment>
    {
        public CommentController(ICommentRepository repository) 
            : base(repository)
        {
        }
    }
}