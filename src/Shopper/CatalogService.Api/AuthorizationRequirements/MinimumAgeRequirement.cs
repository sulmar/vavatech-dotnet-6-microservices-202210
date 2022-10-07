using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CatalogService.Api.AuthorizationRequirements
{

    public class MinimumAgeRequirement : IAuthorizationRequirement // marked interface
    {
        public int MinimumAge { get; }

        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }

    public static class MinimumAgeRequirementExtensions
    {
        public static AuthorizationPolicyBuilder RequireMinimumAge(this AuthorizationPolicyBuilder policy, int age)
        {
            policy.RequireClaim(ClaimTypes.DateOfBirth);
            policy.Requirements.Add(new MinimumAgeRequirement(age));

            return policy;
        }
    }
}
