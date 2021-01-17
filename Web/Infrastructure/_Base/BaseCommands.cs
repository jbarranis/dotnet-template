using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MyApp.Bll.Db;
using MyApp.Web.Infrastructure.Base;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MyApp.Web.Infrastructure
{
  public abstract class GetCommand
  {
    protected MyAppContext Db { get; }
    protected IMapper Mapper { get; }

    public GetCommand(MyAppContext db, IMapper mapper)
        => (Db, Mapper) = (db, mapper);
  }

  public abstract class UpdateRequest<TDM, TModel> : IRequest<IActionResult>
    where TDM: DetailsModelBase 
    where TModel : ModelBase
  {
    public UpdateRequest(TDM viewModel, TModel record, ModelStateDictionary state)
      => (ViewModel, Record, ModelState) = (viewModel, record, state);

    public TDM ViewModel { get; }
    public TModel Record { get; }
    public ModelStateDictionary ModelState { get; }
  }
}
