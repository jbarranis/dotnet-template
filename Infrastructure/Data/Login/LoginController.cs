using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MyApp.Bll.Db;
using MyApp.Bll.Models;
using MyApp.Web.Data.Auth;
using MyApp.Web.Data.Login;
using MyApp.Web.Infrastructure;

namespace MyApp.Web.Data.Login
{
  [Route("api/login")]
  public class LoginController : Controller
  {
    private MyAppContext Db { get; }
    private IMapper Mapper { get; }
    private UserManager<MyAppUser> UserManager { get; }
    private IUserClaimsPrincipalFactory<MyAppUser> ClaimsPrincipalFactory { get; }
    private SignInManager<MyAppUser> SignInManager { get; }
    public LoginController(MyAppContext db, IMapper mapper, UserManager<MyAppUser> userManager,
      IUserClaimsPrincipalFactory<MyAppUser> claimsPrincipalFactory, SignInManager<MyAppUser> signInManager)
      => (Db, Mapper, UserManager, ClaimsPrincipalFactory, SignInManager) = (db, mapper, userManager, claimsPrincipalFactory, signInManager);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LoginModel loginModel) {
      if (!ModelState.IsValid) {
        return BadRequest();
      }
      var signInResult = await SignInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);

      if (signInResult.Succeeded) {
        var currentUser = Db.Users
          .Include(x => x.CustomUser)
          .SingleOrDefault(c => c.UserName == loginModel.Username);

        var roles = await UserManager.GetRolesAsync(currentUser);

        if (currentUser == null) {
          return BadRequest();
        }

        return Ok(Mapper.Map<MyAppUser,CustomUserDetailsModel>(currentUser));
      }
      return BadRequest("incorrect login");
    }
  }
}