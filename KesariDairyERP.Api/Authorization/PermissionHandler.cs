using KesariDairyERP.Shared;
using Microsoft.AspNetCore.Authorization;

namespace KesariDairyERP.Api.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            //  Not authenticated
            if (!context.User.Identity?.IsAuthenticated ?? true)
                return Task.CompletedTask;

            var role = context.User.FindFirst(JwtClaims.Role)?.Value;

            //  SuperAdmin bypass
            if (role == "SuperAdmin")
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            //  Check permission claims
            var permissions = context.User
                .FindAll(JwtClaims.Permission)
                .Select(c => c.Value);

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
