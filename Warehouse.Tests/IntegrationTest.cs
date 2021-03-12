using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Warehouse.Model;
using Warehouse.Services.Bitrix24Service;

namespace Warehouse.Tests
{
    public class IntegrationTest
    {
        protected readonly HttpClient TestClient;
        protected readonly WebApplicationFactory<Startup> factory;

        /// <summary>
        /// Создаем тестовый сервер и устанавливаем сервисы
        /// </summary>
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
            
            TestClient = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                });
            }).CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            TestClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

        }

        /// <summary>
        /// Получаем тестовый контекст данных
        /// </summary>
        /// <returns>Возвращаем контекст данных</returns>
        protected ApplicationDbContext GetContext()
        {
            var context = factory.Services.GetService<ApplicationDbContext>();
            return context;
        }

        /// <summary>
        /// Получаем путь до стадартного окружения
        /// </summary>
        /// <returns>Возвращаем окружение</returns>
        protected IWebHostEnvironment GetEnvironment()
        {
            var Environment = factory.Services.GetService<IWebHostEnvironment>();
            return Environment;
        }

        /// <summary>
        /// Получаем сервис для работы с классом ключей Bitrix
        /// </summary>
        /// <returns></returns>
        protected IOptions<BitrixKeys> GetBitrixKeysOptions()
        {
            var keys = factory.Services.GetService<IOptions<BitrixKeys>>();
            return keys;
        }

        /// <summary>
        /// Получаем логгер для BitrixService
        /// </summary>
        /// <returns>Возвращает логгер</returns>
        protected ILogger<BitrixService> GetLoggerBitrix()
        {
            var logger = factory.Services.GetService<ILogger<BitrixService>>();
            return logger;
        }

    }
}
