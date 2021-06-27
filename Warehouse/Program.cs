using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Warehouse.Controllers;

namespace Warehouse
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await Initializer.InitializeAsync(rolesManager);
                    var userManager = services.GetRequiredService<UserManager<Model.WarehouseUser>>();
                    await Initializer.InitializeRootUser(userManager);
                }
                catch (Exception ex)
                {
                   
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
