using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MyApp.Bll.Db;
using MyApp.Bll.Models;
using MyApp.Web.Infrastructure;

namespace my_app
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            if (Environment.IsDevelopment()) {
                services.AddDbContext<MyAppContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("SqliteDb"), 
                    sql => sql.MigrationsAssembly(migrationAssembly)));
            } else {
                services.AddDbContext<MyAppContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("MyAppDb"),
                    sql => sql.MigrationsAssembly(migrationAssembly)));
            }

            services.AddIdentity<MyAppUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<MyAppContext>();
            services.AddAuthorization(RightsMapping.InitRightsAsPolicies)
                .AddSingleton<IAuthorizationHandler, ViewEditRolesHandler>();
            services.ConfigureApplicationCookie(options => options.LoginPath = "/login");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!!");
                    await SeedDatabase.Initialize(app.ApplicationServices);
                });
            });

        }
    }
}
