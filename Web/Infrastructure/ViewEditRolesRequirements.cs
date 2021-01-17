using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyApp.Web.Infrastructure
{
    public class ViewEditRolesRequirement : IAuthorizationRequirement
    {
        public ViewEditRolesRequirement(Role[] editRoles, Role[] viewRoles)
        {
            if (editRoles != null)
                EditRoles = editRoles.Select(r => r.ToString()).ToArray();
            if (viewRoles != null)
                ViewRoles = viewRoles.Select(r => r.ToString()).ToArray();
        }

        public string[] EditRoles { get; }
        public string[] ViewRoles { get; }
    }

    public class ViewEditRolesHandler : AuthorizationHandler<ViewEditRolesRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public ViewEditRolesHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViewEditRolesRequirement requirement)
        {
            if (context.User != null)
            {
                var httpContext = httpContextAccessor.HttpContext;
                var httpMethod = httpContext.Request.Method;
                if (HttpMethods.IsGet(httpMethod))
                {
                    if (requirement.ViewRoles.Any(r => context.User.IsInRole(r)))
                        context.Succeed(requirement);
                }
                else
                {
                    if (requirement.EditRoles.Any(r => context.User.IsInRole(r)))
                        context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
