using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moncore.Api.Helpers;
using Moncore.Domain.Entities;

namespace Moncore.Api.Controllers
{
    public abstract class BaseController<TEntity> : Controller where TEntity : Entity
    {
        protected const int MaxPageSize = 20;
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

        protected string CreateResourceUri(int page, int size, ResourceUriType type)
        {
            var actionName = $"Get{typeof(TEntity).Name}";
            switch (type)
            {
                case ResourceUriType.NextPage:
                    return _urlHelper.Link(actionName, new
                    {
                        page = page + 1,
                        size = size
                    });
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link(actionName, new
                    {
                        page = page - 1,
                        size = size
                    });
                default:
                    return _urlHelper.Link(actionName, new
                    {
                        page = page,
                        size = size
                    });
            }
        }
    }
}