using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace common.Filters
{
    public class ValidateModelFilter : IActionFilter, IFilterMetadata, IOrderedFilter
    {
        int IOrderedFilter.Order => throw new NotImplementedException();

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //if (!context.ModelState.IsValid)
            //{
            //    var errors = context.ModelState
            //        .Where(e => e.Value.Errors.Count > 0)
            //        .ToDictionary(
            //            kvp => kvp.Key,
            //            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

            //    var problemDetails = new ValidationProblemDetails(errors)
            //    {
            //        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            //        Title = "One or more validation errors occurred.",
            //        Status = StatusCodes.Status400BadRequest
            //    };

            //    context.Result = new BadRequestObjectResult(problemDetails);
            //}
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}

