using MyApp.Web.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Data.Base
{
  public abstract class ListModelBase
  {
    public int Id { get; set; }
  }

  public abstract class DetailsModelBase : ListModelBase
  {
    
  }
}
