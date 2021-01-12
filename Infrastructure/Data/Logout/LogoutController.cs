using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MyApp.Bll.Db;
using MyApp.Bll.Models;
using MyApp.Web.Data.Auth;
using MyApp.Web.Data.Login;

namespace MyApp.Web.Data.Login
{
  [Route("api/logout")]
  public class LogoutController : Controller
  {
    private MyAppContext Db { get; }
    private IMapper Mapper { get; }
    private UserManager<MyAppUser> UserManager { get; }
    private IUserClaimsPrincipalFactory<MyAppUser> ClaimsPrincipalFactory { get; }
    private SignInManager<MyAppUser> SignInManager { get; }
    public LogoutController(MyAppContext db, IMapper mapper, UserManager<MyAppUser> userManager,
      IUserClaimsPrincipalFactory<MyAppUser> claimsPrincipalFactory, SignInManager<MyAppUser> signInManager)
      => (Db, Mapper, UserManager, ClaimsPrincipalFactory, SignInManager) = (db, mapper, userManager, claimsPrincipalFactory, signInManager);

    [HttpPost]
    public async Task<IActionResult> Post() {
      var currentUser = await UserManager.GetUserAsync(User);
      if (currentUser == null) return BadRequest();
      await SignInManager.SignOutAsync();
      return Ok();
    }
  }
}