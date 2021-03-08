﻿using System;
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
        RoleManager<IdentityRole> _roleManager;
        UserManager<WarehouseUser> _userManager;
        ApplicationDbContext _db;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<WarehouseUser> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index() => View(_roleManager.Roles.ToList());
        public IActionResult Create() => View();
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
                return RedirectToPage("/Admin/Index");
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            WarehouseUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (_userManager.IsInRoleAsync(user, "Admin").Result == false)
                {
                    await _userManager.DeleteAsync(user);
                    return RedirectToPage("/Admin/Index");
                }
            }
            return RedirectToPage("/Admin/Index");
        }
    }
}