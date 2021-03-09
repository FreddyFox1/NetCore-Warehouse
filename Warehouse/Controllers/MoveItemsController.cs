using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Warehouse.Model;

namespace Warehouse.Controllers
{

    [Route("api/MoveItems")]
    [ApiController]
    [Authorize(Roles = "Driver, Admin, User")]

    public class MoveItemsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MoveItemsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole("Driver"))
            {
                return await DataProtector();
            }
            return Json(new
            {
                data = await _db.Items.ToListAsync(),
            });
        }

        /// <summary>
        /// 1.Первое условие проверяет защищена ли матрица и тип хранилища, 
        /// если она защищена и хранилище "Автозавод" 
        /// запретить перемещение
        /// 2.Второе условие проверяет склад Архив, если матрица в архиве то у нее заблокировано перемещение
        /// 3.Третье условие проверяет на дублирование склада, 
        /// матрица не может перемещаться если название склада совпадает
        /// </summary>
        /// <param name="id">Уникальный идентифкатор Item'a</param>
        /// <param name="StorageName">Имя склада на который перемещается Item</param>
        /// <param name="UserName">Имя пользователя, который перемещает</param>
        /// <returns>Возвращает Json ответ для всплывающего окна</returns>
        [HttpDelete]
        public async Task<IActionResult> MoveItem(int id, string StorageName, string UserName)
        {
            var Temp = await _db.Items.FirstOrDefaultAsync(a => a.ItemID == id);
            if (Temp.ItemProtect == true && StorageName == "Автозавод")
            {
                return Json(new
                {
                    success = false,
                    message = "Ошибка! Позиция только для собственной формовки."
                });
            }

            else
            {
                if (!(Temp.ItemStorageID == "Архив"))
                {
                    if (StorageName != Temp.ItemStorageID)
                    {
                        ItemLog log = new ItemLog()
                        {
                            LogItemName = Temp.ItemName,
                            LogItemArticle = Temp.ItemArticle,
                            LogUserName = UserName,
                            LogOldStorage = Temp.ItemStorageID,
                            LogCurStorage = StorageName,
                            LogDateTransfer = DateTime.Now
                        };

                        await _db.ItemLogs.AddAsync(log);
                        Temp.ItemStorageID = StorageName;
                        Temp.DateTransferItem = DateTime.Now;

                        if (Temp == null)
                        {
                            return Json(new { success = false, message = "Ошибка перемещения" });
                        }
                        await _db.SaveChangesAsync();
                        return Json(new
                        {
                            success = true,
                            message = "Успешное перемещение"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Невозможно переместить матрицу на один и тот же склад"
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Только администратор системы может перемещать матрицы из архива."
                    });
                }
            }
        }
        /// <summary>
        /// Cкрывает часть данных для закрытых позиций.
        /// </summary>
        /// <returns>Возварщает запрашиваемые Item'ы</returns>
        private async Task<IActionResult> DataProtector()
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
            return Json(new { data = await result.ToListAsync() });
        }
    }
}

