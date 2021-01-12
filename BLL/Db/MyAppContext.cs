using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApp.Bll.Models;

namespace MyApp.Bll.Db
{
  public class MyAppContext : IdentityDbContext<MyAppUser, IdentityRole<int>, int>
  {
    public MyAppContext(DbContextOptions<MyAppContext> options) : base(options)
    {
      Database.EnsureCreated();
      Database.Migrate();
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
    }

    public DbSet<MyAppUser> MyAppUser { get; set; }
    public DbSet<CustomUser> CustomUser { get; set; }
  }
}