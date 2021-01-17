using System;
using System.Linq;
using MyApp.Bll.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MoreLinq;
using MutableLookup;

namespace MyApp.Web.Infrastructure
{
    public static class RightsMapping
    {
        public static HashSetLookup<Role, Right> ViewRights { get; } = new HashSetLookup<Role, Right>();
        public static HashSetLookup<Role, Right> EditRights { get; } = new HashSetLookup<Role, Right>();

        public static void InitRoles(MyAppContext db, IConfiguration config)
        {
            InitRolesInDb(db);
        }

        private static void InitRolesInDb(MyAppContext db)
        {
            var existingRoles = db.Roles.ToList();
            var roleNames = Enum.GetNames(typeof(Role));
            roleNames
                .Where(name => !existingRoles.Any(er => er.Name == name))
                .ForEach(name => db.Roles.Add(new IdentityRole<int>(name) { NormalizedName = name.ToUpper() }));
            existingRoles
                .Where(er => roleNames.Contains(er.Name) == false)
                .ForEach(er => db.Roles.Remove(er));
            db.SaveChanges();
        }

        public static void InitRightsAsPolicies(AuthorizationOptions options)
        {
            InitRightForRoles(options, Right.MyAppUser, new[] { Role.MyAppUser }, new[] { Role.MyAppUser });
        }

        static void InitRightForRoles(AuthorizationOptions options, Right right, Role[] viewRoles = null, Role[] editRoles = null)
        {
            if (viewRoles == null)
                viewRoles = GetAllRoles();
            if (editRoles == null)
                editRoles = GetAllRoles();
            options.AddPolicy(right.ToString(),
                p => p.AddRequirements(new ViewEditRolesRequirement(editRoles, viewRoles)));
            viewRoles?.ForEach(role => ViewRights.Add(role, right));
            editRoles?.ForEach(role => EditRights.Add(role, right));
        }

        private static Role[] GetAllRoles()
            => Enum.GetValues(typeof(Role)).Cast<Role>().ToArray();
    }
}