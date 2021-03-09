﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Warehouse.Controllers
{
    public class RoleInitializer : Controller
    {
        /// <summary>
        /// Создаем 4 роли пользователя
        /// </summary>
        /// <param name="roleManager">Сервис для работы с ролями пользователей приложения</param>
        public static async void InitializeAsync(RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (await roleManager.FindByNameAsync("User") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (await roleManager.FindByNameAsync("Watcher") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Watcher"));
            }

            if (await roleManager.FindByNameAsync("Driver") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Driver"));
            }
        }
    }
}
