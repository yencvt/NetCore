using System;
using System.Security.Claims;

namespace common.Services.Interfaces
{
	public interface IJwtService
	{
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();
    }
}

