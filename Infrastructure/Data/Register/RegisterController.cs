using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using AutoMapper;
using MyApp.Bll.Db;
using MyApp.Bll.Models;
using MyApp.Web.Data.Auth;
using MyApp.Web.Data.Register;
using MyApp.Web.Infrastructure;

namespace MyApp.Web.Data.Register
{
  [Route("api/register")]
  public class RegisterController : Controller
  {
    private MyAppContext Db { get; }
    private IMapper Mapper { get; }
    private UserManager<MyAppUser> UserManager { get; }
    private SignInManager<MyAppUser> SignInManager { get; }
    private DbSet<MyAppUser> Table { get; }
    public RegisterController(MyAppContext db, IMapper mapper, UserManager<MyAppUser> userManager, SignInManager<MyAppUser> signInManager)
      => (Db, Mapper, Table, UserManager, SignInManager) = (db, mapper, db.Users, userManager, signInManager);

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Post([FromBody] RegisterModel viewModel)
    {
      var record = new MyAppUser {};
      if (viewModel.Password != viewModel.ConfirmPassword)
        return new BadRequestObjectResult("Passwords do not match.");
      return await VaidateAndUpdateModel(viewModel, record);
    }

    protected async Task<IActionResult> VaidateAndUpdateModel(RegisterModel viewModel, MyAppUser record)
    {
      if (ModelState.IsValid == false)
        return new BadRequestObjectResult(ModelState);
      
      var user = await UserManager.FindByNameAsync(viewModel.UserName);
      if (user == null) {
        user = new MyAppUser {
          UserName = viewModel.UserName,
          Email = viewModel.Email,
          AccountType = (int)Role.Standard
        };
        var result = await UserManager.CreateAsync(user, viewModel.Password);
        if (!result.Succeeded) return new BadRequestObjectResult(ModelState);
        // add user roles
        await UserManager.AddToRoleAsync(user, "MyAppUser");
        await UserManager.AddToRoleAsync(user, "Standard");
        // add user record
        var customUser = new CustomUser {
          MyAppUserId = user.Id,
        };
        Db.Add<CustomUser>(customUser);
        Db.SaveChanges();

        // TODO - send email confirmation
        
        // sign the user in
        var signInResult = await SignInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);
        if (signInResult.Succeeded) {
          return Ok(Mapper.Map<CustomUser, CustomUserDetailsModel>(customUser));
        }
        return BadRequest();
      }
      return BadRequest();
    }
  }
}