using Microsoft.AspNetCore.Mvc.Filters;
using Moncore.Api.Helpers;

namespace Moncore.Api.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityResult(context.ModelState);

            base.OnActionExecuting(context);
        }
    }
}
