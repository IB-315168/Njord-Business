using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;


namespace Domain.Auth
{
    public static class AuthorizationPolicies
    {
        public static void AddPolicies(IServiceCollection services)
        {
            /* Policies example file */
            //services.AddAuthorizationCore(options =>
            //{
            //    options.AddPolicy("MustBeVia", a =>
            //        a.RequireAuthenticatedUser().RequireClaim("Domain", "via"));

            //    options.AddPolicy("SecurityLevel4", a =>
            //        a.RequireAuthenticatedUser().RequireClaim("SecurityLevel", "4", "5"));

            //    options.AddPolicy("MustBeTeacher", a =>
            //        a.RequireAuthenticatedUser().RequireClaim("Role", "Teacher"));

            //    options.AddPolicy("SecurityLevel2OrAbove", a =>
            //        a.RequireAuthenticatedUser().RequireAssertion(context =>
            //        {
            //            Claim? levelClaim = context.User.FindFirst(claim => claim.Type.Equals("SecurityLevel"));
            //            if (levelClaim == null) return false;
            //            return int.Parse(levelClaim.Value) >= 2;
            //        }));
            //});
        }
    }
}
