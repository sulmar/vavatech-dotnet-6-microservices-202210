using CatalogService.Api.AuthorizationRequirements;
using CatalogService.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CatalogService.Api.AuthorizationHandlers
{
    public class TheSameOwnerHandler : AuthorizationHandler<TheSameOwnerRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TheSameOwnerRequirement requirement, Product resource)
        {
            string username = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (resource.Owner == username)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }


}
