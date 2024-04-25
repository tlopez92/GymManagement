using System.Security.Claims;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Application.Models;
using Throw;

namespace GymManagement.Api.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        httpContextAccessor.HttpContext.ThrowIfNull();

        var idClaim = GetClaims("id").Select(Guid.Parse).First();

        var permissionClaims = GetClaims("permissions");
        
        var roles = GetClaims(ClaimTypes.Role);

        return new CurrentUser(Id: idClaim, Permissions: permissionClaims, Roles: roles);
    }
    
    private IReadOnlyList<string> GetClaims(string claimType)
    {
        return httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}