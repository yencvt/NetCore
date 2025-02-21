using System;
using common.Models.Securities;
using System.Security.Claims;

namespace common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static CustomUser ToCustomUser(this ClaimsPrincipal principal)
        {
            return new CustomUser(principal);
        }
    }

}

