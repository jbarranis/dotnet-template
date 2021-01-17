using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyApp.Bll.Db;
using MyApp.Bll.Models;
using MyApp.Web.Infrastructure.Base;
using MyApp.Web.Infrastructure;
using MoreLinq;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Web.Infrastructure.Auth
{
  [Route("api/profile"), Authorize(Right.MyAppUser)]
  public class CustomUserController : Controller
  {
    protected MyAppContext Db { get; }
    protected IMapper Mapper { get; }
    protected IMediator Mediator { get; }
    private UserManager<MyAppUser> UserManager { get; }
    
    public CustomUserController(MyAppContext db, IMapper mapper, IMediator mediator, UserManager<MyAppUser> userManager)
      => (Db, Mapper, Mediator, UserManager) = (db, mapper, mediator, userManager);

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetails(int id)
    {
      var currentUser = await UserManager.GetUserAsync(User);
      if (currentUser != null) {
        var record = await Db.CustomUser
          .Include(x=> x.MyAppUser)
          .SingleAsync(p => p.MyAppUserId == id);
        if (record == null) return NotFound();

        var vm = Mapper.Map<CustomUserDetailsModel>(record);
        return Ok(vm);
      }
      return BadRequest("access denied");
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateUserRecord(int id, [FromBody] JsonPatchDocument<CustomUser> patchDocument)
    {
      var record = Db.CustomUser
        .Include(x => x.MyAppUser)
        .SingleOrDefault(x => x.MyAppUserId == id);
      if (record == null) {
        return BadRequest();
      }
      // we're not allowing users to edit emails
      if (patchDocument.Operations.Any(op => op.path == "/email")) {
        return BadRequest();
      }
      patchDocument.ApplyTo(record, ModelState);
      var changedCount = (ModelState.IsValid ? await Db.SaveChangesAsync() : 0);

      if (changedCount == 0) {
        return BadRequest(ModelState);
      }
      var userDetailsModel = Mapper.Map<CustomUserDetailsModel>(record);
      return Ok(userDetailsModel);
    }
  }
}
