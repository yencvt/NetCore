using System;
using System.Net.NetworkInformation;
using common.Constants;
using common.Models.Base;
using Microsoft.AspNetCore.Mvc;

namespace common.Exceptions
{
    public class InvalidModelStateResponse
    {
        public static IActionResult CreateResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
            );

            var problemDetails = new ResponseBase<IDictionary<string, string[]>>(Messages.InvalidDataInput.Code, Messages.InvalidDataInput.Message, errors);

            return new BadRequestObjectResult(problemDetails);
        }
    }
}

