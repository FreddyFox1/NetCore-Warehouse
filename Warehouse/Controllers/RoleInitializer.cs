using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Warehouse.Controllers
{
    public class RoleInitializer : Controller
    {
        /// <summary>
        /// Создаем 4 типа пользователей
        /// от админа до водителя
        /// </summary>
        /// <param name="roleManager">Сервис для работы с ролями пользователей приложения</param>
        /// <returns></returns>
        public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
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
