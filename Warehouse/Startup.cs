using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using Warehouse.Model;
using Warehouse.Services.Bitrix24Service;
using Warehouse.Services.Bitrix24Service.Abstractions;
using Warehouse.Services.Integrator;
using Warehouse.Services.Integrator.Abstraction;
using Warehouse.Services.TelegramService;
using static Warehouse.Controllers.AccountController;

namespace Warehouse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _env = env;
            Configuration = configuration;

            //Добавляем JSON файлы с настройками приложения
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("TelegramService.json")
                .AddJsonFile("BitrixService.json");
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TelegramKey>(Configuration.GetSection("Telegram"));
            services.Configure<BitrixKeys>(Configuration.GetSection("Bitrix"));

            //Добавляем контекст для базы данных, и MS SQL в качестве СУБД
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //var serverVersion = new MySqlServerVersion(new Version(10, 5, 4));
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseMySql(Configuration.GetConnectionString("ProdConnection"), serverVersion));

            services.AddDefaultIdentity<WarehouseUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequiredLength = 8;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                    })
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.LoginPath = new PathString("/Login");
                options.AccessDeniedPath = new PathString("/AccessDenied");
                options.LogoutPath = new PathString("/Login");
                options.SlidingExpiration = true;
            });

            //Добавляем поддержку MVC  
            services.AddMvc().AddRazorRuntimeCompilation();

            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Identity/Account/Manage", "/Profile");
                options.Conventions.AddPageRoute("/Identity/Account/Manage/ChangePassword", "/ChangePassword");
                options.Conventions.AddPageRoute("/Identity/Account/Manage/Integrator", "/Integrator");

                options.Conventions.AddPageRoute("/Identity/Account/Manage/Admin", "/Users");
                options.Conventions.AddPageRoute("/Admin/Roles/Edit", "/Users/Edit");

                options.Conventions.AddPageRoute("/Identity/Account/Manage/Bitrix", "/Bitrix");
                options.Conventions.AddPageRoute("/Identity/Account/Manage/Bitrix/Edit", "/Bitrix/Edit");
                options.Conventions.AddPageRoute("/Identity/Account/Manage/Bitrix/Delete", "/Bitrix/Delete");

                options.Conventions.AddPageRoute("/Identity/Account/Manage/Telegram", "/Telegram");
                options.Conventions.AddPageRoute("/Identity/Account/Manage/Telegram/Edit", "/Telegram/Edit");
                options.Conventions.AddPageRoute("/Identity/Account/Manage/Telegram/Delete", "/Telegram/Delete");
            });

            //Добавляем поддержку страниц Razor 
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //Добавляем политики авторизации  
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminArea",
                     policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserArea",
                     policy => policy.RequireRole("Admin", "User"));
                options.AddPolicy("WatcherArea",
                     policy => policy.RequireRole("Admin", "User", "Watcher"));
                options.AddPolicy("DriverArea",
                    policy => policy.RequireRole("Driver", "Admin", "User"));
            });
            //Добавлем сервис для работы Telegram бота в фоновом режиме 
            services.AddHostedService<TelegramService>();
            //Bitrix
            services.AddTransient<BitrixService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //Включаем поддержку статических файлов
            app.UseStaticFiles();

            //CORS (Cross Origin Resource Sharing)по умолчанию в целях безопасности 
            //браузер ограничивает ajax-запросы между различными доменами
            app.UseCors(build => build.AllowAnyOrigin());

            app.UseRouting();

            //Подключаем сервисы аутентификации и авторизации
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "Users/Roles",
                        defaults: new { controller = "Roles", action = "Index" });

                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "Users/Edit/{id?}",
                        defaults: new { controller = "Roles", action = "Edit" });

                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "Users/Roles/Create",
                        defaults: new { controller = "Roles", action = "Create" });

                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "Items/Move",
                        defaults: new { controller = "MoveItems", action = "Index" });

                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
