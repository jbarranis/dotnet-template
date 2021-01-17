using System;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Infrastructure.Login
{
  public class LoginModel
  {
    [Required]
    public string Username { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
  }
}
