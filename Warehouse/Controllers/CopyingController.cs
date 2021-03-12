using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using Warehouse.Model;

namespace Warehouse.Controllers
{
    [Route("api/Items/CreateCopy")]
    [ApiController]
    [Authorize]
    public class CopyingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _environment;

        [BindProperty]
        public Item ItemCopy { get; set; }

        public CopyingController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _environment = environment;
            _context = context;
        }


        /// <summary>
        /// Создание копии копии записи Item'a
        /// </summary>
        /// <param name="id">ID копируемого Item'a </param>
        /// <returns>Возвращает json сообщение для всплывающего уведомления</returns>
        [HttpGet]
        public async Task<IActionResult> CreateCopy(int? id)
        {
            if (id != null)
            {
                try
                {
                    ItemCopy = await _context.Items.AsNoTracking().FirstOrDefaultAsync(a => a.ItemID == id);
                    ItemCopy.ItemID = 0;
                    ItemCopy.ItemName += " <b class=\"bg-warning pr-2 pl-2\">КОПИЯ</b>";
                    if (ItemCopy.ItemPhoto != "noimage.jpg")
                    {
                        ItemCopy.ItemPhoto = CreateCopyPhoto(ItemCopy.ItemPhoto);
                    }
                    var NewItem = _context.Items.Add(ItemCopy);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Матрица скопирована" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Упс! Произошла непредвиденная ошибка" });
                }
            }
            else return NotFound();
        }

        /// <summary>
        /// Копируем текущую картинку, генерируем и возвращаем новое уникальное имя.
        /// </summary>
        /// <param name="CurrentPhoto">Имя исходной(копируемоей) фотографии </param>
        /// <returns></returns>
        private string CreateCopyPhoto(string CurrentPhoto)
        {
            string FileName = Guid.NewGuid().ToString() + ".jpg";
            var PathImgFolder = Path.Combine(_environment.WebRootPath, "img", "Photo");
            var PathIconFolder = Path.Combine(_environment.WebRootPath, "img", "Icons");
            System.IO.File.Copy(Path.Combine(PathImgFolder, CurrentPhoto), Path.Combine(PathImgFolder, FileName));
            System.IO.File.Copy(Path.Combine(PathIconFolder, CurrentPhoto), Path.Combine(PathIconFolder, FileName));
            return FileName;
        }
    }
}