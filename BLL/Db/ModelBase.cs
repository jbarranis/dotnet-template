using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MyApp.Bll.Db
{
  public abstract class ModelBase : IModelBase
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
  }

  public interface IModelBase
  {
      int Id { get; set; }
  }
}