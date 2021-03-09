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

        /// <summary>
        /// Получение списка всех Item'ов 
        /// </summary>
        /// <param name="filter">Параметр фильтрует записи по категориям</param>
        /// <returns>Возварщает запрашиваемые Item'ы</returns>
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

        /// <summary>
        /// Удаление Item'мa из базы данных
        /// </summary>
        /// <param name="id">Уникальны идентификатор Item'a</param>
        /// <returns>Возвращает Json ответ для всплывающего окна</returns>
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

        /// <summary>
        /// Функция удаляет все фотографии Item'a с сервера
        /// </summary>
        /// <param name="_filename">Имя файла для удаления</param>
        private void DeleteItemImages(string _filename)
        {
            string filePath_Icon = Path.Combine(_environment.WebRootPath, "img", "Icons", _filename);
            System.IO.File.Delete(filePath_Icon);
            string filePath_Photo = Path.Combine(_environment.WebRootPath, "img", "Photo", _filename);
            System.IO.File.Delete(filePath_Photo);
        }

        /// <summary>
        /// Cкрывает часть данных для закрытых позиций.
        /// </summary>
        /// <param name="filter">Параметр фильтрует записи по категориям</param>
        /// <returns>Возварщает запрашиваемые Item'ы</returns>
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