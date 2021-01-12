using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyApp.Bll.Db;
using Microsoft.AspNetCore.Identity;

namespace MyApp.Bll.Models
{
  [Table("AspNetUsers")]
  public class MyAppUser : IdentityUser<int>, IModelBase
  {
    public int AccountType { get; set; }
    public CustomUser CustomUser { get; set; }
  }

  public abstract class BaseUser
  {
    public int Id { get; set; }
    public int MyAppUserId { get; set; }
    public MyAppUser MyAppUser { get; set; }
    [StringLength(120)]
    public string FirstName { get; set; }
    [StringLength(120)]
    public string LastName { get; set; }
  }

  public class CustomUser : BaseUser
  {
    // custom properties here
  }
}