using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Bll.Models;

namespace MyApp.Bll.Db
{
  public class SeedDatabase
  {
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
      using (var serviceScope = serviceProvider.CreateScope())
      {
        var context = serviceScope.ServiceProvider.GetService<MyAppContext>();

        // seed roles tables
        string[] roles = new string[] { "MyAppUser", "Standard", "Premium" };

        foreach (string role in roles)
        {
          var roleStore = new RoleStore<IdentityRole>(context);

          if (!context.Roles.Any(r => r.Name == role))
          {
            await roleStore.CreateAsync(new IdentityRole(role));
          }
        }

        // seed users tables
        MyAppUser[] users = {
          new MyAppUser { AccountType = 2, Email = "jones@test.com", NormalizedEmail = "JONES@TEST.COM", UserName = "jones", NormalizedUserName = "JONES", SecurityStamp = Guid.NewGuid().ToString("D") },
          new MyAppUser { AccountType = 3, Email = "smith@test.com", NormalizedEmail = "SMITH@TEST.COM", UserName = "smith", NormalizedUserName = "SMITH", SecurityStamp = Guid.NewGuid().ToString("D") },
        };
        CustomUser[] customUsers = {
          new CustomUser { FirstName = "Mr", LastName = "Jones" },
          new CustomUser { FirstName = "Ms", LastName = "Smith" },
        };

        UserManager<MyAppUser> _userManager = serviceScope.ServiceProvider.GetService<UserManager<MyAppUser>>();
        var currentUser = 0;
        foreach (var user in users) {
          if (!context.Users.Any(u => u.UserName == user.UserName))
          {
            var password = new PasswordHasher<MyAppUser>();
            var hashed = password.HashPassword(user, "SomePassword!1");
            user.PasswordHash = hashed;
            var result = await _userManager.CreateAsync(user);

            var assigned = await AssignRoles(_userManager, user.Email, user.AccountType);
            if (assigned != null) {
              await CreateCustomUser(user.Id, customUsers[currentUser], context);
              ++currentUser;
            }
          }
        }
      }
    }

    public static async Task<IdentityResult> AssignRoles(UserManager<MyAppUser> _userManager, string email, int accountType)
    {
      MyAppUser user = await _userManager.FindByEmailAsync(email);
      if (user != null) {
        var role = accountType == 3 ? "Premium" : "Standard";
        await _userManager.AddToRoleAsync(user, "MyAppUser");
        var result = await _userManager.AddToRoleAsync(user, role);
        return result;
      }
      return null;
    }
    public static async Task CreateCustomUser(int id, CustomUser userData, MyAppContext context)
    {
      var customUser = new CustomUser {
        MyAppUserId = id,
        FirstName = userData.FirstName,
        LastName = userData.LastName,
      };
      context.Add<CustomUser>(customUser);
      await context.SaveChangesAsync();
    }
  }
}