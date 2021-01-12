using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApp.Bll.Db;
using MyApp.Bll.Models;
using MyApp.Web.Data.Auth;
using MyApp.Web.Data.Base;
using MyApp.Web.Infrastructure;
using MoreLinq;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Web.Data.Auth
{
  [Route("api/users"), Authorize(Right.MyAppUser)]
  public class UserController : Controller
  {
    protected MyAppContext Db { get; }
    protected IMapper Mapper { get; }
    protected IMediator Mediator { get; }
    private UserManager<MyAppUser> UserManager { get; }
    public UserController(MyAppContext db, IMapper mapper, IMediator mediator, UserManager<MyAppUser> userManager)
      => (Db, Mapper, Mediator, UserManager) = (db, mapper, mediator, userManager);


    [HttpGet("authorizations")]
    public async Task<AuthorizationsViewModel> GetMyAuthorizations()
    {
      var auths = new AuthorizationsViewModel();
      auths.UserName = User.Identity.Name;

      var user = await UserManager.GetUserAsync(User);
      if (user != null)
      {
        var rolesStrings = await UserManager.GetRolesAsync(user);
        auths.Role = rolesStrings.Select(Enum.Parse<Role>);

        auths.EditRights = auths.Role.SelectMany(role => RightsMapping.EditRights[role]);
        auths.ViewRights = auths.Role.SelectMany(role => RightsMapping.ViewRights[role]);
      }
      else
      {
        auths.EditRights = Enumerable.Empty<Right>();
        auths.ViewRights = Enumerable.Empty<Right>();
      }
      return auths;
    }
  }
}