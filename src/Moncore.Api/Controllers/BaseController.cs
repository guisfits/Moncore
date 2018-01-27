using Microsoft.AspNetCore.Mvc;
using Moncore.Api.Helpers;
using Moncore.CrossCutting.Helpers;
using Moncore.Domain.Entities;

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

        protected string CreateResourceUri(PaginationParameters parameters, ResourceUriType type)
        {
            var actionName = $"Get{typeof(TEntity).Name}s";
            switch (type)
            {
                case ResourceUriType.NextPage:
                    return _urlHelper.Link(actionName, new
                    {
                        page = parameters.Page + 1,
                        size = parameters.Size
                    });
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link(actionName, new
                    {
                        page = parameters.Page - 1,
                        size = parameters.Size
                    });
                default:
                    return _urlHelper.Link(actionName, new
                    {
                        page = parameters.Page,
                        size = parameters.Size
                    });
            }
        }
    }
}