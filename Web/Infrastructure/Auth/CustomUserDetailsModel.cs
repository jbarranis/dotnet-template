using System.Collections.Generic;
using MyApp.Bll.Models;
using MyApp.Web.Infrastructure.Base;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.Infrastructure.Auth
{
  public class CustomUserDetailsModelProfile : Profile
  {
    public CustomUserDetailsModelProfile()
    {
      CreateMap<CustomUser, CustomUserDetailsModel>()
        .ForMember(cdm => cdm.Id, x => x.MapFrom(src => src.MyAppUser.Id))
        .ForMember(cdm => cdm.Username, x => x.MapFrom(src => src.MyAppUser.UserName))
        .ForMember(cdm => cdm.Email, x => x.MapFrom(src => src.MyAppUser.Email))
        .ForMember(cdm => cdm.AccountType, x => x.MapFrom(src => src.MyAppUser.AccountType))
        .ForMember(cdm => cdm.FirstName, x => x.MapFrom(src => src.FirstName))
        .ForMember(cdm => cdm.LastName, x => x.MapFrom(src => src.LastName));
      CreateMap<MyAppUser, CustomUserDetailsModel>()
        .ForMember(cdm => cdm.Id, x => x.MapFrom(src => src.CustomUser.MyAppUserId))
        .ForMember(cdm => cdm.Username, x => x.MapFrom(src => src.UserName))
        .ForMember(cdm => cdm.Email, x => x.MapFrom(src => src.Email))
        .ForMember(cdm => cdm.FirstName, x => x.MapFrom(src => src.CustomUser.FirstName))
        .ForMember(cdm => cdm.LastName, x => x.MapFrom(src => src.CustomUser.LastName));
    }
  }

  public class CustomUserDetailsModel : DetailsModelBase
  {
    public string Username { get; set; }
    public string Email { get; set; }
    public string AccountType { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }
}
