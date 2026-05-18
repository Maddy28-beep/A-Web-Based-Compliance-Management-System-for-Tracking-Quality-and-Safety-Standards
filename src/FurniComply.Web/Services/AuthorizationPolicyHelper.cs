using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FurniComply.Web.Services;

public sealed class AuthorizationPolicyHelper
{
    private readonly IAuthorizationService _authorization;

    public AuthorizationPolicyHelper(IAuthorizationService authorization)
    {
        _authorization = authorization;
    }

    public async Task<bool> HasPolicyAsync(ClaimsPrincipal user, string policyName)
    {
        var result = await _authorization.AuthorizeAsync(user, policyName);
        return result.Succeeded;
    }
}