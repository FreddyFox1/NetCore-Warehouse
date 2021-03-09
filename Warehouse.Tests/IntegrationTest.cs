using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Warehouse.Model;

namespace Warehouse.Tests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected readonly WebApplicationFactory<Startup> factory;
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(ApplicationDbContext));
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });

            factory = appFactory;
            TestClient = appFactory.CreateClient();
            //appFactory.Services.GetService<ApplicationDbContext>();
        }

        protected void GetContext(Action<ApplicationDbContext> test)
        {
            using (var scope = factory.Server.Host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                test(context);
            }
        }

        protected ApplicationDbContext GetContext()
        {
            //using (var scope = factory.Server.Host.Services.CreateScope())
            //{
            var context = factory.Services.GetService<ApplicationDbContext>();// ServiceProvider.GetRequiredService<ApplicationDbContext>();
                return context;
            
        }

    }
}
