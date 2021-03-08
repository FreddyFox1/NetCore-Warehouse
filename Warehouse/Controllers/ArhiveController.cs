using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.Model;

namespace Warehouse.Controllers
{
    [Route("api/archive")]
    [ApiController]
    [Authorize(Roles ="Admin")]

    public class ArhiveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArhiveController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFromArchive(int ID)
        {
            var Temp = await _context.Items.FirstOrDefaultAsync(a => a.ItemStorageID == "Архив");
            if (Temp != null)
            {
                Temp.ItemStorageID = "Склад не указан";
                _context.SaveChanges();
                return Json(new { success = true, message = "Позиция вернулась из небытия", text = Temp.ItemStorageID });
            }
            else return Json(new { success = false, message = "Ошибка. Позиция не находится в архиве." });
        }

    }
}

