using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Booky.PermissionFilter;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirment>
{
    public PermissionAuthorizationHandler()
    {

    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
    {
        if (context.User == null) return;

        var permissions = context.User.Claims.Where(c => c.Type == "Permission"
        && c.Value == requirement.Permission && c.Issuer == "LOCAL AUTHORITY");

        if (permissions.Any())
        {
            context.Succeed(requirement);
            return;
        }
    }
}