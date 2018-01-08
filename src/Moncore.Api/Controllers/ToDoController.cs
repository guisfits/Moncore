using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    public class ToDoController : BaseController<ToDo>
    {
        public ToDoController(IToDoRepository repository) 
            : base(repository)
        {
        }
    }
}