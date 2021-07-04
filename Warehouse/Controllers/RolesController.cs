using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<WarehouseUser> _userManager;
        private readonly ApplicationDbContext _db;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<WarehouseUser> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        /// <summary>
        /// Страница список ролей
        /// </summary>
        /// <returns>Возвращает представление для списка ролей</returns>
        public IActionResult Index() => View(_roleManager.Roles.ToList());

        /// <summary>
        /// Страница создания роли
        /// </summary>
        /// <returns>Возвращает представление для создания роли</returns>
        public IActionResult Create() => View();
        
        /// <summary>
        /// Создание роли
        /// </summary>
        /// <param name="name">Имя новой роли</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        public IActionResult UserList() => View(_userManager.Users.ToList());

        /// <summary>
        /// Страница редактирования ролей пользователя
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Возвращает представление для редактирования</returns>
        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            WarehouseUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }

        /// <summary>
        /// Страница редактирования ролей пользователя
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <param name="roles">Список ролей</param>
        /// <param name="userName">Имя изменяемого пользователя</param>
        /// <returns>Возвращает страницу Admin/Index в случае успеха</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles, string userName)
        {
            string _NormalizeName = "";
            // получаем пользователя
            WarehouseUser user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                _NormalizeName = await _userManager.GetEmailAsync(user);
                await _userManager.SetUserNameAsync(user, userName);
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                //_userManager.NormalizeName(_NormalizeName);
                var _user = _db.Users.FirstOrDefault(a => a.Id == userId);
                _user.NormalizedUserName = _NormalizeName;
                await _db.SaveChangesAsync();
                return RedirectToPage("/Account/Manage/Admin", new { area = "Identity" });
            }
            return NotFound();
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Уникальный идентификатор удаляемого пользователя</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            WarehouseUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!_userManager.IsInRoleAsync(user, "Admin").Result)
                {
                    await _userManager.DeleteAsync(user);
                    return RedirectToPage("/Account/Manage/Admin", new { area = "Identity" });
                }
            }
            return RedirectToPage("/Account/Manage/Admin", new { area = "Identity" });
        }
    }
}
