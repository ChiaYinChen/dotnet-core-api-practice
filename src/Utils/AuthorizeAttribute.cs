using Microsoft.AspNetCore.Mvc.Filters;
using WebApiApp.Constants;
using WebApiApp.Entities;
using WebApiApp.Helpers;

namespace WebApiApp.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // Authorization
            var account = (Account?)context.HttpContext.Items["Account"];
            if (account == null)
            {
                throw new UnauthenticatedError(CustomErrorCode.NotAuthenticated, "Not authenticated");
            }
        }
    }
}
