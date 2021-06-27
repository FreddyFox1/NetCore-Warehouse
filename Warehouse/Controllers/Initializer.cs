using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Warehouse.Controllers
{
    public class Initializer : Controller
    {
        /// <summary>
        /// Создаем 4 роли пользователя
        /// </summary>
        /// <param name="roleManager">Сервис для работы с ролями пользователей приложения</param>
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

        public static async Task InitializeRootUser(UserManager<Model.WarehouseUser> _userManager)
        {
            Warehouse.Model.WarehouseUser root = new Model.WarehouseUser
            {
                UserName = "root@admin.ru",
                Email = "root@admin.ru",
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(root, "root1234");
            if (result.Succeeded)
            {
                var user1 = await _userManager.FindByEmailAsync(root.Email);
                if (user1 != null)
                {
                    var user = await _userManager.FindByEmailAsync(root.Email);
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
            
        }
    }
}
