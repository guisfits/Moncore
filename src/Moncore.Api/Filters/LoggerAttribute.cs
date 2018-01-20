using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Moncore.Api.Filters
{
    public class LoggerAttribute : ActionFilterAttribute
    {
        private readonly ILogger<LoggerAttribute> _logger;

        public LoggerAttribute(ILogger<LoggerAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var data = new
            {
                Version = "v1",
                User = context.HttpContext.User.Identity.Name,
                IP = context.HttpContext.Connection.LocalIpAddress.ToString(),
                Hostname = context.HttpContext.Request.Host.ToString(),
                Action = context.ActionDescriptor.DisplayName,
                Controller = context.Controller.GetType().Name
            };

            _logger.LogInformation(1, data.ToString());
            _logger.LogTrace(">>> logado <<<");
        }
    }
}
