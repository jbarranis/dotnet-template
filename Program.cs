using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyApp.Bll.Db;
using Z.EntityFramework.Extensions;
using MyApp.Web.Infrastructure;

namespace my_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            InitializeDb(host);
            host.Run();
        }

        static void InitializeDb(IWebHost host)
        {
            var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            EntityFrameworkManager.ContextFactory = (_ => services.GetRequiredService<MyAppContext>());

            var db = services.GetRequiredService<MyAppContext>();
            DbInitializer.Initialize(db);
            RightsMapping.InitRoles(db, services.GetRequiredService<IConfiguration>());
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
