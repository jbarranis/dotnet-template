using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyApp.Bll.Db;
using MyApp.Bll.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DelegateDecompiler.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace MyApp.Web.Infrastructure.Base
{
  [Route("api/[controller]")]
  public abstract class BaseController<TModel, TList, TDetails> : Controller
    where TModel: class, IModelBase, new()
    where TList : ListModelBase
    where TDetails : DetailsModelBase
  {
    protected MyAppContext Db { get; }
    protected IMapper Mapper { get; }
    protected DbSet<TModel> Table { get; }
    protected IQueryable<TModel> IncludedTable { get; }
    protected IMediator Mediator { get; }
    private UserManager<MyAppUser> UserManager { get; }

    public BaseController(MyAppContext db, IMapper mapper, IMediator mediator, DbSet<TModel> table,
      IQueryable<TModel> includedTable, UserManager<MyAppUser> userManager)
      => (Db, Mapper, Mediator, Table, IncludedTable, UserManager) = (db, mapper, mediator, table, includedTable, userManager);

    protected async Task<IEnumerable<TList>> Get(Expression<Func<TModel, bool>> filter = null)
    {
      var items = IncludedTable;
      if (filter != null)
        items = items.Where(filter);
      return await items
        .ProjectTo<TList>(Mapper.ConfigurationProvider)
        .DecompileAsync()
        .ToListAsync();
    }
    
    protected virtual IActionResult VaidateAndUpdateModel(TDetails viewModel, TModel record)
    {
      Mapper.Map(viewModel, record);
      if (ModelState.IsValid == false)
        return new BadRequestObjectResult(ModelState);

      Db.SaveChanges();
      return new OkResult();
    }
  }
}