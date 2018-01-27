using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Moncore.Api.Helpers
{
    public class UnprocessableEntityResult : ObjectResult
    {
        public UnprocessableEntityResult(ModelStateDictionary modelState) 
            : base(new SerializableError(modelState))
        {
            if(modelState == null)
                throw new ArgumentException(nameof(modelState));

            StatusCode = 422;
        }
    }
}
