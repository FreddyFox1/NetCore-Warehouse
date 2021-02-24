using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Areas.Identity.Data;

namespace Warehouse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Добавляем контекст для базы данных, и MS SQL в качестве СУБД
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(
                 Configuration.GetConnectionString("DefaultConnection")));
            //Добавляем поддержку MVC  
            services.AddMvc().AddRazorRuntimeCompilation();
            //Добавляем поддержку страниц Razor 
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //Добавляем политику авторизации  
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminArea",
                     policy => policy.RequireRole("Admin"));
            });
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Включаем поддержку статических файлов
            app.UseStaticFiles();

            app.UseCors();
            app.UseRouting();

            //Подключаем сервисы аутентификации и авторизации
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

        }
    }
}
