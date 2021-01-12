using System;
using System.ComponentModel.DataAnnotations;
using MyApp.Bll.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MyApp.Web.Data.Register
{
  public class RegisterModelProfile : Profile
  {
    public RegisterModelProfile()
    {
      CreateMap<MyAppUser, RegisterModel>()
        .ForMember(u => u.Password, x => x.Ignore())
        .ForMember(u => u.ConfirmPassword, x => x.Ignore());
      CreateMap<RegisterModel, MyAppUser>()
        .ForMember(u => u.CustomUser, x => x.Ignore())
        .ForMember(u => u.Id, x => x.Ignore())
        .ForMember(u => u.NormalizedUserName, x => x.Ignore())
        .ForMember(u => u.NormalizedEmail, x => x.Ignore())
        .ForMember(u => u.PasswordHash, x => x.Ignore())
        .ForMember(u => u.SecurityStamp, x => x.Ignore())
        .ForMember(u => u.ConcurrencyStamp, x => x.Ignore())
        .ForMember(u => u.PhoneNumber, x => x.Ignore())
        .ForMember(u => u.PhoneNumberConfirmed, x => x.Ignore())
        .ForMember(u => u.TwoFactorEnabled, x => x.Ignore())
        .ForMember(u => u.LockoutEnd, x => x.Ignore())
        .ForMember(u => u.LockoutEnabled, x => x.Ignore())
        .ForMember(u => u.AccessFailedCount, x => x.Ignore());
    }
  }
  public class RegisterModel
  {
    [Required]
    public string UserName { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
  }
}
