using System.Collections.Generic;
using MyApp.Web.Infrastructure;

namespace MyApp.Web.Infrastructure.Auth
{
    public class AuthorizationsViewModel
    {
        public string UserName { get; set; } // this is here for debugging
        public IEnumerable<Role> Role { get; set; }
        public IEnumerable<Right> ViewRights { get; set; }
        public IEnumerable<Right> EditRights { get; set; }
    }
}
