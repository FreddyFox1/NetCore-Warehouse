using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Model;

namespace Warehouse.Controllers
{
    [Route("api/Items")]
    [ApiController]
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private IWebHostEnvironment _environment;

        public ItemsController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        //Загрузка таблицы с данными
        [HttpGet]
        public async Task<IActionResult> GetAll(string filter)
        {
            if (User.IsInRole("Driver"))
            {
                return await DataProtector(filter);
            }
            else
            {
                if (filter == "NaN")
                {
                    return Json(new { data = await _db.Items.Where(a => a.Category.CategoryName == null).ToListAsync() });
                }
                else if (filter != "All")
                {
                    return Json(new { data = await _db.Items.Where(a => a.Category.CategoryName == filter).ToListAsync() });
                }
                else
                {
                    return Json(new { data = await _db.Items.ToListAsync() });
                }
            }
        }

        //Удаление записи из базы данных
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (User.IsInRole("Driver") || User.IsInRole("Watcher"))
            {
                return Json(new { success = false, message = "Ошибка. Недостаточно прав для удаления" });
            }

            else
            {
                var Item = await _db.Items.FirstOrDefaultAsync(u => u.ItemID == id);
                if (Item.ItemPhoto != "noimage.jpg")
                    DeleteItemImages(Item.ItemPhoto);
                if (Item == null)
                {
                    return Json(new { success = false, message = "Ошибка удаления" });
                }
                _db.Items.Remove(Item);
                await _db.SaveChangesAsync();
                return Json(new { success = true, message = "Успешное удаление" });
            }
        }

        //Удаляет изображения удаленных позиций со склада
        private void DeleteItemImages(string _path)
        {
            string filePath_Icon = Path.Combine(_environment.WebRootPath, "img", "Icons", _path);
            System.IO.File.Delete(filePath_Icon);
            string filePath_Photo = Path.Combine(_environment.WebRootPath, "img", "Photo", _path);
            System.IO.File.Delete(filePath_Photo);
        }

        ///<summary>
        ///Защита данных от пользователя с ролью "Driver", скрывает часть данных для закрытых позиций.
        ///</summary>
        private async Task<IActionResult> DataProtector(string filter)
        {
            var Data = _db.Items;
            var ProtectedItems = Data.Where(a => a.ItemProtect == true);
            var FreeItems = Data.Where(a => a.ItemProtect == false);
            var result = FreeItems.Union(ProtectedItems);

            foreach (var Item in ProtectedItems)
            {
                Item.ItemPhoto = "noimage.jpg";
                Item.ItemDescription = "Закрытая позиция";
                Item.ItemName = "Пусто";
                Item.ItemSizes = "-";
            }

            if (filter == "NaN")
            {
                return Json(new { data = await result.Where(a => a.Category.CategoryName == null).ToListAsync() });
            }
            else if (filter != "All")
            {
                return Json(new { data = await result.Where(a => a.Category.CategoryName == filter).ToListAsync() });
            }
            else
            {
                return Json(new { data = await result.ToListAsync() });
            }
        }
    }
}