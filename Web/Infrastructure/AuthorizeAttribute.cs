using System;

namespace MyApp.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        /// <summary>
        /// Specifies authorization based on a Right assigned to the Role (as a Claim)
        /// </summary>
        public AuthorizeAttribute(Right right) 
            => Policy = right.ToString();
    }
}
