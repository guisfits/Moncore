using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Helpers;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Helpers;
//Test GitKraken
namespace Moncore.Api.Controllers
{
    public abstract class BaseController<TEntity> : Controller where TEntity : Entity
    {
        protected readonly IUrlHelper _urlHelper;

        protected BaseController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

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