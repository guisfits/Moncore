using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/BaseController")]
    public class BaseController<TEntity> : Controller where TEntity : Entity
    {
        protected bool ValidateEntity(TEntity obj)
        {
            var validationResult = obj.Validate();
            if (!validationResult.IsValid)
            {
                foreach (var erro in validationResult.Errors)
                {
                    ModelState.AddModelError(erro.PropertyName, erro.ErrorMessage);
                }
                return false;
            }
            return true;
        }
    }
}